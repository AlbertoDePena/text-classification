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
    NavbarState : Navbar.State
    CurrentPage : Page }

type Msg =
  | ClassificationMsg of ClassificationPage.Msg
  | LabelMsg of LabelPage.Msg
  | TextSampleMsg of TextSamplePage.Msg
  | NavbarMsg of Navbar.Msg
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
  let navbarState, navbarCmd = Navbar.init ()
  
  let currentPage = Router.currentUrl() |> parseUrl

  let initialState = 
    { ClassificationState = classificationState
      LabelState = labelState
      TextSampleState = textSampleState
      NavbarState = navbarState
      CurrentPage = currentPage }
  
  let initialCmd = Cmd.batch [
    Cmd.map ClassificationMsg classificationCmd
    Cmd.map LabelMsg labelCmd
    Cmd.map TextSampleMsg textSampleCmd
    Cmd.map NavbarMsg navbarCmd
  ]

  initialState, initialCmd

let update (msg: Msg) (state: State) =
  match msg with
  | PageChanged page -> 
    { state with CurrentPage = page }, Cmd.none

  | NavbarMsg navbarMsg -> 
    let navbarState, navbarCmd = Navbar.update navbarMsg state.NavbarState
    { state with NavbarState = navbarState }, Cmd.map NavbarMsg navbarCmd

  | ClassificationMsg classificationMsg -> 
    let classificationState, classificationCmd = ClassificationPage.update classificationMsg state.ClassificationState    
    { state with ClassificationState = classificationState }, Cmd.map ClassificationMsg classificationCmd

  | LabelMsg labelMsg ->
    let labelState, labelCmd = LabelPage.update labelMsg state.LabelState    
    { state with LabelState = labelState }, Cmd.map LabelMsg labelCmd

  | TextSampleMsg textSampleMsg ->
    let textSampleState, textSampleCmd = TextSamplePage.update textSampleMsg state.TextSampleState    
    { state with TextSampleState = textSampleState }, Cmd.map TextSampleMsg textSampleCmd

let render (state: State) (dispatch: Msg -> unit) =
  let mainElement =
    match state.CurrentPage with
    | Classification -> ClassificationPage.render state.ClassificationState (ClassificationMsg >> dispatch)
    | Labels -> LabelPage.render state.LabelState (LabelMsg >> dispatch)
    | TextSamples -> TextSamplePage.render state.TextSampleState (TextSampleMsg >> dispatch)
    | NotFound -> Html.h1 "Not found"

  React.router [
    router.onUrlChanged (parseUrl >> PageChanged >> dispatch)
    router.children [
      Navbar.render state.NavbarState (NavbarMsg >> dispatch)
      Html.main [
        prop.classes ["container"; "is-fluid"]
        prop.children [
          mainElement
        ]
      ]
    ]
  ]