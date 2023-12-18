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

    let register context client =
        registerEffect context client
        
    let registerHandler : HttpHandler =
        let handle (client:Client) : HttpHandler = run register client
        Request.mapJson handle
                
    let receive context sysUpdate = 0

module Server =
    let send context = 0


let register = post "/api/register" Client.registerHandler
let send = post "/api/status" (Response.ofPlainText "")

let status = get "/api/status" (Response.ofPlainText "")
let endpoints = [ register; send; status ]
