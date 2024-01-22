open Argu
open System
open MonitorServer.Client
open Domain


type Arguments =
    | Host of host: string
    | Key of key: string
    | Interval of number: int
    | Client of client: string

    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | Host s -> "host to connect"
            | Key s -> "api key to auth"
            | Interval s -> "interval in ms to update server"
            | Client s -> "client id to connect"


let getArg (args: string array) (name: string) (defaultValue: string) =
    let index = args |> Seq.tryFindIndex (fun i -> i = name)

    match index with
    | None -> defaultValue
    | Some i ->
        let value = args.[i + 1]
        value


let parse (args: string array) =
    // printfn "Interval: %i" (result.GetResult Interval)
    let env =
        { host = getArg args "--host" "http://localhost:5262"
          apiKey = getArg args "--key" "123"
          interval = getArg args "--interval" "3200" |> int
          clientId = Guid.Empty }

    printfn "%s" env.apiKey
    //if env.host = String.Empty || env.apiKey = String.Empty then
    //invalidate "host and apikey are required"
    //else
    succeed env


[<EntryPoint>]
let main args =

    if Seq.contains "--h" args then
        let parser =
            ArgumentParser.Create<Arguments>(programName = "MonitorServerClient.exe")

        let usage = parser.PrintUsage()
        printfn "%s" usage
        0
    else
        // printfn "Arguments passed to function : %A" args
        let output = parse args

        match output with
        | Success env -> Monitor.handle env |> Async.RunSynchronously
        | Failure f -> printfn "%s" f.ErrorMessage
        // Return 0. This indicates success.
        0
