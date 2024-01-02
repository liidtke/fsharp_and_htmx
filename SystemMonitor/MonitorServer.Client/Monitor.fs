[<RequireQualifiedAccess>]
module MonitorServer.Client.Monitor

open System
open System.Threading
open Hardware.Info

let mount (env:Env) (loaders:StatLoader list) =
    {
     deviceStats = loaders |> List.map (fun i -> i())
     date = DateTime.UtcNow
     clientId =  env.clientId
    }

let private run env t =
    printfn "run"
    let timer = new PeriodicTimer(TimeSpan.FromMilliseconds(env.interval))

    let rec work t =
        task {
            let! tick = timer.WaitForNextTickAsync(t)

            let! response = mount env Loader.loaders |> FlurlBuilder.post env "/api/status"
            match response with
            | Success systemUpdateModel -> printfn "send ok"
            | Failure errorResult -> printfn "%s" errorResult.ErrorMessage
            
            if not t.IsCancellationRequested then
                return! work t
        }
    work t

let handle env =
    async {
        let! token = Async.CancellationToken
        run env token |> Async.AwaitTask |> Async.RunSynchronously
    }
