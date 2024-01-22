module MonitorServer.Client.Loader

open Domain
open System
open System.Diagnostics

let run name =
    let start = ProcessStartInfo()
    start.CreateNoWindow <- true
    start.RedirectStandardError <- true
    start.RedirectStandardOutput <- true
    start.RedirectStandardInput <- true
    start.FileName <- "/bin/bash"
    start.Arguments <- $"-c \"{name}\""

    use proc = Process.Start(start)

    async {
        return! proc.StandardOutput.ReadToEndAsync() |> Async.AwaitTask
    }

let parseRawCPU raw = raw

let cpuTemp: StatLoader =
    let value = run "cat /proc/cpu" |> Async.RunSynchronously |> parseRawCPU
    fun () ->
        { device = "CPU"
          name = "Generic CPU"
          stat = "Temperature"
          value = "50" }

let cpuUsage: StatLoader =
    fun () ->
        { device = "CPU"
          name = "Generic CPU"
          stat = "Utilization"
          value = "1" }

let memoryUsage: StatLoader =
    fun () ->
        { device = "Memory"
          name = "RAM"
          stat = "Utilization"
          value = "30" }

let loaders: StatLoader list = [ cpuTemp; memoryUsage ]
