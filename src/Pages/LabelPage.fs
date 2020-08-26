[<RequireQualifiedAccess>]
module LabelPage

open Elmish
open Elmish.React
open Feliz

type State =
    { Labels : string list }

type Msg =
    | LoadLabels

let init () =
    { Labels = List.empty }, Cmd.none

let update (msg : Msg) (state : State) =
    state, Cmd.none    

let render (state: State) (dispatch: Msg -> unit) =
    Html.none