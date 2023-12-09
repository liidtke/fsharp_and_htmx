module Domain

open System



type Device =
    | CPU
    | Memory
    | GPU

type Stat =
    | Utilization
    | Temperature
    
type ClientId = Guid

type DeviceStat = {
    Device: Device
    Stat: Stat
    Value: int
}
type SystemUpdate = {
   DeviceStats: DeviceStat list
   Date:DateTime
   Client:ClientId
}