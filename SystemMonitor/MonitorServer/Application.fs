module Application

open System
open System.Security.Claims
open System.Text.Json
open Domain
open Falco
open Falco.Security
open System.Collections.Generic
open SharedKernel
open Microsoft.AspNetCore.Http
open System.Linq


let handleInvalidAuth: HttpHandler =
    Response.withStatusCode 401
    >> Response.ofPlainText "Unauthorized"

let secureHandler (ok: HttpHandler) : HttpHandler =
    Request.ifAuthenticated ok handleInvalidAuth


type SecuritySettings =
    { JwtSecurityKey: string
      TokenExpirationTime: int32
      JwtIssuer: string
      JwtAudience: string }

let jsonOptions = JsonSerializerOptions(JsonSerializerDefaults.Web)

//output
let errorStatusCode (serviceError: ErrorResult) =
    match serviceError.errorType with
    | NotFound -> Response.withStatusCode 404
    | Validation -> Response.withStatusCode 409
    | InvalidInput -> Response.withStatusCode 400
    | InternalError -> Response.withStatusCode 500
    | Forbidden -> Response.withStatusCode 403
    | FormValidation -> Response.withStatusCode 422

let errorHandler (serviceError: ErrorResult) : HttpHandler =
    errorStatusCode serviceError
    >> Response.ofPlainText serviceError.ErrorMessage


let htmlErrorHandler (serviceError: ErrorResult) : HttpHandler =
    match serviceError.errorHtml with
    | Some s -> errorStatusCode serviceError >> Response.ofHtml s
    | None -> errorHandler serviceError

let handleJson error : HttpHandler =
    let message = sprintf "Invalid JSON: %s" error

    Response.withStatusCode 400
    >> Response.ofPlainText message

let authHandler: HttpHandler =
    Response.withStatusCode 401
    >> Response.ofPlainText "Unauthorized"

let from serviceOutput =
    match serviceOutput with
    | Success result -> result |> Response.ofJsonOptions jsonOptions
    | Failure e -> (errorHandler e)


let fromHtml serviceOutput =
    match serviceOutput with
    | Success result -> result |> Response.ofHtml
    | Failure e -> (htmlErrorHandler e)

let fromModel toModel serviceOutput =
    match serviceOutput with
    | Success result ->
        result
        |> toModel
        |> Response.ofJsonOptions jsonOptions
    | Failure e -> errorHandler e

let badRequest text =
    Response.withStatusCode 400
    >> Response.ofPlainText text

//system
type SystemUser =
    { userId: Guid
      userName: string
      isAdmin: bool }

module SystemUser =
    let getIntValues (headers: IDictionary<string, string>) name =
        if headers.ContainsKey(name) && headers[name] <> "" then
            try
                headers[name].Split(",")
                |> List.ofArray
                |> List.map (fun x -> x |> int)
            with e ->
                List.empty
        else
            List.Empty

    let getStringValues (headers: IDictionary<string, string>) name =
        if headers.ContainsKey(name) then
            headers[name].Split(",") |> List.ofArray
        else
            List.Empty

    let standard =
        { userId = Guid.Empty
          userName = "System"
          isAdmin = true }

let getClaims (ctx: HttpContext) =
    Auth.getClaims ctx
    |> Seq.map (fun c -> (c.Type, c.Value))
    |> dict


type ServiceContext =
    {
      state:ServerState
      settings: Settings }

type ServiceHandler<'input, 'output> = 'input -> ServiceOutput<'output>

let getDependencies =
    fun (ctx: HttpContext) ->
        let env = ctx.GetService<Settings>()
        let serverState = ctx.GetService<ServerState>()
        { settings = env; state = serverState }


let keyCheck (ctx:HttpContext) (settings:Settings) =
    let env = ctx.GetService<Settings>()
    let headers = ctx.Request.Headers |> Seq.map (fun x -> x.Key, (x.Value |> Seq.head)) |> dict
    let apiKey = if headers.ContainsKey("x-api-key") then headers["x-api-key"] else ""
    let clientKey = if headers.ContainsKey("ClientKey") then headers["ClientKey"] else ""

    settings.apiKey = apiKey || settings.clientKey = clientKey


let run (serviceHandler: ServiceContext -> ServiceHandler<'input, 'output>) (input: 'input) : HttpHandler =
    // secureHandler <|
    fun (ctx: HttpContext) ->
        let services = getDependencies ctx

        match keyCheck ctx services.settings with 
        | true -> from (serviceHandler <| services <| input) ctx
        | false -> authHandler ctx

let runHtml
    (serviceHandler: ServiceContext -> ServiceHandler<'input, Falco.Markup.XmlNode>)
    (input: 'input)
    : HttpHandler =
    fun (ctx: HttpContext) ->
        let services = getDependencies ctx
        fromHtml (serviceHandler <| services <| input) ctx
        //| Failure f -> authHandler ctx
