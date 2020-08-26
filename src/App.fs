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
    LabelState : LabelPage.State
    TextSampleState : TextSamplePage.State
    CurrentPage : Page }

type Msg =
  | ClassificationMsg of ClassificationPage.Msg
  | LabelMsg of LabelPage.Msg
  | TextSampleMsg of TextSamplePage.Msg
  | PageChanged of Page

let parseUrl segments =
  match segments with
  | [] -> Classification
  | [ "labels" ] -> Labels
  | [ "text-samples" ] -> TextSamples
  | _ -> NotFound

let init () =
  let classificationState, classificationCmd = ClassificationPage.init ()
  let labelState, labelCmd = LabelPage.init ()
  let textSampleState, textSampleCmd = TextSamplePage.init ()
  
  let currentPage = Router.currentUrl() |> parseUrl

  let initialState = 
    { ClassificationState = classificationState
      LabelState = labelState
      TextSampleState = textSampleState
      CurrentPage = currentPage }
  
  let initialCmd = Cmd.batch [
    Cmd.map ClassificationMsg classificationCmd
    Cmd.map LabelMsg labelCmd
    Cmd.map TextSampleMsg textSampleCmd
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

  | LabelMsg labelMsg ->
    let labelState, labelCmd = LabelPage.update labelMsg state.LabelState
    let appCmd = Cmd.map LabelMsg labelCmd

    { state with LabelState = labelState }, appCmd

  | TextSampleMsg textSampleMsg ->
    let textSampleState, textSampleCmd = TextSamplePage.update textSampleMsg state.TextSampleState
    let appCmd = Cmd.map TextSampleMsg textSampleCmd

    { state with TextSampleState = textSampleState }, appCmd

let render (state: State) (dispatch: Msg -> unit) =
  React.router [
    router.onUrlChanged (parseUrl >> PageChanged >> dispatch)
    router.children [
      match state.CurrentPage with
      | Classification -> ClassificationPage.render state.ClassificationState (ClassificationMsg >> dispatch)
      | Labels -> LabelPage.render state.LabelState (LabelMsg >> dispatch)
      | TextSamples -> TextSamplePage.render state.TextSampleState (TextSampleMsg >> dispatch)
      | NotFound -> Html.h1 "Not found"
    ]
  ]