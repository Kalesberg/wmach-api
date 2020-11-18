using System.Data;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using API.Data;
using Newtonsoft.Json.Linq;
using API.Utilities.Auth;
using API.Managers;
using API.Models;
using System.Collections.Generic;

namespace API.Controllers
{
    [JWTAuthorization]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class YardController : ApiController
    {
        /// <summary>
        /// Returns list of parent/child yards
        /// </summary>
        /// <returns>Equipment</returns>
        [HttpGet]
        [Route("api/yards")]
        public HttpResponseMessage Get()
        {
            var yards = Builder.Build(new YardListBuilder());
            return Request.CreateResponse(HttpStatusCode.OK, yards);
        }
    }
}