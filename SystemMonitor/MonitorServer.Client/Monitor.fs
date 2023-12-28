[<RequireQualifiedAccess>]
module MonitorServer.Client.Monitor

open System
open System.Threading
open Hardware.Info
open Flurl

type DeviceStatModel = { device: string; name:string; stat: string;value:string }
type ClientModel =
    { name: string
      id: Guid
      clientType: string }

type SystemUpdateModel =
    { deviceStats: DeviceStatModel list
      date: DateTime
      clientId: Guid }

let getSomething () =
    let hw = HardwareInfo()
    hw.RefreshCPUList() |> ignore
    let cpu = hw.CpuList |> Seq.head
    let core = cpu.CpuCoreList |> Seq.head
    printf "%s" cpu.Name
    0
    

let private run (interval: int) t =
    printfn "run"
    let timer = new PeriodicTimer(TimeSpan.FromMilliseconds(interval))

    let rec work t =
        task {
            let! tick = timer.WaitForNextTickAsync(t)

            getSomething () |> ignore
            if not t.IsCancellationRequested then
                return! work t
        }
    work t

let handle interval =
    async {
        let! token = Async.CancellationToken
        run interval token |> Async.AwaitTask |> Async.RunSynchronously
    }
