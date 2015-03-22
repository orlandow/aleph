namespace Aleph.Web.Models

open Raven.Abstractions.FileSystem
open Raven.Client.Embedded
open Raven.Client.Document
open Raven.Client.FileSystem

module Store =
    open System.Threading.Tasks
    open System.Collections.Generic

    let store = new EmbeddableDocumentStore(
                        RunInMemory = true,
                        EnlistInDistributedTransactions = false)

    store.Conventions.CustomizeJsonSerializer <- FJsonConverters.converters
    store.Initialize() |> ignore

    let session() = store.OpenSession()

    let ravenfs() = 
        let fs = new FilesStore(Url = "http://localhost:8080", DefaultFileSystem = "TestFS")
        fs.Initialize(false, false)
