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
          image = None 
          code = Some "123"
          aside = Some "1986" }
    
    member this.Index () = 
        let cards = [ card(); card(); card(); card() ]
        this.View(cards)

