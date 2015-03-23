namespace Aleph.Web

open System.Web.Mvc
open System.Web.Routing

/// Route for ASP.NET MVC applications
type Route = { 
    controller : string
    action : string
    id : UrlParameter }

type Global() =
    inherit System.Web.HttpApplication() 

    static member RegisterFilters(filters: GlobalFilterCollection) =
        filters.Add(new HandleErrorAttribute())

    static member RegisterRoutes(routes:RouteCollection) =
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}")
        routes.MapRoute(
            "Default", // Route name
            "{controller}/{action}/{id}", // URL with parameters
            { controller = "Home"; action = "Index"; id = UrlParameter.Optional } // Parameter defaults
        ) |> ignore

    member x.Application_Start() =
        AreaRegistration.RegisterAllAreas()
        Global.RegisterFilters(GlobalFilters.Filters)
        Global.RegisterRoutes(RouteTable.Routes)
        DependencyResolver.SetResolver
            { new IDependencyResolver with
                member x.GetService ty =
                    if ty = typeof<IControllerActivator> 
                        then new CompositionRoot() :> _
                    else null
                 
                member x.GetServices _ = Seq.empty }