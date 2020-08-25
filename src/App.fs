module App

open Elmish
open Elmish.React
open Feliz

type State =
  { RandomNumber : DeferredState<int> }

type Msg =
  | GenerateRandomNumber of MsgStatus<int>

let init () =
  { RandomNumber = HasNotStartedYet }, Cmd.none

let update (msg: Msg) (state: State) =
  match msg with
  | GenerateRandomNumber Dispatched when state.RandomNumber = InProgress ->
    state, Cmd.none

  | GenerateRandomNumber Dispatched ->
    let generateRandom = 
      Api.generateRandom 
      |> Async.map (GenerateRandomNumber << Processed)

    { state with RandomNumber = InProgress }, Cmd.fromAsync generateRandom

  | GenerateRandomNumber (Processed randomNumber) ->
    { state with RandomNumber = Resolved randomNumber }, Cmd.none

let render (state: State) (dispatch: Msg -> unit) =
  let content =
    match state.RandomNumber with
    | HasNotStartedYet -> Html.h1 "Has not started yet!"
    | InProgress -> Html.h1 "LOADING..."
    | Resolved randomNumber -> Html.h1 randomNumber

  Html.div [
    Html.button [
      prop.disabled (state.RandomNumber = InProgress)
      prop.onClick (fun _ -> dispatch (GenerateRandomNumber Dispatched))
      prop.text "Generate Random Number"
    ]

    content
  ]

Program.mkProgram init update render
|> Program.withConsoleTrace
|> Program.withReactSynchronous "elmish-app"
|> Program.run