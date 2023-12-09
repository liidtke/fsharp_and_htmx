module Endpoints

let handlers =
    []
    @ Pages.Landing.endpoints
    @ Pages.App.endpoints
    @ Pages.Category.endpoints
    @ Pages.Target.endpoints
    @ Pages.Opening.endpoints
    @ Pages.PrivateScheduling.endpoints
