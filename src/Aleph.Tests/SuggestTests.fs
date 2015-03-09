module SuggestTests

open NUnit.Framework
open DataProviders

[<Test;Ignore("not unit test")>]
let ``can suggest`` () =
    let suggestions = 
        Providers.suggest "borges"
        |> Async.RunSynchronously
        |> Array.choose (function
            | Success s -> Some s
            | Failure e -> None )

    Assert.IsNotEmpty(suggestions)