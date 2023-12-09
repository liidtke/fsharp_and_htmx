module Pages.App

open Falco.Markup
open Falco
open Falco.Routing
open Falco.Markup
open Falco.Markup.Attr
open Pages.Components


let homeContent = [
    Elem.h2 [] [txt "Welcome Home"]
    card "Test" "texto"
    Elem.button [cl "btn"] [txt "oioi"]
    Elem.div [cl "row"] [
        Elem.div [cl "col-6"] [
        ]
        Elem.div [cl "col-6"] [txt "Right Content"]
    ]
]

let app = page "App" homeContent

let home = get "/app" (Response.ofHtml <| app )
let endpoints = [ home ]
