module Pages.PrivateScheduling

open Domain
open Falco
open Falco.Routing
open Falco.Markup
open Pages.Components
open Application

//criar / liberar horários p/ virarem agendamentos

let top =
    [ Elem.h1 [ cl "header" ] [ txt "Agendar" ]
      Elem.p [ cl "text-sm" ] [ txt "Crie ou edite agendamentos" ]
      Elem.div [ cl "divider" ] [] ]


let listing () = Elem.div [ cl "content" ] []
let adding () = Elem.div [ cl "content" ] []
let editing () = Elem.div [ cl "content" ] []

let home =
    top @ [ listing () ]
    |> page "Agendamentos"
    |> Response.ofHtml

let homeEndpoint = get "/app/agendamento" home


let endpoints = [ homeEndpoint ]

