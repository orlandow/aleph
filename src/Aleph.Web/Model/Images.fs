namespace Aleph.Web.Models

module Images =
    open Raven.Client.Document
    open Raven.Client.FileSystem
    open System.IO
    open Aleph.Web.Models
    open Raven.Json.Linq

    let get (id:string) = 
        use session = Store.fs()
        async {
            let! file = Async.AwaitTask <| session.Query().WhereEquals("Identifier", id).FirstOrDefaultAsync()
            match file with
            | null -> return None
            | file -> 
                let! stream = Async.AwaitTask <| session.DownloadAsync(file.FullPath)
                return Some stream
        } 

    let save (id:string) (stream:Stream) = 
        use session = Store.fs()

        let meta = new RavenJObject()
        meta.Add("Identifier", RavenJToken.FromObject(id))

        session.RegisterUpload(id, stream, meta)

        session.SaveChangesAsync() |> Async.awaitVoid |> Async.RunSynchronously
