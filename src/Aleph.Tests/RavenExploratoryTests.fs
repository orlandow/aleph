module RavenExploratoryTests

open NUnit.Framework
open Raven.Client.Embedded
open Geography
open System
open Aleph.Data.Converters

let store = new EmbeddableDocumentStore(RunInMemory = true)
store.Conventions.CustomizeJsonSerializer <-
    fun x ->
        x.Converters.Add(new OptionConverter())
        x.Converters.Add(new ListConverter())
        x.Converters.Add(new SetConverter())
        x.Converters.Add(new MapConverter())
        x.Converters.Add(new UnionConverter())
store.Initialize() |> ignore

let stores (obj:'a) =
    use session = store.OpenSession()
    session.Store(obj)
    session.SaveChanges()
    let id = session.Advanced.GetDocumentId(obj)

    use session = store.OpenSession()
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

    use session = store.OpenSession()
    cs |> Seq.iter (session.Store)
    session.SaveChanges()

    let ids = cs |> Seq.map (session.Advanced.GetDocumentId)

    Assert.That(ids |> Seq.pairwise |> Seq.forall (fun (x,y) -> x <> y), 
        sprintf "same ids: %A" ids)

[<Test>]
let ``can get ids for some documents`` () =
    let a = { name = "A"; continent = America }
    let b = { name = "B"; continent = America }

    use session = store.OpenSession()
    session.Store(a)
    session.Store(b)
    session.SaveChanges()

    let cs = session.Query<Country>()
    let ids = cs |> Seq.map (session.Advanced.GetDocumentId)

    Assert.That(ids |> Seq.pairwise |> Seq.forall (fun (x,y) -> x <> y), 
        sprintf "same ids: %A" ids)
