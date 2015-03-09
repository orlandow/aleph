[<AutoOpen>]
module Maybe

type MaybeBuilder() =
    member this.Bind (m, f) = Option.bind f m
    member this.Return x = Some x

let maybe = new MaybeBuilder()
