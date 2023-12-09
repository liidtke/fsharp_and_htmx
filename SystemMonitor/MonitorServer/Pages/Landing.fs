module Pages.Landing

open Falco
open Falco.Routing
open Falco.Markup
open Falco.Markup.Attr
open Pages.Components

let page title main =
    Elem.html
        []
        [ Elem.head
              []
              [ Elem.meta
                    [ name "viewport"
                      content "width=device-width, initial-scale=1, maximum-scale=1, user-scalable=0" ]
                Elem.meta [ charset "UTF-8" ]
                Elem.link
                    [ href "https://fonts.googleapis.com/css?family=Lato:400,700"
                      rel "stylesheet" ]
                Elem.link [ href "/static/landing.css"; rel "stylesheet" ]
                Elem.link [ href "https://unpkg.com/cirrus-ui"; type' "text/css"; rel "stylesheet" ]
                Elem.link
                    [ href "https://fonts.googleapis.com/css?family=Montserrat:400,700"
                      rel "stylesheet" ]
                // Elem.link
                //     [ href "https://cdn.jsdelivr.net/npm/flatpickr/dist/flatpickr.min.css"
                //       rel "stylesheet" ]
                // Elem.script [ src "/static/script.js" ] []
                Elem.script [ src "https://unpkg.com/htmx.org@1.9.7" ] []
                // Elem.script [ src "https://unpkg.com/hyperscript.org@0.9.7" ] []
                // Elem.script [ src "https://cdn.jsdelivr.net/npm/flatpickr" ] []
                Elem.title [] [ txt title ] ]
          Elem.body [] [ main ] ]


let main () =
    Elem.div [cl "u-flex u-center h-100p"] [
        Elem.div [cl "intro-card frame px-3 py-4"] [
            Elem.div [cl "frame__body"] [
                Elem.div [cl "u-flex u-center"] [
                    Elem.img [cl "logo"; src "https://raw.githubusercontent.com/Spiderpig86/Cirrus/master/img/CirrusLogo.png"]
                ]
                Elem.h3 [] [txt "Schedule Center"]
                Elem.p [] [txt "Gerenciamento e Controle de Agendamento"]
                Elem.div [cl "divider"] []
                Elem.a [href "/app"; cl "btn btn-link"] [txt "Abrir App"]
            ]
        ]
    ]

let homePage () =
    page "Home" <| main ()

let home = get "/" (Response.ofHtml <| homePage ())
let endpoints = [ home ]
