using API.Data;
using API.Models;
using API.Utilities.Auth;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using API.Managers;

namespace API.Controllers
{
    [JWTAuthorization]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class OpportunityController : ApiController
    {
        ///<summary>
        ///Returns opportunity by opportunityID
        ///</summary>
        [HttpGet]
        [Route("api/opportunities/{opportunityID}")]
        public HttpResponseMessage GetLostRentals(string opportunityID)
        {
            var db = DAL.GetInstance();
            var json = new JObject();
            json.Add("OpportunityID", opportunityID);
            var data = db.GetOpportunity(json);
            return data != null ? Request.CreateResponse(HttpStatusCode.OK, data) : Request.CreateResponse(HttpStatusCode.NoContent);
        }

        ///<summary>
        ///Creates a lost rental opportuntity
        ///</summary>
        [HttpPost]
        [Route("api/opportunities")]
        public HttpResponseMessage CreateLostOpportunity([FromBody] JObject json)
        {
            var db = DAL.GetInstance();
            var token = Request.Headers.Authorization.Parameter;
            json.Add("UserName", Authentication.GetUserName(token));
            var created = db.CreateOpportunity(json);
            if (created) return Request.CreateResponse(HttpStatusCode.Created);
            else return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        ///<summary>
        ///Returns static list of lost opportunity reasons
        ///</summary>
        [HttpGet]
        [Route("api/opportunities/reasons/lost")]
        public HttpResponseMessage GetLostOpportunitiesReasons()
        {
           var db = DAL.GetInstance();
           var data = db.GetOpportunityReasons();
           return Request.CreateResponse(HttpStatusCode.OK, data);
        }

        ///<summary>
        ///Returns all lost opportunity for mobile
        ///</summary>
        [HttpGet]
        [Route("api/opportunities")]
        public HttpResponseMessage GetAlltOpportunities()
        {
            var db = DAL.GetInstance();
            var data = db.GetAllOpportunities();
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

        ///<summary>
        ///Returns new opportunity manual and linked quote for mobile
        ///</summary>
        [HttpPost]
        [Route("api/newopportunity")]
        public HttpResponseMessage CreateNewLostOpportunity(OpportunityMobile json)
        {
            var db = DAL.GetInstance();
            var created = OpportunityManager.Create(json);
            if (created != 0) return Request.CreateResponse(HttpStatusCode.Created);
            else return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        ///<summary>
        ///Returns opportuinty detail by opportunityid
        ///</summary>
        [HttpGet]
        [Route("api/opportunity/{opportunityid}")]
        public HttpResponseMessage GetQuotesDetailByQuoteID(int opportunityid)
        {
            var sqlParams = new JObject { { "OpportunityID", opportunityid } };
            var quote = Builder.Build(new OpportunityDetail(), sqlParams);
            return quote == null ? Request.CreateResponse(HttpStatusCode.NoContent) : Request.CreateResponse(HttpStatusCode.OK, quote);

        }

        ///<summary>
        ///Returns update opportunity
        ///</summary>
        [HttpPost]
        [Route("api/updateopportunity")]
        public HttpResponseMessage UpdateNewLostOpportunity(OpportunityMobile json)
        {
            var db = DAL.GetInstance();
            var updated = OpportunityManager.Update(json);
            if (updated) return Request.CreateResponse(HttpStatusCode.OK);
            else return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        ///<summary>
        ///Returns disable opportunity
        ///</summary>
        [HttpPost]
        [Route("api/opportunity/deactive")]
        public HttpResponseMessage DeactiveOpportunity([FromBody]JObject json)
        {
            var db = DAL.GetInstance();
            var jobject = new JObject();
            jobject.Add("OpportunityID", json["OpportunityID"]);
            jobject.Add("EnterUserStr", "WWM\\"+json["EnterUserStr"]);
            var DeavativeResult = db.DeactiveOpportunity(jobject);
            return DeavativeResult ? Request.CreateResponse(HttpStatusCode.OK, DeavativeResult) : Request.CreateResponse(HttpStatusCode.InternalServerError);
        }
    }
}