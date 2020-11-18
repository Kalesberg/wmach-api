using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using API.Data;
using Newtonsoft.Json.Linq;
using API.Utilities.Auth;
using API.Models;

namespace API.Controllers
{
    [JWTAuthorization]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DivisionController : ApiController
    {
        ///<summary>
        ///Returns list of all divisions
        ///</summary>
        [HttpGet]
        [Route("api/divisions")]
        public HttpResponseMessage GetEquipmentDivisions()
        {
            var db = DAL.GetInstance();
            var divisions = db.getDivisions();
            return Request.CreateResponse(HttpStatusCode.OK, divisions);
        }
        [HttpGet]
        [Route("api/divisions/taxschedules")]
        public HttpResponseMessage GetDivisionTadCode()
        {
            var db = DAL.GetInstance();
            var taxIds = db.GetTaxCodes();
            return Request.CreateResponse(HttpStatusCode.OK, taxIds);
        }

        ///<summary>
        ///Returns list of all applicable divisions
        ///</summary>
        [HttpPost]
        [Route("api/divisions")]
        public HttpResponseMessage GetEquipmentDivisions([FromBody] JObject json)
        {
            var db = DAL.GetInstance();
            var divisions = db.getDivisions(json);
            return Request.CreateResponse(HttpStatusCode.OK, divisions);
        }

        ///<summary>
        ///Returns list of all applicable divisions in a hierarchical format
        ///</summary>
        [HttpGet]
        [Route("api/divisions/structured")]
        public HttpResponseMessage GetEquipmentDivisionHierarchy()
        {
            var db = DAL.GetInstance();
            var divisions = db.getDivisionsHierarchy();
            return Request.CreateResponse(HttpStatusCode.OK, divisions);
        }

        ///<summary>
        ///return division Detail by divisionID
        ///</summary>
        [HttpGet]
        [Route("api/divisions/{divisionID}")]
        public HttpResponseMessage GetContactsByAccountManager(int divisionID)
        {
            var db = DAL.GetInstance();
            var tokens = new JObject();
            tokens.Add("divisionID", divisionID);
            DivisionDetail myClients = db.getDivisionDetailByDivisionID(tokens);
            if (myClients == null) return Request.CreateResponse(HttpStatusCode.NoContent);
            return Request.CreateResponse(HttpStatusCode.OK, myClients);
        }

        ///<summary>
        ///Returns Sales Rep or Sales manager or Coordinator contactid by division id and type
        ///</summary>
        [HttpPost]
        [Route("api/divisions/salesContact")]
        public HttpResponseMessage GetSalesContactID([FromBody] JObject json)
        {
            var db = DAL.GetInstance();
            var contactID = db.getSalesContactIDByDivision(json);
            return Request.CreateResponse(HttpStatusCode.OK, contactID);
        }

        ///<summary>
        ///Returns division by salesperson contact id
        ///</summary>
        [HttpPost]
        [Route("api/divisions/salesperson")]
        public HttpResponseMessage GetDivisionBySalesPersonContactID([FromBody] JObject json)
        {
            var db = DAL.GetInstance();
            var contactID = db.getDivisionBySalesPersonContactID(json);
            return Request.CreateResponse(HttpStatusCode.OK, contactID);
        }

    }
}