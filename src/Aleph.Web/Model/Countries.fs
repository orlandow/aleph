namespace Aleph.Web.Models

[<RequireQualifiedAccess>]
module Countries =
    open Store

    let insert () =
        use session = Store.session()
        session.Store({ name = "Cuba"; continent = America })
        session.Store({ name = "France"; continent = Europe })
        session.SaveChanges()

    let all = 
        use session = Store.session()
        
        session.Query<Country>()
        |> Seq.map (fun c -> Card.fromTitle <| c.name)

