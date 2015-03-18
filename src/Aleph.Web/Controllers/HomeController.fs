namespace Aleph.Web.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Web
open System.Web.Mvc
open Aleph.Web.Models

type HomeController() =
    inherit Controller()

    let card () =
        { title = "This is the title"
          desc = Some "this is some long text for description" 
          data = [(Clock, "hello world"); (Clock, "something else")]
          image = None }
    
    member this.Index () = 
        let cards = [ card(); card() ]
        this.View(cards)

