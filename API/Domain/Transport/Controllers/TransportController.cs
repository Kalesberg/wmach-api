using API.Data;
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

namespace API.Controllers
{
    [JWTAuthorization]
    [EnableCors(origins: "*", headers: "*", methods: "*")]   
    public class TransportController : ApiController
    {
        [Route("api/transport/{EquipmentID}")]
        public HttpResponseMessage Get(int EquipmentID)
        {
            var sqlParams = JSON.Parse(EquipmentID, "EquipmentID");
            var transport = Builder.Build(new TransportBuilder(), sqlParams);
            return Request.CreateResponse(HttpStatusCode.OK, transport);
        }

        [OverrideActionFilters]
        [CustomerAuthorization]
        [HttpPost]
        [Route("api/customers/newtransportationquote")]
        public HttpResponseMessage CreateCustomerTransporationQuoteRequest([FromBody]JObject json)
        {
            
            int quoteID = int.Parse(json["quoteID"].ToString());

            var db = DAL.GetInstance();
            var CustomerPortalAuthObj = new CustomerPortalAuth();
            int customerID = CustomerPortalAuthObj.GetCustomerIDFromToken(this.ActionContext);
            //check customerid match contractid
            JObject check = new JObject { { "QuoteID", quoteID }, { "CustomerID", customerID } };
            if (!db.CheckQuoteIDMatchCustomerID(check))
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            JObject transportation = new JObject();
            if (json["address"].Type != JTokenType.Null)
            {
                transportation = new JObject {
                    {"Address", true},
                    {"Street", json["address"]["street"].Value<string>() },
                    {"City", json["address"]["city"].Value<string>() },
                    {"State", json["address"]["state"].Value<string>() },
                    {"Country", json["address"]["country"].Value<string>() },
                    {"Zip", json["address"]["zip"].Value<string>() }
                };
            }
            else
            {
                transportation = new JObject {
                    {"Address", false}
                };
            }
            transportation.Add("QuoteId", quoteID);
            transportation.Add("RoundTrip", json["roundTrip"]);
            transportation.Add("RawAddress", json["rawAddress"]);
            transportation.Add("ReqDeliveryDate", json["deliveryDate"]);
            var shippingQuote = db.createCustomerTransporationQuoteRequest(transportation);
            return shippingQuote == 0 ? Request.CreateResponse(HttpStatusCode.NoContent) : Request.CreateResponse(HttpStatusCode.OK, shippingQuote);

        }
        
    }


}
