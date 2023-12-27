module Endpoints

let handlers =
    []
    @ Pages.Home.endpoints
    @ Service.endpoints

