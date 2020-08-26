[<RequireQualifiedAccess>]
module ClassificationPage

open Elmish
open Feliz

type TextSample = 
    { Text : string
      Labels : string list }

type State =
    { TextSamples : DeferredState<TextSample list> }

type Msg =
    | LoadTextSamples of MsgStatus<TextSample list>

let init () =
    { TextSamples = HasNotStartedYet }, Cmd.none

let update (msg : Msg) (state : State) =
    state, Cmd.none    

let render (state: State) (dispatch: Msg -> unit) =
    match state.TextSamples with
    | HasNotStartedYet -> Html.none
    | InProgress -> Html.h1 "Loaiding"
    | Resolved textSamples -> Html.h1 "I Have data"