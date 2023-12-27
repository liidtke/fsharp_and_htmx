open Argu
open System
open MonitorServer.Client

type Env = { host: string; apiKey: string; interval: int }

type Arguments =
    | Host of host:string
    | ApiKey of apikey:string
    | Interval of number:int

    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | Host s -> "host to connect"
            | ApiKey s -> "api key to auth"
            | Interval s -> "interval in ms to update server"

let parse (parser: ArgumentParser<Arguments>) (args: string array) =
    let result = parser.Parse [| "--host"; "http://localhost:5262"; "--apikey"; "123"; "--interval"; "1400" |]

    printfn "Interval: %i" (result.GetResult Interval)
    let env =
        { host = result.GetResult Host
          apiKey = result.GetResult ApiKey
          interval = result.GetResult Interval
           }

    //if env.host = String.Empty || env.apiKey = String.Empty then
        //invalidate "host and apikey are required"
    //else
    succeed env


[<EntryPoint>]
let main args =
    let parser = ArgumentParser.Create<Arguments>(programName = "MonitorServer.Client")

    if Seq.contains "-hp" args then
        let usage = parser.PrintUsage()
        printfn "%s" usage
        0
    else    
        printfn "Arguments passed to function : %A" args
        let output = parse parser args

        match output with
            | Success env -> Monitor.handle env.interval |> Async.RunSynchronously 
            | Failure f -> printfn "%s" f.ErrorMessage
        // Return 0. This indicates success.
        0
