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
        store.Initialize() |> ignore
        fs.Initialize() |> ignore
        { session = store.OpenSession 
          fs = fs.OpenAsyncSession }

module Server =
    open Raven.Client.Document

    let memory () =
        let store = new EmbeddableDocumentStore(
                            RunInMemory = true,
                            EnlistInDistributedTransactions = false)
        let fs = store.FilesStore

        Raven.fromRaven (store, fs)

    let local () =
        let url = "http://localhost:8080" 
        let store = new DocumentStore(
                        Url = url,
                        DefaultDatabase = "Aleph")
        let fs = new FilesStore(
                        Url = url,
                        DefaultFileSystem = "Aleph")

        Raven.fromRaven (store, fs)
