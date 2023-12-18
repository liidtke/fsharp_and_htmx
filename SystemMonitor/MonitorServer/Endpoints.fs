module Endpoints

let handlers =
    []
    @ Pages.Landing.endpoints
    @ Service.endpoints

