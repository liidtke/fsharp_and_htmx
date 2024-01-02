module Pages.Home

open Components
open Falco
open Falco.Routing
open Falco.Markup
open Application
open Falco.Markup.Attr
open Models
open Pages.Components

let cardItem (item: DeviceStatModel) =
    //Elem.article [] [ txt $"{item.device}: {item.value}" ]
    card item.device item.value


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
               [ Elem.div [ cl "grid" ] (List.map cardItem item.deviceStats) ])

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
