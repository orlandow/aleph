namespace Aleph.Web

open System.Web.Mvc
open System.Web.Routing
open System
open Aleph.Web.Controllers
open Aleph.Web.Models
open Aleph.Data
open Images

type CompositionRoot() =
    interface IControllerActivator with

        member x.Create(request, controllerType) = 
            let raven = Server.local()
            let imager = Images.imager raven

            if controllerType = typeof<HomeController> then
                new HomeController() :> IController
            elif controllerType = typeof<CountriesController> then
                new CountriesController(raven, imager) :> IController
            elif controllerType = typeof<ImagesController> then
                new ImagesController(imager) :> IController
            else
                failwith <| sprintf "unknown controller type %s" controllerType.Name
        
