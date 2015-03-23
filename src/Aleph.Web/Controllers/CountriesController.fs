namespace Aleph.Web.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Web
open System.Web.Mvc
open Aleph.Web.Models
open Aleph.Data
open Images

type CountriesController(raven, imager) =
    inherit Controller()

    let toCard (c:Country, id:string) = 
        { Card.fromTitle c.name with image = Some id; desc = Some <| sprintf "%A" c.continent }

    member this.Index () = 
        use session = raven.session()
        let countries = 
            session.Query<Country>().Take(100)
            |> Seq.map (fun c -> (c, session.Advanced.GetDocumentId c))

        let cards = countries |> Seq.map toCard

        this.View(cards)

    [<HttpPost>]
    member this.Create (name, continent, flag:HttpPostedFileBase) =
        use session = raven.session()
        let c = { name = name; continent = continent |> Reflection.toDU<Continent>  }
        session.Store(c)
        session.SaveChanges()

        let id = session.Advanced.GetDocumentId c

        imager.save (id, flag.InputStream) |> Async.RunSynchronously

        this.RedirectToAction("Index")

