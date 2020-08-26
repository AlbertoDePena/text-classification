[<RequireQualifiedAccess>]
module Navbar

open System
open Elmish
open Feliz
open Feliz.Router

type State =
    { IsTouchDevice : bool }

type Msg =
    | ToggleTouchDevice

let init () =
    { IsTouchDevice = false }, Cmd.none  
    
let update (msg: Msg) (state: State) =    
    match msg with
    | ToggleTouchDevice ->
        { state with IsTouchDevice = not state.IsTouchDevice }, Cmd.none

let render (state: State) (dispatch: Msg -> unit) =
    let isActiveClass = if state.IsTouchDevice then "is-active" else String.Empty

    Html.nav [
        prop.classes ["navbar"; "is-fixed-top"; "is-primary"]
        prop.role "navigation"
        prop.ariaLabel "main navigation"
        prop.children [
            Html.div [
                prop.classes ["navbar-brand"]
                prop.children [
                    Html.a [
                        prop.classes ["navbar-item"]
                        prop.text "Text Classification"
                        prop.href (Router.format(""))
                    ]
                    Html.a [
                        prop.role "button"
                        prop.classes ["navbar-burger"; "burger"; isActiveClass]
                        prop.ariaLabel "menu"
                        prop.ariaExpanded false
                        prop.onClick (fun _ -> dispatch ToggleTouchDevice)
                        prop.children [
                            Html.span [ prop.ariaHidden true ]
                            Html.span [ prop.ariaHidden true ]
                            Html.span [ prop.ariaHidden true ]
                        ]
                    ]
                ]
            ]
            Html.div [
                prop.classes ["navbar-menu"; isActiveClass]
                prop.children [
                    Html.div [
                        prop.classes ["navbar-start"]
                    ]
                    Html.div [
                        prop.classes ["navbar-end"]
                        prop.children [
                            Html.a [
                                prop.classes ["navbar-item"]
                                prop.text "Labels"
                                prop.href (Router.format("labels"))
                            ]
                            Html.a [
                                prop.classes ["navbar-item"]
                                prop.text "Text Samples"
                                prop.href (Router.format("text-samples"))
                            ]
                        ]
                    ]
                ]
            ]
        ]
    ]
