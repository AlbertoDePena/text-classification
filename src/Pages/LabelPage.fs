[<RequireQualifiedAccess>]
module LabelPage

open Elmish
open Feliz

type Label = Label of string

type State =
    { Labels : DeferredState<Label list> }

type Msg =
    | LoadLabels of MsgStatus<Label list>

let init () =
    { Labels = HasNotStartedYet }, Cmd.none

let update (msg : Msg) (state : State) =
    state, Cmd.none    

let render (state: State) (dispatch: Msg -> unit) =
    match state.Labels with
    | HasNotStartedYet -> Html.none
    | InProgress -> Html.h1 "Loading"
    | Resolved labels -> Html.h1 "I Have data"