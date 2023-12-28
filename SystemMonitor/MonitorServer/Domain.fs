module Domain

open System


type Device =
    | CPU
    | Memory
    | GPU
    | Disk

type Stat =
    | Utilization
    | Temperature
    | Available

type ClientId = Guid

type DeviceStat =
    { device: Device
      name: string
      stat: Stat
      value: string }

type SystemUpdate =
    { deviceStats: DeviceStat list
      date: DateTime
      clientId: ClientId }

type Settings = { apiKey: string; clientKey: string }

type ClientType =
    | Source
    | Display

type Client =
    { name: string
      id: Guid
      sourceId: Guid option
      clientType: ClientType }

type ClientRegistration =
    { mutable clients: Client list }

    member this.AddOne(one: Client) =
        this.clients <- (this.clients @ [ one ])

    member this.RemoveOne() = ignore

type ServerState =
    { mutable lastSystemUpdate: SystemUpdate
      mutable history: SystemUpdate list
      clientRegistration: ClientRegistration }

module ServerState =
    let initial =
        { lastSystemUpdate =
            { deviceStats = []
              date = DateTime.Now
              clientId = Guid.Empty }
          history = []
          clientRegistration = { clients = [] } }
