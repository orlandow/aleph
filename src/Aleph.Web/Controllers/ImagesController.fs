namespace Aleph.Web.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Web
open System.Web.Mvc
open System.Web.Helpers

type ImagesController() =
    inherit Controller()

    member this.Get(id:string, width, height) = 
        let img = new WebImage(this.Server.MapPath("~/assets/imgs/question.jpg"))
        img.Resize(width, height, true)
           .Crop(1,1)
           .Write() 
           |> ignore