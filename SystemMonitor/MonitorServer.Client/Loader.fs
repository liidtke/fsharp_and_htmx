module MonitorServer.Client.Loader

open Domain
open System


let cpuTemp: StatLoader =
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
