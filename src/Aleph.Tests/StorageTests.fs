module SerializationTests

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
    let cuba = { id = Guid.NewGuid(); name = "Cuba"; continent = America }

    stores cuba
    stores Africa
    stores { id = Guid.NewGuid(); name = "Havana"; country = cuba }
    stores <| CountryRegion cuba
