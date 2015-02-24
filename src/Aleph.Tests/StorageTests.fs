module SerializationTests

open NUnit.Framework
open Raven.Client.Embedded
open Geography
open System

let store = new EmbeddableDocumentStore(RunInMemory = true)
store.Initialize() |> ignore

let stores (obj:'a) =
    use session = store.OpenSession()
    session.Store(obj, "id")
    session.SaveChanges()

    use session = store.OpenSession()
    let loaded = session.Load<'a>("id")

    Assert.AreEqual(obj, loaded)


[<Test>]
let ``can store a country`` () =
    let c = { id = Guid.NewGuid(); name = "Cuba"; continent = America }
    stores c