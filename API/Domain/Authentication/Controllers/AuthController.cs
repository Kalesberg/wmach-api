using API.Models;
using API.Utilities;
using API.Utilities.Auth;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "Authorization")]
    public class AuthController : ApiController
    {
        ///<summary>
        ///Returns JWT (JSON Web Token) if authentication is successful
        ///</summary>
        ///<param name="credentials">Credentials</param>
        ///<response code="403">Authentication Failed</response>
        ///<response code="200">Authentication Success</response>
        [HttpPost]
        [Route("api/auth")]
        public HttpResponseMessage Authenticate(Credentials credentials)
        {
            Contact contact = new Contact();
            string username = credentials.username;
            string password = credentials.password;

            if (Authentication.IsAuthenticated(username, password, out contact))
            {
                //Contact contact = null;
                string token = null;
                var secretKey = ConfigurationManager.AppSettings["AuthKey"];

                //GRAB DATA FROM AD IF IT EXISTS
                //contact = ActiveDirectory.GetActiveDirectoryContactInfo(username);

                //CHECK TO SEE IF USER HAS BEEN FLAGGED TO HAVE A LONGER TOKEN DURATION. WILL RETURN 0 IF NO RECORD FOUND.
                var loggedInDuration = Authentication.GetUserExtendedLoginDuration(username);

                //IF NO CONTACT IS FOUND THEN JUST USE PASSED IN USERNAME
                if (contact == null) token = Authentication.GetToken(username, loggedInDuration);
                else token = Authentication.GetToken(contact, loggedInDuration);

                var response = new HttpResponseMessage();
                response.StatusCode = HttpStatusCode.OK;

                var obj = JWT.JsonWebToken.DecodeToObject(token, secretKey, true);
                string json = JsonConvert.SerializeObject(obj);
                response.Content = new StringContent(json, Encoding.UTF8, "application/json");
                response.Headers.Add("Authorization", "Bearer " + token);
                return response;
            }

            //send failed notification
            SlackNotification(username);

            return Request.CreateResponse(HttpStatusCode.Unauthorized);
        }

        private static void SlackNotification(string username)
        {
            //TODO:The slack bot messages nuget plugin is not compatible with .net 4.5
            //leaving this here incase we upgrade the framework. Also, the slack webhoourl was
            //generated for a private generate, please re-generate.
            //var WebHookUrl = "https://hooks.slack.com/services/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";

            //var client = new SbmClient(WebHookUrl);

            //var message = new Message("Login Failed Attempt")
            //    .SetUserWithEmoji("Website", Emoji.Loudspeaker);
            //message.AddAttachment(new Attachment()
            //    .AddField("User Name", username, true)
            //    .AddField("Website", "m2.wwmach.com", true)
            //    .AddField("Date", DateTime.Now.ToShortDateString(), true)
            //    .AddField("IP", Utilities.IPAddress.GetIPAddress(), true)
            //    .AddField("Bio",
            //        username + "attempted to login")
            //    //.SetThumbUrl("https://wwmach.com/images/logo.png?width=500&height=500&mode=crop&anchor=top")
            //    .SetColor("#f96332")
            //);

            //client.Send(message);
            Task.Run(async () => await new Email("Login Monitor", "dev@wwmach.com", "applicationsupport@wwmach.com", "Failed Login Attempt", username + " " + DateTime.Now.ToShortDateString() + " " + Utilities.IPAddress.GetIPAddress()).Send());
            
        }

        ///<summary>
        ///Returns HTTP status to verify authentication.
        ///<remarks></remarks>
        ///</summary>
        [HttpGet]
        [JWTAuthorization]
        [Route("api/auth/check")]
        public HttpResponseMessage CheckAuth()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
