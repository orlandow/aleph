namespace Aleph.Web.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Web
open System.Web.Mvc
open System.Web.Helpers
open Aleph.Web.Models
open Images

type ImagesController(imager) =
    inherit Controller()

    let [<Literal>] nothing = "~/assets/imgs/nothing.png"

    let getImage id =
        async {
            let! result = imager.get id

            return result |> Option.map (fun s -> new WebImage(s))
        } |> Async.RunSynchronously

    member this.Get(identifier, width, height) = 
        let path = this.Server.MapPath(nothing)
        let img = getImage identifier |> defaultArg <| new WebImage(path)

        img.Resize(width, height)
           .Crop(1,1)
           .Write() |> ignore

