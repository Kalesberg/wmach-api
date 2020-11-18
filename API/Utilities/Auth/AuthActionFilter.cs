using API.Manager;
using Newtonsoft.Json;
using System;
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
    //CLASS FOR JWTAuthorization METHOD ATTRIBUTE, VERIFIES SECURITY TOKEN
    public class JWTAuthorization : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext ctx)
        {
#if (!DEBUG)
            try
            {
                if (ctx.Request.Headers.Authorization != null)
                {
                    string token = ctx.Request.Headers.Authorization.Parameter;
                    if (IsTokenValid(token))
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

            catch (Exception e)
            {
                throw;
            }

#   endif
        }

        private bool IsTokenValid(string token)
        {
            try
            {
                //WILL TROW EXCEPTION IF DECODE FAILS
                var secretKey = ConfigurationManager.AppSettings["AuthKey"];
                string jsonPayload = JWT.JsonWebToken.Decode(token, secretKey);
                //M1-1070

                //Convert jsonPayload to JsonObject
                var JsonObject = JsonConvert.DeserializeObject<dynamic>(jsonPayload);

                //Check if token come from internal or external user
                //If token is external decline access 

                return JsonObject["IsExt"] == "1" ? false : true;

            }
            catch (JWT.SignatureVerificationException)
            {
                return false;
            }
        }
    }

    public class CustomerAuthorization : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext ctx)
        {

            try
            {
                if (ctx.Request.Headers.Authorization != null)
                {
                    string token = ctx.Request.Headers.Authorization.Parameter;
                    if (IsCustomerTokenValid(token))
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

        private bool IsCustomerTokenValid(string token)
        {
            try
            {
                //WILL TROW EXCEPTION IF DECODE FAILS
                var secretKey = ConfigurationManager.AppSettings["CustomerPortalAuth"];
                string jsonPayload = JWT.JsonWebToken.Decode(token, secretKey);
                //M1-1070

                //Convert jsonPayload to JsonObject
                var JsonObject = JsonConvert.DeserializeObject<dynamic>(jsonPayload);

                //Check if token come from internal or external user
                //If token is external decline access 

                var IsExt = JsonObject["IsExt"] == "1" ? true : false;

                if (IsExt)
                {
                    //Check if external user has CompanyOwner as role
                    if (JsonObject["Role"].ToString() == "CompanyOwner") return true;
                    else return false;
                }

                else
                {
                    return false;
                }
            }
            catch (JWT.SignatureVerificationException)
            {
                return false;
            }
        }
    }

}