module Service


open Domain
open Application
open Falco
open Falco.Routing
open Application

module Client =

    let register context = 0 
    let receive context sysUpdate = 0

module Server =
    let send context = 0
      

let register = post "/api/register" (Response.ofPlainText "")
let send = post "/api/status" (Response.ofPlainText "")

let status = get "/api/status" (Response.ofPlainText "")
let endpoints = [ register; send; status ]
