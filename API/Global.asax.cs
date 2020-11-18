using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configuration.Formatters.Clear();
            GlobalConfiguration.Configuration.Formatters.Add(new JsonMediaTypeFormatter());
            GlobalConfiguration.Configure(WebApiConfig.Register);
            string Env = System.Configuration.ConfigurationManager.ConnectionStrings["mach1"].ConnectionString;
            if (Env.Contains("L3SQ"))
                SelectPdf.GlobalProperties.LicenseKey = System.Configuration.ConfigurationManager.AppSettings["SelectPDFKey"];
        
        }
    }
}
