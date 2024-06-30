using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Shebenka
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("Zvonki", "Zvonki/{action}/{name}", new { controller = "Zvonki", action = "Telefons", name = UrlParameter.Optional }, new[] { "Shebenka.Controllers" });
            routes.MapRoute("Shop", "Shop/{action}/{name}", new { controller = "Shop", action = "Products", name = UrlParameter.Optional }, new[] { "Shebenka.Controllers" });
            routes.MapRoute("Account", "Account/{action}/{id}", new { controller = "Account", action = "Index", id = UrlParameter.Optional }, new[] { "Shebenka.Controllers" });
            routes.MapRoute("Default", "", new { controller = "Shop", action = "Products" }, new[] { "Shebenka.Controllers" });
            routes.MapRoute("Info", "Info/{action}/{id}", new { controller = "Info", action = "Index", id = UrlParameter.Optional }, new[] { "Shebenka.Controllers" });
        }
    }
}
