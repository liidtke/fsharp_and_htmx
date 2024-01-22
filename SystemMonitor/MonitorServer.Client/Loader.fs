module MonitorServer.Client.Loader

open Domain
open System
open System.Diagnostics


module Raw =

    let run name =
        async {
            use p = new Process()
            let inf = ProcessStartInfo()
            inf.CreateNoWindow <- true
            inf.RedirectStandardError <- true
            inf.RedirectStandardOutput <- true
            inf.RedirectStandardInput <- true
            inf.FileName <- "/bin/bash"
            inf.Arguments <- $"-c \"{name}\""

            let proc = Process.Start(inf)

            return! proc.StandardOutput.ReadToEndAsync() |> Async.AwaitTask
        }

    let getRawSensors () =
        let value = run "sensors" |> Async.RunSynchronously
        // printfn "%s" value
        value

    let parseRawCPUTemp (raw: string) =
        let keyword = "Tctl"
        let lines = raw.Split("\n")
        // printfn "%i" lines.Length

        let line = lines |> Seq.find (fun i -> i.Contains(keyword))
        // printfn "%s" line

        let plusIndex = line.IndexOf("+")

        if plusIndex > -1 then
            line[plusIndex + 1 .. plusIndex + 4]
        else
            raw


    let rawCpuStat () =
        run "mpstat -P all" |> Async.RunSynchronously

    let parseRawCPUUtilization (raw: string) =
        let values = raw.Split(" ", StringSplitOptions.RemoveEmptyEntries) |> Seq.toList
        let index = values |> Seq.findIndex (fun i -> i = "all")
        printf "index:%i"
        values.[index + 1].Replace(",", ".")

    let getMemoryUtilization () =
        run "free | grep Mem | awk '{print $3/$2 * 100.0}'" |> Async.RunSynchronously

open Raw

let cpuTemp value : StatLoader =
    fun () ->
        { device = "CPU"
          name = "Generic CPU"
          stat = "Temperature"
          value = value }

let cpuUsage value : StatLoader =
    fun () ->
        { device = "CPU"
          name = "Generic CPU"
          stat = "Usage"
          value = value }

let memoryUsage value : StatLoader =
    fun () ->
        { device = "Memory"
          name = "RAM"
          stat = "Usage"
          value = value }

let loaders () : StatLoader list =
    let rawSensors = getRawSensors ()
    let cpuValue = rawSensors |> parseRawCPUTemp
    // let cpuUtilization = rawCpuStat () |> parseRawCPUUtilization
    
    // printfn "%s" cpuValue
    // printfn "%s" cpuUtilization

    [ cpuTemp cpuValue
      // cpuUsage cpuUtilization
      memoryUsage <| getMemoryUtilization () ]
