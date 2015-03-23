module RavenExploratoryTests

open NUnit.Framework
open Raven.Client.Embedded
open Geography
open People
open Raven.Abstractions.FileSystem
open System.IO
open Aleph.Data

let raven = Server.memory()

let stores (obj:'a) =
    use session = raven.session()
    session.Store(obj)
    session.SaveChanges()
    let id = session.Advanced.GetDocumentId(obj)

    use session = raven.session()
    let loaded = session.Load<'a>(id)

    Assert.AreEqual(obj, loaded)

[<Test>]
let ``can store geography types`` () =
    stores { name = "Cuba"; continent = America }
    stores Africa

[<Test>]
let ``documents have ids even when there is no id on record`` () =
    let c name = { name = name; continent = America }
    let cs = [1..10] |> List.map (fun i -> c <| sprintf "country%i" i) 

    use session = raven.session()
    cs |> Seq.iter (session.Store)
    session.SaveChanges()

    let ids = cs |> Seq.map (session.Advanced.GetDocumentId)

    Assert.That(ids |> Seq.pairwise |> Seq.forall (fun (x,y) -> x <> y), 
        sprintf "same ids: %A" ids)

[<Test>]
let ``can get ids for some documents`` () =
    let a = { name = "A"; continent = America }
    let b = { name = "B"; continent = America }

    use session = raven.session()
    session.Store(a)
    session.Store(b)
    session.SaveChanges()

    let cs = session.Query<Country>()
    let ids = cs |> Seq.map (session.Advanced.GetDocumentId)

    Assert.That(ids |> Seq.pairwise |> Seq.forall (fun (x,y) -> x <> y), 
        sprintf "same ids: %A" ids)

[<Test>]
let ``can store people`` () =
    let p = { 
        name = "Borges"
        fullName = "Jorge Luis Borges"
        lifespan = Dead (Day (24, August, 1899), Day (14, June, 1986))
        country = { name = "Argentina"; continent = America }
        profession = Profession "Writer"
    }

    stores p
