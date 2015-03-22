namespace Aleph.Web.Models

module Images =
    open Raven.Client.Document
    open Raven.Client.FileSystem
    open System.IO
    open Aleph.Web.Models

    let store = Store.ravenfs()
    
    let get (id:string) = 
        use session = store.OpenAsyncSession()
        async {
            let! file = Async.AwaitTask <| session.Query().WhereEquals("Id", id).FirstOrDefaultAsync()
            match file with
            | null -> return None
            | file -> 
                let! stream = Async.AwaitTask <| session.DownloadAsync(file.Name)
                return Some stream
        } 

    let save (id:string) (data:byte[]) = 
        use session = store.OpenAsyncSession()
        let stream = new MemoryStream(data)
        session.RegisterUpload(id, stream)
        session.SaveChangesAsync().RunSynchronously()
