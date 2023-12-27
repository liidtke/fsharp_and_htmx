module Pages.Components

open Falco.Markup
open Falco.Markup.Attr

let cl = class'
let txt = Text.raw
let hxGet = create "hx-get"
let hxPost = create "hx-post"
let hxSwap = create "hx-swap"
let hxTrigger = create "hx-trigger"
let hxTarget = create "hx-target"
let hxSelect = create "hx-select"
let hxIndicator = create "hx-indicator"
let hxPush = create "hx-push-url" <| "true"
let swapOuter = hxSwap "outerHTML"
let hypr = create "_"

let iden = Attr.id
let place = Attr.placeholder
let tp = Attr.type'

let card title text =
    Elem.article
        [ cl ""; style "" ]
        [ Elem.div
              [ cl "content u-text-center pt-3" ]
              [ Elem.p [ cl "title mt-0 mb-0" ] [ txt title ]; Elem.p [ cl "" ] [ txt text ] ] ]

let page title ct =
    let main =
        Elem.main
            [ cl "container" ]
            (ct
             @ [ Elem.div [ iden "message"; cl "toaster-wrapper" ] [] ])

    let body = [ main ]

    Elem.html
        [create "data-theme" <| "light"]
        [ Elem.head
              []
              [ Elem.meta
                    [ name "viewport"
                      content "width=device-width, initial-scale=1, maximum-scale=1, user-scalable=0" ]
                Elem.meta [ charset "UTF-8" ]
                Elem.link
                    [ href "https://fonts.googleapis.com/css?family=Lato:400,700"
                      rel "stylesheet" ]
                Elem.link [ href "/static/app.css"; rel "stylesheet" ]
                Elem.link
                    [ href "https://cdn.jsdelivr.net/npm/@picocss/pico@1/css/pico.min.css"
                      type' "text/css"
                      rel "stylesheet" ]
                Elem.link
                    [ href "https://fonts.googleapis.com/css?family=Nunito+Sans:200,300,400,600,700"
                      rel "stylesheet" ]
                Elem.link
                    [ href "https://fonts.googleapis.com/css?family=Montserrat:400,700"
                      rel "stylesheet" ]
                // Elem.link
                //     [ href "https://cdn.jsdelivr.net/npm/flatpickr/dist/flatpickr.min.css"
                //       rel "stylesheet" ]
                Elem.script [ src "/static/app.js" ] []
                Elem.script [ src "https://unpkg.com/htmx.org@1.9.7" ] []
                Elem.script [ src "https://unpkg.com/hyperscript.org@0.9.7" ] []
                // Elem.script [ src "https://cdn.jsdelivr.net/npm/flatpickr" ] []
                Elem.title [] [ txt title ] ]
          Elem.body [] body ]


let toToaster (output: ServiceOutput<'t>) =
    let boxClass =
        match output with
        | Success foo -> "dark"
        | Failure errorResult -> "warning"

    let message =
        match output with
        | Success s -> "Salvo com Sucesso"
        | Failure f -> "Erro: " + f.ErrorMessage

    Elem.div
        [ cl "toaster-wrapper"; id "message" ]
        [ Elem.div
              [ cl $"toast toast--{boxClass} ml-0 mr-0 mxw-30"
                hypr "init wait 2s transition my opacity to 0% over 1 seconds then hide me" ]
              [ txt message ] ]
