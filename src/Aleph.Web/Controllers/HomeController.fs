namespace Aleph.Web.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Web
open System.Web.Mvc
open Aleph.Views.Countries

type HomeController() =
    inherit Controller()
    
    member this.Index () = 
        this.View(all)

