module Pages.Category


open Domain
open Falco
open Falco.Routing
open Falco.Markup
open Pages.Components
open Application

let top =
    [ Elem.h1 [ cl "header" ] [ txt "Categorias de Agendamento" ]
      Elem.p [ cl "text-sm" ] [ txt "Defina categorias de atendimento para auxiliar no gerenciamento dos agendamentos" ]
      Elem.div [ cl "divider" ] [] ]

let listing (categories: Category list) (showToaster: bool) =
    Elem.div
        [ iden "content" ]
        [ Elem.h5 [] [ txt "Categorias Cadastradas" ]
          Elem.table
              [ cl "table striped" ]
              [ Elem.thead [] [ Elem.tr [] [ Elem.th [] [ txt "Nome" ] ] ]
                Elem.tbody
                    []
                    (categories
                     |> List.map (fun c -> Elem.tr [] [ Elem.td [] [ txt c.name ] ])) ]
          if showToaster then toToaster <| succeed "teste" else ()
          Elem.button
              [ cl "btn btn-link "
                hxGet "/part/categorias/nova"
                hxTarget "#content"
                swapOuter ]
              [ txt "Adicionar Categoria" ] ]

let adding () =
    Elem.div
        [ iden "content" ]
        [ Elem.form
              [ hxPost "/app/categorias" ]
              [ Elem.h5 [] [ txt "Cadastrar" ]
                Elem.input [ tp "text"; place "Nome da Categoria" ]
                Elem.div
                    [ cl "grid " ]
                    [ Elem.button
                          [ cl "secondary outline"
                            hxGet "/app/categorias"
                            hxTarget "#content"
                            swapOuter
                            hxSelect "#content" ]
                          [ txt "Cancelar" ]
                      Elem.button [ cl "btn ml-1" ] [ txt "Salvar" ] ] ] ]

let editing () =
    top @ [ Elem.div [ iden "content" ] [] ]

let saveCategoryHandler =
    listing Data.Dummy.categories true
    |> succeed
    |> fromHtml
// validation "Nome Incorreto"
// |> toToaster
// |> formValidation
// |> fromHtml

let editHandler =
    editing ()
    |> page "Categorias - Editar"
    |> Response.ofHtml

let homeHandler =
    top @ [ listing Data.Dummy.categories false ]
    |> page "Categorias"
    |> Response.ofHtml

let saveEndpoint = post "/app/categorias/" saveCategoryHandler

let addEndpoint = get "/part/categorias/nova" (adding () |> Response.ofHtml)

let editEndpoint = get "/app/categorias/{id}" editHandler

let homeEndpoint = get "/app/categorias" homeHandler


let endpoints = [ homeEndpoint; editEndpoint; addEndpoint; saveEndpoint ]
