[<AutoOpen>]
module MonitorServer.Client.Domain

open System

type Env = { host: string; apiKey: string; interval: int; clientId:Guid }
//later add client id

type DeviceStatModel = { device: string; name:string; stat: string; value:string }

type SystemUpdateModel =
    { deviceStats: DeviceStatModel list
      date: DateTime
      clientId: Guid }
