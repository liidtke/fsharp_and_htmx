[<RequireQualifiedAccess>]
module Models

open System
open Domain


module Device =
    let convert (d: Device) =
        match d with
        | CPU -> "CPU"
        | Memory -> "Memory"
        | GPU -> "GPU"
        | _ -> failwith "todo"

    let parse (str: string) =
        match str with
        | "CPU" -> Device.CPU
        | "Memory" -> Device.Memory
        | "GPU" -> Device.GPU
        | _ -> failwith "todo"

module Stat =
    let convert (s: Stat) =
        match s with
        | Utilization -> "Utilization"
        | Temperature -> "Temperature"
        | _ -> failwith "todo"

    let parse (str: string) =
        match str with
        | "Utilization" -> Utilization
        | "Temperature" -> Temperature
        | _ -> failwith "todo"

type DeviceStatModel = { device: string; stat: string }

type SystemUpdateModel =
    { deviceStats: DeviceStatModel list
      date: DateTime
      client: Guid }

module SystemUpdate = 
    let convert (s: SystemUpdate) : SystemUpdateModel =
        { date = s.date
          client = s.client
          deviceStats =
            s.deviceStats
            |> List.map (fun ds ->
                { device = Device.convert ds.device
                  stat = Stat.convert ds.stat }) }
    
    let parse (s: SystemUpdateModel) : SystemUpdate =
        { date = s.date
          client = s.client
          deviceStats =
            s.deviceStats
            |> List.map (fun ds ->
                { device = Device.parse ds.device
                  stat = Stat.parse ds.stat }) }
