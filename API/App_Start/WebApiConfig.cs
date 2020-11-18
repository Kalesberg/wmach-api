using API.Utilities.Auth;
using Microsoft.AspNet.OData.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            config.AddODataQueryFilter();
            config.Count().Filter().OrderBy().Expand().Select().MaxTop(null);
            config.EnableDependencyInjection();
            config.MapHttpAttributeRoutes();
            config.EnableCors();
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional },
                constraints: null
            );
        }
    }
}
