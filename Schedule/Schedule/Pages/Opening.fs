module Pages.Opening

open Domain
open Falco
open Falco.Routing
open Falco.Markup
open Pages.Components
open Application

//criar / liberar horários p/ virarem agendamentos

let top =
    [ Elem.h1 [ cl "header" ] [ txt "Horários de Agendamento" ]
      Elem.p [ cl "text-sm" ] [ txt "Configure os horários disponíveis para agendamento" ]
      Elem.div [ cl "divider" ] [] ]


let listing () = Elem.div [ cl "content" ] []
let adding () = Elem.div [ cl "content" ] []
let editing () = Elem.div [ cl "content" ] []

let home =
    top @ [ listing () ]
    |> page "Horários"
    |> Response.ofHtml

let homeEndpoint = get "/app/horarios" home


let endpoints = [ homeEndpoint ]
