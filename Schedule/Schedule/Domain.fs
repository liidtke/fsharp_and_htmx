module Domain

open System

type Duration = int
type UserId = Guid

type Client = {
    id:Guid
    isAuthorized:bool
    name:string
}

type ClientConfiguration = {
    clientId:Guid
    //config here
}

type User =
    { id: UserId
      name: string
      email: string
      password: string
      isAdmin:bool
      client: Client option
      isVerified: bool }

type TargetType =
    | UserTarget
    | NonUserTarget

type Category = { id: int; name: string }

type Visibility =
    | Private
    | Public

//tipos de agendamento
type Target =
    { targetType: TargetType
      userId: UserId option
      created: DateTime
      isActive: bool
      categories: Category list
      visibility: Visibility
      name: string }


type CreationMode =
    | Automatic
    | Manual


type Availability =
    { id: int

    }

type ScheduleCreator =
    | RegisteredUser of UserId
    | AnonymousUser of string

//horários disponíveis
type Opening =
    { id: int
      openedBy: UserId
      created: DateTime
      start: DateTime
      duration: Duration }

type Schedule =
    { opening: Opening
      created: DateTime
      createdBy: ScheduleCreator }

type Action =
    | CreateOpening
    | UpdateOpening
    | CancelOpening
    | CreateSchedule
    | ReSchedule
    | CancelSchedule
