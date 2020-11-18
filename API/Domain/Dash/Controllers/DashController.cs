using API.Data;
using API.Managers;
using API.Models;
using API.Utilities;
using API.Utilities.Auth;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;


namespace API.Domain.Dash.Controllers
{
    [JWTAuthorization]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DashController : ApiController
    {
        [HttpGet]
        [Route("api/dashboard")]
        public HttpResponseMessage GetDashes()
        {
            var data = Builder.Build(new DashBuilder(), null);
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Returns current list of all machine categories
        /// </summary>
        /// <returns> list of all machine categories</returns>
        [HttpGet]
        [Route("api/dashboard/categories")]
        public HttpResponseMessage GetDashCategories()
        {
            var db = DAL.GetInstance();
            var categories = db.getEquipmentCategories("machine");
            return Request.CreateResponse(HttpStatusCode.OK, categories);
        }

        /// <summary>
        /// Returns current list of all machine categories by model
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/public/dashboard/categories")]
        public HttpResponseMessage GetDashCategoriesByModel([FromBody] JObject json)
        {
            var db = DAL.GetInstance();
            var categories = db.getMachineDataByModelNum(json);
            return Request.CreateResponse(HttpStatusCode.OK, categories);
        }

        ///<summary>
        ///Returns list of all applicable machine manufacturers
        ///</summary>
        [HttpPost]
        [Route("api/dashboard/makes")]
        public HttpResponseMessage GetDashManufacturers([FromBody] JObject json)
        {
            var db = DAL.GetInstance();
            var makes = db.getEquipmentManufacturers(json, "machine");
            return Request.CreateResponse(HttpStatusCode.OK, makes);
        }

        ///<summary>
        ///Returns list of all applicable machine models
        ///</summary>
        [HttpPost]
        [Route("api/dashboard/models")]
        public HttpResponseMessage GetDashModels([FromBody] JObject json)
        {
            var db = DAL.GetInstance();
            var models = db.getEquipmentModels(json, "machine");
            return Request.CreateResponse(HttpStatusCode.OK, models);
        }




     
    }
}
