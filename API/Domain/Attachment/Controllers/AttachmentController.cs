using API.Data;
using API.Utilities.Auth;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using API.Models;
using Newtonsoft.Json;

namespace API.Controllers
{
    [JWTAuthorization]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AttachmentController : ApiController
    {
        ///<summary>
        ///Returns list of all attachments that match search criteria
        ///</summary>
        [HttpPost] //BREAKING REST..TOO MANY KEY/VALUE PAIRS TO SHOVE IN HTTP GET
        [Route("api/attachment/search")]
        public HttpResponseMessage GetAttachment(AttachmentSearch search)
        {
            var json = JObject.FromObject(search);
            var equipments = Builder.Build(new AttachmentSimple(), json);
            return equipments == null ? Request.CreateResponse(HttpStatusCode.NoContent) : Request.CreateResponse(HttpStatusCode.OK, equipments);
        }

        ///<summary>
        ///Returns attachment info by serial number
        ///</summary>
        [HttpGet]
        [Route("api/attachment/")]
        public HttpResponseMessage GetEquipmentBySerialNum([FromUri] string serialNum)
        {
            var tokens = new JObject { { "SerialNum", serialNum } };
            var equipment = Builder.Build(new AttachmentDetail(), tokens);
            if (equipment == null) return Request.CreateResponse(HttpStatusCode.NoContent);
            return Request.CreateResponse(HttpStatusCode.OK, equipment);
        }

        ///<summary>
        ///Returns list of all attachment categories
        ///</summary>
        [HttpGet]
        [Route("api/attachment/categories")]
        public HttpResponseMessage GetAttachmentCategories()
        {
            var db = DAL.GetInstance();
            var categories = db.getEquipmentCategories("attachment");
            return Request.CreateResponse(HttpStatusCode.OK, categories);
        }

        ///<summary>
        ///Returns list of applicable attachment manufacturers
        ///</summary>
        [HttpPost]
        [Route("api/attachment/makes")]
        public HttpResponseMessage GetAttachmentManufacturers([FromBody] JObject json)
        {
            var db = DAL.GetInstance();
            var makes = db.getEquipmentManufacturers(json, "attachment");
            return Request.CreateResponse(HttpStatusCode.OK, makes);
        }

        ///<summary>
        ///Returns list of applicable attachment models
        ///</summary>
        [HttpPost]
        [Route("api/attachment/models")]
        public HttpResponseMessage GetAttachmentModels([FromBody] JObject json)
        {
            var db = DAL.GetInstance();
            var models = db.getEquipmentModels(json, "attachment");
            return Request.CreateResponse(HttpStatusCode.OK, models);
        }

        ///<summary>
        ///Returns list of applicable attachment types
        ///</summary>
        [HttpPost]
        [Route("api/attachment/types")]
        public HttpResponseMessage GetAttachmentTypes([FromBody] JObject json)
        {
            var db = DAL.GetInstance();
            var models = db.getAttachmentTypes(json);
            return Request.CreateResponse(HttpStatusCode.OK, models);
        }
       
        ///<summary>
        ///API take different parameter will return diffierent position of categories or types
        ///</summary>
        [HttpPost]
        [Route("api/attachment/categoriesortypes")]
        public HttpResponseMessage GetAttachmentFrontTypes([FromBody] JObject json)
        {
            var db = DAL.GetInstance();
            var data = db.getAttachmentCategoriesOrTypes(json);
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

        ///<summary>
        ///API get attachment category from fitson machine cate/make/model
        ///</summary>
        [HttpPost]
        [Route("api/attachment/categories/fitson")]
        public HttpResponseMessage GetAttachmentCategoryfromFitsOn([FromBody] JObject json)
        {
            var db = DAL.GetInstance();
            var data = db.GetAttachmentCategoryfromFitsOn(json);
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }
    }
}