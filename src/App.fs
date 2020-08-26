[<RequireQualifiedAccess>]
module App

open Elmish
open Feliz
open Feliz.Router
open Browser.Dom

[<RequireQualifiedAccess>]
type Page =
  | Classification
  | Labels
  | TextSamples
  | NotFound

type State =
  { Classification : ClassificationPage.State
    Label : LabelPage.State
    TextSample : TextSamplePage.State
    Navbar : Navbar.State
    CurrentPage : Page }

type Msg =
  | ClassificationMsg of ClassificationPage.Msg
  | LabelMsg of LabelPage.Msg
  | TextSampleMsg of TextSamplePage.Msg
  | NavbarMsg of Navbar.Msg
  | PageChanged of Page

let parseUrl segments =
  match segments with
  | [] -> Page.Classification
  | [ "labels" ] -> Page.Labels
  | [ "text-samples" ] -> Page.TextSamples
  | _ -> Page.NotFound

let getDocumentTitle page =
  match page with
  | Page.Classification -> "Text Classification"
  | Page.Labels -> "Text Classification - Labels"  
  | Page.TextSamples -> "Text Classification - Text Samples"
  | Page.NotFound -> "Text Classification - Page Not Found"  

let init () =
  let classificationState, classificationCmd = ClassificationPage.init ()
  let labelState, labelCmd = LabelPage.init ()
  let textSampleState, textSampleCmd = TextSamplePage.init ()
  let navbarState, navbarCmd = Navbar.init ()
  
  let currentPage = Router.currentUrl() |> parseUrl

  let initialState = 
    { Classification = classificationState
      Label = labelState
      TextSample = textSampleState
      Navbar = navbarState
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
    let navbarState, navbarCmd = Navbar.update navbarMsg state.Navbar
    { state with Navbar = navbarState }, Cmd.map NavbarMsg navbarCmd

  | ClassificationMsg classificationMsg -> 
    let classificationState, classificationCmd = ClassificationPage.update classificationMsg state.Classification    
    { state with Classification = classificationState }, Cmd.map ClassificationMsg classificationCmd

  | LabelMsg labelMsg ->
    let labelState, labelCmd = LabelPage.update labelMsg state.Label    
    { state with Label = labelState }, Cmd.map LabelMsg labelCmd

  | TextSampleMsg textSampleMsg ->
    let textSampleState, textSampleCmd = TextSamplePage.update textSampleMsg state.TextSample    
    { state with TextSample = textSampleState }, Cmd.map TextSampleMsg textSampleCmd

let render (state: State) (dispatch: Msg -> unit) =
  document.title <- getDocumentTitle state.CurrentPage
  let mainElement =
    match state.CurrentPage with
    | Page.Classification -> ClassificationPage.render state.Classification (ClassificationMsg >> dispatch)
    | Page.Labels -> LabelPage.render state.Label (LabelMsg >> dispatch)
    | Page.TextSamples -> TextSamplePage.render state.TextSample (TextSampleMsg >> dispatch)
    | Page.NotFound -> Html.h1 "Not found"

  React.router [
    router.onUrlChanged (parseUrl >> PageChanged >> dispatch)
    router.children [
      Navbar.render state.Navbar (NavbarMsg >> dispatch)
      Html.main [
        prop.classes ["container"]
        prop.children [
          mainElement
        ]
      ]
    ]
  ]