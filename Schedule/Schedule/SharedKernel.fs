[<AutoOpen>]
module SharedKernel


open System
open Falco.Markup

type ErrorType =
    | NotFound
    | Validation
    | FormValidation
    | InvalidInput
    | InternalError
    | Forbidden

type ErrorResult =
    { errorType: ErrorType
      errorMessages: string list
      errorHtml: XmlNode option }

    member this.ErrorMessage = String.concat "\n" this.errorMessages

type ServiceOutput<'TSuccess> =
    | Success of 'TSuccess
    | Failure of ErrorResult

let succeed a = Success a

let fail a =
    Failure
        { errorType = ErrorType.InternalError
          errorMessages = []
          errorHtml = None }

let failWith err = Failure err

let invalidate message =
    ServiceOutput.Failure
        { errorType = ErrorType.InvalidInput
          errorMessages = [ message ]
          errorHtml = None }

let forbid message =
    Failure
        { errorType = ErrorType.Forbidden
          errorMessages = [ message ]
          errorHtml = None }

let fromError errorType errorMessage =
    Failure
        { errorType = errorType
          errorHtml = None
          errorMessages = [ errorMessage ] }

let fromException (ex: Exception) =
    Failure
        { errorType = InternalError
          errorHtml = None
          errorMessages = [ ex.Message ] }

let validation message = fromError Validation message
let notFound message = fromError NotFound message
let formValidation html =
    Failure
        { errorType = FormValidation
          errorHtml = Some(html)
          errorMessages = [] }

let addMessage (message: string) (serviceOutput: ServiceOutput<'a>) =
    match serviceOutput with
    | Success res ->
        Failure
            { errorType = ErrorType.Validation
              errorHtml = None
              errorMessages = [ message ] }
    | Failure e ->
        Failure
            { errorType = e.errorType
              errorHtml = None
              errorMessages = e.errorMessages @ [ message ] }

//add one service output to another
let join (s1: ServiceOutput<'a>) (s2: ServiceOutput<'a>) =
    match s1, s2 with
    | Success service1, Success service2 -> s2
    | Success service1, Failure e2 -> s2
    | Failure e1, Success service2 -> s1
    | Failure e1, Failure e2 ->
        Failure
            { errorType = e2.errorType
              errorHtml = None
              errorMessages = e1.errorMessages @ e2.errorMessages }

let newOutput (obj: 'b) (s1: ServiceOutput<'a>) =
    match s1 with
    | Success a -> succeed obj
    | Failure error -> fromError error.errorType error.ErrorMessage

/// apply func if success (similar to either)
let successfully func serviceOutput =
    match serviceOutput with
    | Success s -> func s
    | Failure e -> failWith e

//my bind version (uses result into next func)
/// execute next function with output if success
let may func = successfully func

//similar to successfully
/// continue execution ignoring previous output
let (&?>) output func =
    match output with
    | Success s -> func
    | Failure e -> output

/// compose two functions, only executes second if success
let (&>) s1 s2 = s1 >> may s2

let tryTo f exnHandler x =
    try
        f x |> ignore
        succeed x
    with ex ->
        exnHandler ex

let tryWith f successFunc exnHandler =
    try
        f |> successFunc
    with ex ->
        exnHandler ex


/// map one track function into two preserving output similar to map
let maybe f output = successfully (f >> succeed) output

/// mas option type to service output
let someToOutput a =
    match a with
    | None -> notFound "not found"
    | Some s -> succeed s

///map a bool function to service output with custom validation message
let mapIf f input err =
    match f input with
    | true -> succeed input
    | false -> err

/// map a bool validation to service output or runs func with param
let mayIf v msg f input =
    match v input with
    | true -> f input
    | false -> invalidate msg

//adapted from recipe prefer using previous functions

//https://fsharpforfunandprofit.com/posts/recipe-part3/

/// apply either a success function or failure function
let either successFunc failureFunc input =
    match input with
    | Success s -> successFunc s
    | Failure e -> failureFunc e

/// convert a switch function into a two-track function
let bind f = either f fail

/// pipe a two-track value into a switch function
let (>>=) x f = bind f x

/// compose two switches into another switch
let (>=>) s1 s2 = s1 >> bind s2

/// convert a one-track function into a switch
let switch f = f >> succeed

/// convert a one-track function into a two-track function
// let map f =
//    either (f >> succeed) fail

let tee f x =
    f x
    x

// convert a one-track function into a switch with exception handling
let tryCatch f exnHandler x =
    try
        f x |> succeed
    with ex ->
        exnHandler ex |> fail

// convert two one-track functions into a two-track function
let doubleMap successFunc failureFunc =
    either (successFunc >> succeed) (failureFunc >> fail)

//fsharp helpers
let isNullOrEmpty str = String.IsNullOrEmpty(str)

let parseGuid (str: string) =
    try
        Some(Guid.Parse str)
    with ex ->
        None
