[<RequireQualifiedAccess>]
module ClassificationPage

open Elmish
open Elmish.React
open Feliz

type State =
    { TextSamples : string list }

type Msg =
    | LoadTextSamples

let init () =
    { TextSamples = List.empty }, Cmd.none

let update (msg : Msg) (state : State) =
    state, Cmd.none    

let render (state: State) (dispatch: Msg -> unit) =
    Html.none