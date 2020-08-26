[<RequireQualifiedAccess>]
module App

open Elmish
open Feliz
open Feliz.Router

type Page =
  | Classification
  | Labels
  | TextSamples
  | NotFound

type State =
  { ClassificationState : ClassificationPage.State
    CurrentPage : Page }

type Msg =
  | ClassificationMsg of ClassificationPage.Msg
  | PageChanged of Page

let parseUrl segments =
  match segments with
  | [] -> Classification
  | [ "labels" ] -> Labels
  | [ "text-samples" ] -> TextSamples
  | _ -> NotFound

let init () =
  let classificationState, classificationCmd = ClassificationPage.init ()
  
  let currentPage = Router.currentUrl() |> parseUrl

  let initialState = 
    { ClassificationState = classificationState
      CurrentPage = currentPage }
  
  let initialCmd = Cmd.batch [
    Cmd.map ClassificationMsg classificationCmd
  ]

  initialState, initialCmd

let update (msg: Msg) (state: State) =
  match msg with
  | PageChanged page -> 
    { state with CurrentPage = page }, Cmd.none

  | ClassificationMsg classificationMsg -> 
    let classificationState, classificationCmd = ClassificationPage.update classificationMsg state.ClassificationState
    let appCmd = Cmd.map ClassificationMsg classificationCmd

    { state with ClassificationState = classificationState }, appCmd

let render (state: State) (dispatch: Msg -> unit) =
  React.router [
    router.onUrlChanged (parseUrl >> PageChanged >> dispatch)
    router.children [
      match state.CurrentPage with
      | Classification -> ClassificationPage.render state.ClassificationState (ClassificationMsg >> dispatch)
      | Labels -> Html.h1 "Labels page"
      | TextSamples -> Html.h1 "Text Samples page"
      | NotFound -> Html.h1 "Not found"
    ]
  ]