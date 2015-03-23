namespace Aleph.Web.Models

module Images =
    open System.IO
    open Aleph.Data
    open Raven.Json.Linq

    type Imager = {
        get: string -> Async<Stream option>
        save: string * Stream -> Async<unit>
    }

    let imager raven =
        let get id = 
            use session = raven.fs()
            async {
                let! file = Async.AwaitTask <| session.Query().WhereEquals("Identifier", id).FirstOrDefaultAsync()
                match file with
                | null -> return None
                | file -> 
                    let! stream = Async.AwaitTask <| session.DownloadAsync(file.FullPath)
                    return Some stream
            }

        let save (id, stream) = 
            use session = raven.fs()

            let meta = new RavenJObject()
            meta.Add("Identifier", RavenJToken.FromObject(id))

            session.RegisterUpload(id, stream, meta)

            session.SaveChangesAsync() |> Async.awaitVoid

        { get = get
          save = save }
