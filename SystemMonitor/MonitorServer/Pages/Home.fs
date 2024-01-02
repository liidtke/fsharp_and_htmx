module Pages.Home

open Components
open Falco
open Falco.Routing
open Falco.Markup
open Application
open Falco.Markup.Attr
open Models
open Pages.Components

let card (item:DeviceStatModel) =
    Elem.div
        [ cl "card"; style "" ]
        [ Elem.div
              [ cl "content u-text-center pt-3" ]
              [ Elem.p [ cl "title mt-0 mb-0" ] [ txt $"{item.device} - {item.stat}" ]
                Elem.p [ cl "" ] [ txt item.value ] ]
           
          Elem.div [cl "card-flag"] []
              ]

let monitor (item: SystemUpdateModel) =
    Elem.div
        [ Attr.id "monitor"
          hxGet "/monitor"
          //hxTrigger "every 2s"
          //hxSwap "outerHTML"
          //hxTarget "monitor"
          ]
        ([ Elem.h1 [] [ txt "Monitor" ] ]
         @ if item.deviceStats.Length = 0 then
               [ Elem.p [] [ txt "Nada para exibir" ] ]
           else
               [ Elem.div [ cl "monitor-grid" ] (List.map card item.deviceStats) ])

let loadMonitor output =
    match output with
    | Success s -> monitor s
    | Failure f -> Elem.p [] [ txt "Error" ]

let monitorPage: HttpHandler =
    let work context () =
        Service.Server.getLast context () |> loadMonitor |> single "Monitor" |> succeed

    runHtml work ()

let homePage () =
    page "Home" [ Elem.h1 [] [ txt "Home" ] ]

let homeEndpoint = get "/" (Response.ofHtml <| homePage ())
let monitorEndpoint = get "/monitor" monitorPage
let endpoints = [ homeEndpoint; monitorEndpoint ]
