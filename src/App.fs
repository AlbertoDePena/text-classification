[<RequireQualifiedAccess>]
module App

open Elmish
open Feliz
open Feliz.Router

type State =
  { CurrentUrl : string list }

type Msg =
  | UrlChanged of string list

let init () =
  { CurrentUrl = Router.currentUrl() }, Cmd.none

let update (msg: Msg) (state: State) =
  match msg with
  | UrlChanged segments -> 
    { state with CurrentUrl = segments }, Cmd.none

let render (state: State) (dispatch: Msg -> unit) =
  React.router [
    router.onUrlChanged (UrlChanged >> dispatch)
    router.children [
      match state.CurrentUrl with
      | [ ] -> Html.h1 "Classification Page"
      | [ "labels" ] -> Html.h1 "Labels page"
      | [ "text-samples" ] -> Html.h1 "Text Samples page"
      | _ -> Html.h1 "Not found"
    ]
  ]