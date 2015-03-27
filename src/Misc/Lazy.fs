
[<AutoOpen;RequireQualifiedAccess>]
module Lazy 

    let get (value:Lazy<'a>) = value.Value
