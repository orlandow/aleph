namespace Aleph.Web.Model

module Images =
    open Raven.Client.Document
    open Raven.Client.FileSystem
    open System.IO

    let store = new FilesStore()
    store.Initialize() |> ignore
    
    let get (id:string) = 
        use session = store.OpenAsyncSession()
        async {
            let! stream = Async.AwaitTask <| session.DownloadAsync(id)
            return match stream with
                   | null -> None
                   | stream -> Some stream
        }

    let save (id:string) (data:byte[]) = 
        use session = store.OpenAsyncSession()
        let stream = new MemoryStream(data)
        session.RegisterUpload(id, stream)
        session.SaveChangesAsync().RunSynchronously()
