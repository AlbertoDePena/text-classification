module Main

open Elmish
open Elmish.React

Program.mkProgram App.init App.update App.render
|> Program.withConsoleTrace
|> Program.withReactSynchronous "text-classification-app"
|> Program.run