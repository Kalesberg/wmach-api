using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace API.Utilities.Auth
{
    public class QAAuthorization : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext ctx)
        {

            try
            {
                if (ctx.Request.Headers.GetValues("QA-Signature").First() != null)
                {
                    string token = ctx.Request.Headers.GetValues("QA-Signature").First();
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


        }

        private bool IsHashValid(string token)
        {
            try
            {
                var dt = DateTime.Today.Date.ToShortDateString();
                string hashedobj = StringHash.ComputeHash(dt);

                return token == hashedobj;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}