namespace Aleph.Data

open Raven.Client.FileSystem
open Raven.Client
open Raven.Client.Embedded

type Raven = {
    session: unit -> IDocumentSession
    fs: unit -> IAsyncFilesSession
}
with 
    static member fromRaven (store:IDocumentStore, fs:IFilesStore) =
        store.Conventions.CustomizeJsonSerializer <- FJsonConverters.converters

        { session = store.OpenSession 
          fs = fs.OpenAsyncSession }

module Server =
    open Raven.Client.Document

    let memory () =
        let store = new EmbeddableDocumentStore(
                            RunInMemory = true,
                            EnlistInDistributedTransactions = false)
        store.Initialize() |> ignore
        
        let fs = store.FilesStore
        fs.Initialize() |> ignore

        Raven.fromRaven (store, fs)

    let local () =
        let url = "http://localhost:8080" 
        let store = new DocumentStore(
                        Url = url,
                        DefaultDatabase = "Aleph")
        let fs = new FilesStore(
                        Url = url,
                        DefaultFileSystem = "Aleph")

        store.Initialize() |> ignore
        fs.Initialize(false) |> ignore

        Raven.fromRaven (store, fs)
