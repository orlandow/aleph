
[<AutoOpen;RequireQualifiedAccess>]
module Map 

    let choose chooser map =
        map
        |> Map.toList
        |> List.choose chooser
        |> Map.ofList

