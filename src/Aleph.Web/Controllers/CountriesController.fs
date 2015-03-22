namespace Aleph.Web.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Web
open System.Web.Mvc
open Aleph.Web.Models

type CountriesController() =
    inherit Controller()

    let toCard (c:Country) = 
        { Card.fromTitle c.name with image = Some <| sprintf "flags/%s" c.name }

    member this.Index () = 
        use session = Store.session()
        let countries = session.Query<Country>()

        let cards = countries |> Seq.map toCard

        this.View(cards)


