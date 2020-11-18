using System.Data;
using System.Net;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using API.Data;
using API.Models;
using Newtonsoft.Json.Linq;
using API.Utilities.Auth;
using API.Managers;
using System.Text;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http;

namespace API.Controllers
{
    [JWTAuthorization]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class IndustryController : ApiController
    {
        ///<summary>
        ///get all industry
        ///</summary>
        [HttpGet]
        [Route("api/industry")]
        public HttpResponseMessage getAllIndustry()
        {
            var db = DAL.GetInstance();
            var data = db.getAllIndustry();
            return data != null ? Request.CreateResponse(HttpStatusCode.OK, data) : Request.CreateResponse(HttpStatusCode.InternalServerError);
        }
    }
}