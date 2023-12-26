module Service


open System
open System.Net.Http
open Domain
open Application
open Falco
open Falco.Routing
open Application

module Client =

    let registerEffect (context: ServiceContext) (client: Client) =
        if
            context.state.clientRegistration.clients
            |> List.exists (fun i -> i.id = client.id)
        then
            succeed client
        else
            client |> context.state.clientRegistration.AddOne |> ignore
            succeed client

    let validate client =
        if client.id = Guid.Empty then
            invalidate "invalid id"
        else
            succeed client

    let validateName client =
        if client.name = String.Empty then
            invalidate "invalid name"
        else
            succeed client

    let register context client =
        client
        |> Models.ClientModel.parse
        |> validate
        |> may validateName
        |> may (registerEffect context)
        |> maybe Models.ClientModel.convert

    let registerHandler: HttpHandler =
        let handle (client: Models.ClientModel) : HttpHandler = run register client
        Request.mapJson handle

    let receive context sysUpdate = 0

module Server =
    let updateEffect serverState up =
        serverState.lastSystemUpdate <- up
        serverState.history <- serverState.history @ [ up ]
        up

    let update context up =
        up
        |> Models.SystemUpdate.parse
        |> updateEffect context.state
        |> Models.SystemUpdate.convert
        |> succeed

    let updateHandler: HttpHandler =
        let handle (up: Models.SystemUpdateModel) : HttpHandler = run update up
        Request.mapJson handle
    let getLast context () = context.state.lastSystemUpdate |> Models.SystemUpdate.convert |> succeed
    let getHandler : HttpHandler =
        run getLast ()

let register = post "/api/client" Client.registerHandler
let send = post "/api/status" Server.updateHandler

let status = get "/api/status" Server.getHandler
let endpoints = [ register; send; status ]
