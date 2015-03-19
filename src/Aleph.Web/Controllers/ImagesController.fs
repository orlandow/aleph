namespace Aleph.Web.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Web
open System.Web.Mvc
open System.Web.Helpers
open Aleph.Web.Model

type ImagesController() =
    inherit Controller()

    member this.question() = this.Server.MapPath("~/assets/imgs/question.jpg")

    member this.Get(id, width, height) = 
        async {
            let! result = Images.get id
            let img = match result with
                      | Some img -> new WebImage(img)
                      | None -> new WebImage(this.question())

            img
                .Resize(width, height)
                .Crop(1,1)
                .Write()
                |> ignore
        } |> Async.RunSynchronously
