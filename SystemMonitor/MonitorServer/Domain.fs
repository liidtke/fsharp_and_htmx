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

type DeviceStat = { device: Device; stat: Stat }

type SystemUpdate =
    { deviceStats: DeviceStat list
      date: DateTime
      client: ClientId }

type Settings = { apiKey: string; clientKey: string }

type ClientType =
    | Source
    | Display

type Client =
    { name: string
      id: Guid
    }

type ClientRegistration =
    { mutable clients: Client list }

    member this.AddOne(one: Client) =
        this.clients <- (this.clients @ [ one ])

    member this.RemoveOne() = ignore

type ServerState =
    { lastSystemUpdate: SystemUpdate
      clientRegistration: ClientRegistration }

module ServerState =
    let initial =
        { lastSystemUpdate =
            { deviceStats = []
              date = DateTime.Now
              client = Guid.Empty }
          clientRegistration = { clients = [] } }
