[<AutoOpen>]
module MonitorServer.Client.Domain

open System

type Env = { host: string; apiKey: string; interval: int; clientId:Guid }

type DeviceStatModel = { device: string; name:string; stat: string; value:string }

type SystemUpdateModel =
    { deviceStats: DeviceStatModel list
      date: DateTime
      clientId: Guid }

type StatLoader = unit -> DeviceStatModel
