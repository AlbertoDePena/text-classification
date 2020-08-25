[<AutoOpen>]
module Extensions

open Elmish

/// Describes the status of an asynchronous operation ('state) that generates `'t` when it finishes
type DeferredState<'t> =
    | HasNotStartedYet
    | InProgress
    | Resolved of 't

/// Describes the status of an asynchronous operation ('msg) that generates `'t` when it finishes
type MsgStatus<'t> =
    | Dispatched
    | Processed of 't    

[<RequireQualifiedAccess>]
module Cmd =
   
    let fromAsync (operation: Async<'msg>) : Cmd<'msg> =
        let delayedCmd (dispatch: 'msg -> unit) : unit =
            let delayedDispatch = async {
                let! msg = operation
                dispatch msg
            }

            Async.StartImmediate delayedDispatch

        Cmd.ofSub delayedCmd

[<RequireQualifiedAccess>]
module Async =

    let singleton x = async {
        return x
    }

    let map f (computation: Async<'t>) = async {
        let! x = computation
        return f x
    }

    let bind f (computation: Async<'t>) = async {
        let! x = computation
        return! f x
    }

[<RequireQualifiedAccess>]
module DeferredState =
    
    /// Returns whether the `DeferredState<'T>` value has been resolved or not.
    let resolved = function
        | HasNotStartedYet -> false
        | InProgress -> false
        | Resolved _ -> true
    
    /// Returns whether the `DeferredState<'T>` value is in progress or not.
    let inProgress = function
        | HasNotStartedYet -> false
        | InProgress -> true
        | Resolved _ -> false
    
    /// Verifies that a `DeferredState<'T>` value is resolved and the resolved data satisfies a given requirement.
    let exists (predicate: 'T -> bool) = function
        | HasNotStartedYet -> false
        | InProgress -> false
        | Resolved value -> predicate value
        
    /// Transforms the underlying value of the input deferred value when it exists from type to another
    let map (transform: 'T -> 'U) (deferred: DeferredState<'T>) : DeferredState<'U> =
        match deferred with
        | HasNotStartedYet -> HasNotStartedYet
        | InProgress -> InProgress
        | Resolved value -> Resolved (transform value)
    
    /// Like `map` but instead of transforming just the value into another type in the `Resolved` case, it will transform the value into potentially a different case of the the `Deferred<'T>` type.
    let bind (transform: 'T -> DeferredState<'U>) (deferred: DeferredState<'T>) : DeferredState<'U> =
        match deferred with
        | HasNotStartedYet -> HasNotStartedYet
        | InProgress -> InProgress
        | Resolved value -> transform value