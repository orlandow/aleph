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

    let fileSystem = new FilesStore(Url = "http://localhost:8080", DefaultFileSystem = "TestFS")

    store.Conventions.CustomizeJsonSerializer <- FJsonConverters.converters

    fileSystem.Initialize(false, false) |> ignore
    store.Initialize() |> ignore

    let session() = store.OpenSession()

    let fs() = fileSystem.OpenAsyncSession()