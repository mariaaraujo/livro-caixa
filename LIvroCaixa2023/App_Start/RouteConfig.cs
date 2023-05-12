using Microsoft.AspNet.FriendlyUrls;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;

namespace LivroCaixa2023
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.EnableFriendlyUrls();
        }
    }
}
