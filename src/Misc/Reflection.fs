[<RequireQualifiedAccess>]
module Reflection

open Microsoft.FSharp.Reflection

let private like a b =
    let trim (x:string) = x.Trim().ToLowerInvariant()
    trim a = trim b

let toDU<'a> str =
    let t = typedefof<'a>
    let u = FSharpType.GetUnionCases(t)
            |> Array.find (fun u -> like u.Name str)
        
    FSharpValue.MakeUnion(u, [||]) :?> 'a

let toStr du =
    let t = du.GetType()
    let u,_ = FSharpValue.GetUnionFields(du, t)
    u.Name
