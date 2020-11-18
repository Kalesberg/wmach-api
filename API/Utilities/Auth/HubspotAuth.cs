using API.Manager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace API.Utilities.Auth
{
    public class HubspotAuthorization : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext ctx)
        {
#if (!DEBUG)
            try
            {

                if (ctx.Request.Headers.GetValues("X-HubSpot-Signature").First() != null)
                {
                    string token = ctx.Request.Headers.GetValues("X-HubSpot-Signature").First();
                    if (IsHashValid(token))
                    {


                        base.OnActionExecuting(ctx);
                    }
                    else
                    {
                        ctx.Response = ctx.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    }
                }
                else
                {
                    ctx.Response = ctx.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }

            catch (Exception)
            {
                throw;
            }

#endif
        }

        private bool IsHashValid(string token)
        {
            try
            {
                string clientSecret = ConfigurationManager.AppSettings["HubspotKey"];

                var bodyStream = new StreamReader(HttpContext.Current.Request.InputStream);
                bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
                var bodyText = bodyStream.ReadToEnd();

                string concat = string.Concat(clientSecret, bodyText);
                string hashedobj = WebhookManager.ComputeHash(concat);

                return token == hashedobj;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}