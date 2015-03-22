
[<AutoOpen;RequireQualifiedAccess>]
module Async 

open System.Threading.Tasks

    let awaitVoid (t:Task) = t |> Async.AwaitIAsyncResult |> Async.Ignore

