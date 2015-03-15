namespace Aleph.Web.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Web
open System.Web.Mvc
open Aleph.Web.Models

type HomeController() =
    inherit Controller()
    
    member this.Index () = 
        Countries.insert()
        this.View(Countries.all)

