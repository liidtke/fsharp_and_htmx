module Models

open System
open Domain


module Device =
    let convert (d: Device) =
        match d with
        | CPU -> "CPU"
        | Memory -> "Memory"
        | GPU -> "GPU"
        | Disk -> "Disk"
        | _ -> failwith "todo"

    let parse (str: string) =
        match str with
        | "CPU" -> Device.CPU
        | "Memory" -> Device.Memory
        | "GPU" -> Device.GPU
        | "Disk" -> Device.Disk
        | _ -> failwith "todo"

module Stat =
    let convert (s: Stat) =
        match s with
        | Usage -> "Usage"
        | Temperature -> "Temperature"
        | Available -> "Available"
        | _ -> failwith "todo"

    let parse (str: string) =
        match str with
        | "Usage" -> Usage
        | "Temperature" -> Temperature
        | "Available" -> Available
        | _ -> failwith "todo"

type DeviceStatModel = { device: string; name:string; stat: string;value:string }

type ClientModel =
    { name: string
      id: Guid
      clientType: string }

module ClientModel =
    let convert (client: Client) : ClientModel =
        { id = client.id
          name = client.name
          //add later
          clientType =
            match client.clientType with
            | Source -> "Source"
            | Display -> "Display"
            | _ -> failwith "todo" }

    let parse (client: ClientModel) : Client =
        { id = client.id
          name = client.name
          sourceId = None //add later 
          clientType =
            match client.clientType with
            | "Source" -> Source
            | "Display" -> Display
            | _ -> failwith "todo" }

type SystemUpdateModel =
    { deviceStats: DeviceStatModel list
      date: DateTime
      clientId: Guid }

module SystemUpdate =
    let convert (s: SystemUpdate) : SystemUpdateModel =
        { date = s.date
          clientId = s.clientId
          deviceStats =
            s.deviceStats
            |> List.map (fun ds ->
                { value = ds.value
                  device = Device.convert ds.device
                  name = ds.name
                  stat = Stat.convert ds.stat }) }

    let parse (s: SystemUpdateModel) : SystemUpdate =
        { date = s.date
          clientId = s.clientId
          deviceStats =
            s.deviceStats
            |> List.map (fun ds ->
                { value = ds.value
                  name = ds.name 
                  device = Device.parse ds.device
                  stat = Stat.parse ds.stat }) }
