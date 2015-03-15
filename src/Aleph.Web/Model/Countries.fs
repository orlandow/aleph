namespace Aleph.Web.Models

[<RequireQualifiedAccess>]
module Countries =
    open Store

    let insert () =
        use session = store.OpenSession()
        session.Store({ name = "Cuba"; continent = America })
        session.Store({ name = "France"; continent = Europe })
        session.SaveChanges()

    let all = 
        use session = store.OpenSession()
        
        session.Query<Country>()
        |> Seq.map (fun c -> Card.fromTitle <| c.name)

