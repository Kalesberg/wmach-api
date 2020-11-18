using API.Data;
using API.Domain.ForSaleList.Manager;
using API.Models;
using API.Templates;
using API.Utilities.Auth;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Xml;

namespace API.Controllers
{
    [JWTAuthorization]
    [EnableCors("*","*","*")]
    //[RoutePrefix("api/ForSaleList")]
    public class ForSaleListController : ApiController
    {
        ///<summary>
        ///Returns forsale list Categories by user(A.Greenberg's) preference as XML
        ///then converts the XML and returns a JSON object
        ///</summary>
        [HttpGet]
        [Route("api/ForSaleList")]
        public IHttpActionResult GetForSalelist()
        {
            var db = DAL.GetInstance();
            var data = db.getForSaleListMobileProductCategory();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(data[0]);
            var _xmlDoc = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);


            //TODO: clean the serialized xml to a readable json object
            var _trimXmlDoc = _xmlDoc.Substring(_xmlDoc.LastIndexOf("string"));
            var _json = _trimXmlDoc.Replace("string", "").Replace(@"}", @"").Replace(@":", "").Substring(1).TrimStart('"').TrimEnd('"');

            return Ok(_json);

           
        }
        ///<summary>
        ///Updates the User Categories Preference
        ///</summary>
        [HttpPost]
        [Route("api/ForsaleList/{username}/UpdatePreference")]
        public HttpResponseMessage ForSaleList([FromBody] JObject json)
        {
            var data = ForsaleListManagers.UpdatePreferences(json);
            return data != null ? Request.CreateResponse(HttpStatusCode.OK, data) : Request.CreateResponse(HttpStatusCode.Forbidden);
        }
    }
}
