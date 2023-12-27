[<RequireQualifiedAccess>]
module MonitorServer.Client.Monitor

open System
open System.Threading


let private run (interval: int) t =
    printfn "run"
    let timer = new PeriodicTimer(TimeSpan.FromMilliseconds(interval))

    let rec work t =
        task {
            let! tick = timer.WaitForNextTickAsync(t)

            printfn "executing thing here"
            
            if not t.IsCancellationRequested then
                return! work t
        }
    work t

let handle interval =
    async {
        let! token = Async.CancellationToken
        run interval token |> Async.AwaitTask |> Async.RunSynchronously
    }
