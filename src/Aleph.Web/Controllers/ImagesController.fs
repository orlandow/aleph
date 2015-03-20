namespace Aleph.Web.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Web
open System.Web.Mvc
open System.Web.Helpers
open Aleph.Web.Models

type ImagesController() =
    inherit Controller()

    let [<Literal>] questionPath = "~/assets/imgs/question.jpg"

    let getImage id =
        async {
            let! result = Images.get id

            return result |> Option.map (fun s -> new WebImage(s))
        } |> Async.RunSynchronously

    member this.Get(id, width, height) = 
        let path = this.Server.MapPath(questionPath)
        let img = getImage id |> defaultArg <| new WebImage(path)

        img.Resize(width, height)
           .Crop(1,1)
           .Write() |> ignore

