module Pages.Target

open Domain
open Falco
open Falco.Routing
open Falco.Markup
open Pages.Components
open Application

let top =
    [ Elem.h1 [ cl "header" ] [ txt "Tipos de Agendamento" ]
      Elem.p [ cl "text-sm" ] [ txt "Defina os tipos de agendamentos disponíveis" ]
      Elem.div [ cl "divider" ] [] ]


let listing () = Elem.div [ cl "content" ] []
let adding () = Elem.div [ cl "content" ] []
let editing () = Elem.div [ cl "content" ] []

let home =
    top @ [ listing () ]
    |> page "Tipos de Agendamento"
    |> Response.ofHtml

let homeEndpoint = get "/app/tipos-agendamento" home


let endpoints = [ homeEndpoint ]
