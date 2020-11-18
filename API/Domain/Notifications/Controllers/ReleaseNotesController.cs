using API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using Newtonsoft.Json.Linq;
using API.Utilities.Auth;

namespace API.Controllers
{
    [JWTAuthorization]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ReleaseNotesController : ApiController
    {
        /// <summary>
        /// Returns all release notes by system
        /// </summary>
        /// <param name="system"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("api/releasenote/{system}")]
        public HttpResponseMessage GetAllReleaseNotes(string system)
        {
            var db = DAL.GetInstance();
            var releaseNote = db.GetAllReleaseNotes(system);
            return Request.CreateResponse(HttpStatusCode.OK, releaseNote);
        }

        /// <summary>
        /// Conditionally returns most recent release notes by system
        /// </summary>
        /// <remarks>Uses the JWT (JSON Web Token) to grab username and checks to see if release notes have already been viewed. Returns empty if previously viewed.</remarks>
        /// <param name="system"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/releasenotes/{system}/usercheck")]
        public HttpResponseMessage Get(string system)
        {
            var db = DAL.GetInstance();
            var jwt = Request.Headers.Authorization.Parameter;
            var userName = Authentication.GetUserName(jwt);
            var releaseNotes = db.GetMostRecentReleaseNotes(system, userName);
            return Request.CreateResponse(HttpStatusCode.OK, releaseNotes);
        }
    }
}