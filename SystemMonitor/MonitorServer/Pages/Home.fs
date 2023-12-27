module Pages.Home

open Components
open Falco
open Falco.Routing
open Falco.Markup
open Falco.Markup.Attr
open Pages.Components

let test = Elem.h1 [] [txt "Teste"]

let homePage () =
    page "Home" [test]

let home = get "/" (Response.ofHtml <| homePage ())
let endpoints = [ home ]
