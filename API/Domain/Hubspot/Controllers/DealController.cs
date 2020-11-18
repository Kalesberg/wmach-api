using API.Data;
using API.Models;
using API.Services.HubspotService.Company.Controller; 
using API.Services.HubspotService.Deals.Helper;
using API.Services.HubspotService.Deals.Models;
using API.Utilities.Auth;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace API.Domain.Hubspot.Controllers
{
    /// <summary>
    /// Hubspot deals controller
    /// Get deals from Hubspot 
    /// Update Hubspot deals
    /// </summary>
    [JWTAuthorization]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DealController : ApiController
    {
        private readonly DealsHelper dealHelper = new DealsHelper();
 
        /// <summary>
        /// API endpoint to retrieve deals from Hubspot using the Sales Rep's contact ID
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/hubspot/deal/")]
        public HttpResponseMessage GetDealsBySalesRepId([FromUri] int contactId)
        {
            JObject sqlParams = new JObject();
            sqlParams.Add("ContactID", contactId);

            var db = DAL.GetInstance(); 
            string dealOwnerEmail = db.GetHubspotEmailByContactId(sqlParams);
            if (string.IsNullOrEmpty(dealOwnerEmail))
                return Request.CreateResponse(HttpStatusCode.NotFound, "deal owner not found!");
            // stages beore quote_accepted
            string Env = ConfigurationManager.ConnectionStrings["mach1"].ConnectionString.Contains("L3SQ") ? "Prod" : "Test";
            JObject para = new JObject { { "Enviroment", Env } };
            List<DealStages> dealstages = db.GetAllDealStageValue(para);
            string unassigned = dealstages.Find(s => s.Name.Trim() == "Unassigned Deal (no sales rep)").value;          // unassigned deal (no sales rep)
            string assigned = dealstages.Find(s => s.Name.Trim() == "Rep Assigned To Deal").value;    // rep assigned to deal
            string accepted = dealstages.Find(s => s.Name.Trim() == @"Quote Accepted--""Order""").value;  //quote accepted
            List<string> allowedStages = new List<string>
            {
                 unassigned,
                 assigned,
                 accepted
            };
            try
            {
                var associatedDealIds = db.GetAllAssociatedDealIds() ?? new List<long>();
                long ownerId = dealHelper.GetDealOwnerId(dealOwnerEmail);
                if(ownerId == 0)
                    return Request.CreateResponse(HttpStatusCode.NotFound, "deal owner not found!");

                var deals = dealHelper.GetDeals(ownerId.ToString(), allowedStages); 
                deals = deals.Where(d => !associatedDealIds.Contains(d.dealId)).ToList();

                var response = deals.Select(d => new
                {
                    dealId = d.dealId,
                    dealName = d.properties.dealname.value
                }).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.ToString());
            }
           
        }

        /// <summary>
        /// update some deal properties on Hubspot
        /// </summary>
        [HttpPut]
        [Route("api/hubspot/deal/")]
        public HttpResponseMessage UpdateDeal([FromBody] DealUpdateRequestDTO deal)
        {
            var isUpdated = dealHelper.UpdateDeal(deal);
            if (isUpdated)
                return Request.CreateResponse(HttpStatusCode.OK);
           
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Deal update failed!");
        }

        /// <summary>
        /// get a deal by dealid
        /// </summary>
        [HttpGet]
        [Route("api/hubspot/deal/{dealID}")]
        public HttpResponseMessage GetADeal(long dealID)
        {
            var dealName = dealHelper.GetADealbyDealID(dealID);
            if (dealName != "")
                return Request.CreateResponse(HttpStatusCode.OK, dealName);

            return Request.CreateResponse(HttpStatusCode.BadRequest, "Deal Not Found!");
        }

        /// <summary>
        ///create deal
        /// </summary>
        [HttpPost]
        [Route("api/hubspot/deal/")]
        public HttpResponseMessage CreateADeal(DealCreateRequestDTO newDeal)
        {
           
            var dealID = dealHelper.CreateADeal(newDeal);
            if (dealID > 0)
                return Request.CreateResponse(HttpStatusCode.OK, dealID);

            return Request.CreateResponse(HttpStatusCode.InternalServerError, "Deal Creation Failed!");
        }

        /// <summary>
        /// API endpoint to retrieve deals from Hubspot using the Sales Rep's contact ID
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/hubspot/deal/contract")]
        public HttpResponseMessage GetDealsBySalesRepIdForContract([FromUri] int contactId)
        {
            JObject sqlParams = new JObject();
            sqlParams.Add("ContactID", contactId);

            var db = DAL.GetInstance();
            string dealOwnerEmail = db.GetHubspotEmailByContactId(sqlParams);
            if (string.IsNullOrEmpty(dealOwnerEmail))
                return Request.CreateResponse(HttpStatusCode.NotFound, "deal owner not found!");
            // stages beore Contract_accepted
            string Env = ConfigurationManager.ConnectionStrings["mach1"].ConnectionString.Contains("L3SQ") ? "Prod" : "Test";
            JObject para = new JObject { { "Enviroment", Env } };
            List<DealStages> dealstages = db.GetAllDealStageValue(para);
            string unassigned = dealstages.Find(s => s.Name.Trim() == "Unassigned Deal (no sales rep)").value;          // unassigned deal (no sales rep)
            string assigned = dealstages.Find(s => s.Name.Trim() == "Rep Assigned To Deal").value;    // rep assigned to deal
            string accepted = dealstages.Find(s => s.Name.Trim() == @"Quote Accepted--""Order""").value;  //quote accepted
            string quoted = dealstages.Find(s => s.Name.Trim() == "Quoted").value;   // quoted
            List<string> allowedStages = new List<string>
            {
                 unassigned,
                 assigned,
                 accepted,
                 quoted
            };
            try
            {
                var associatedDealIds = db.GetAllAssociatedDealIdsForContract() ?? new List<long>();
                long ownerId = dealHelper.GetDealOwnerId(dealOwnerEmail);
                if (ownerId == 0)
                    return Request.CreateResponse(HttpStatusCode.NotFound, "deal owner not found!");

                var deals = dealHelper.GetDeals(ownerId.ToString(), allowedStages);
                deals = deals.Where(d => !associatedDealIds.Contains(d.dealId)).ToList();

                var response = deals.Select(d => new
                {
                    dealId = d.dealId,
                    dealName = d.properties.dealname.value
                }).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.ToString());
            }

        }


        /// <summary>
        /// update some deal properties on Hubspot
        /// </summary>
        [HttpPut]
        [Route("api/hubspot/deal/contract")]
        public HttpResponseMessage UpdateDealForContract([FromBody] DealUpdateRequestDTOForContract deal)
        {
            string Env = ConfigurationManager.ConnectionStrings["mach1"].ConnectionString;
            var isUpdated = dealHelper.UpdateDealForContract(deal);
               
            if (isUpdated)
                return Request.CreateResponse(HttpStatusCode.OK);

            return Request.CreateResponse(HttpStatusCode.BadRequest, "Deal update failed!");
        }
    }
}