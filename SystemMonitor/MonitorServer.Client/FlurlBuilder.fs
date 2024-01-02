module MonitorServer.Client.FlurlBuilder

open System.Text.Json
open Flurl.Http
open Microsoft.FSharp.Control

let private jsonOptions = JsonSerializerOptions(JsonSerializerDefaults.Web)

let private withHeader name (value: string) (request: IFlurlRequest) = request.WithHeader(name, value)

let private get (request: IFlurlRequest) = request.GetAsync() |> Async.AwaitTask

let private getJson<'T> (request: IFlurlRequest) =
    request.GetJsonAsync<'T>() |> Async.AwaitTask

let private postJson<'a> (input: 'a) (request: IFlurlRequest) =
    request.PostJsonAsync(input).ReceiveJson<'a>() |> Async.AwaitTask

let private getString (request: IFlurlRequest) =
    request.GetStringAsync() |> Async.AwaitTask

let private getBytes (request: IFlurlRequest) =
    request.GetBytesAsync() |> Async.AwaitTask

let private toJson<'T> (response: IFlurlResponse) =
    response.GetJsonAsync<'T>() |> Async.AwaitTask

let private create (env: Env) route =
    let route = env.host + route
    route.WithHeader("x-api-key", env.apiKey)

let private fromEx<'a> (e: exn) (obj: 'a) =
    if e = null then
        succeed obj
    else if e.InnerException <> null && e.InnerException :? FlurlHttpException then
        let flurl = e.InnerException :?> FlurlHttpException

        invalidate flurl.Message
    else
        invalidate e.Message

let post<'a> (env: Env) (route:string) (object: 'a) =
    async {
        try
            let! result = create env route |> postJson object

            return succeed result
        with
        | :? FlurlHttpException as e -> return fromEx e object
        | e -> return fromEx e object

    }
