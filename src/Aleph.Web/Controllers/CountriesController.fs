namespace Aleph.Web.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Web
open System.Web.Mvc
open Aleph.Web.Models

type CountriesController() =
    inherit Controller()

    let toCard (c:Country, id:string) = 
        { Card.fromTitle c.name with image = Some id }

    member this.Index () = 
        use session = Store.session()
        let countries = 
            session.Query<Country>().Take(100)
            |> Seq.map (fun c -> (c, session.Advanced.GetDocumentId c))

        let cards = countries |> Seq.map toCard

        this.View(cards)

    [<HttpPost>]
    member this.Create (name, flag:HttpPostedFileBase) =
        use session = Store.session()
        let c = { name = name; continent = America }
        session.Store(c)
        session.SaveChanges()

        let id = session.Advanced.GetDocumentId c

        Images.save id flag.InputStream

        this.RedirectToAction("Index")

