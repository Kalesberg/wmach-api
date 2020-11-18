using API.Data;
using API.Models;
using API.Services.HubspotService.Company.Controller;
using API.Services.HubspotService.Deals.Models;
using API.Utilities.Auth;
using API.Utilities.Company;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace API.Controllers
{
    [HubspotAuthorization]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class WebhookController : ApiController
    {

        private readonly CompanyController companyController = new CompanyController();
        /// <summary>
        /// Receives a webhook object when an event is triggered in Hubspot
        /// </summary>
        /// <param name="webhookDTO"></param>
        /// <returns>Status code</returns>
        ///<response code="500">Server Error</response>
        ///<response code="200">Success</response>
        ///<response code="204">No Content</response>
        ///<response code="401">Unauthorized</response>
        [HttpPost]
        [Route("api/hubspot/webhook")]
        public HttpResponseMessage GetWebhookObject([FromBody] List<WebhookDTO> webhookDTO)
        {
            try
            {
                int res = 1;
                if (webhookDTO == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

                foreach (var item in webhookDTO)
                {
                    DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                    var date = dateTime.AddMilliseconds(item.occurredAt).ToLocalTime();

                    JObject jObject = new JObject
                    {
                        {"objectId", item.objectId },
                        {"propertyName", item.propertyName },
                        {"changeSource", item.changeSource },
                        {"eventId", item.eventId },
                        {"subscriptionId", item.subscriptionId },
                        {"portalId", item.portalId },
                        {"appId", item.appId },
                        {"occurredAt", item.occurredAt },
                        {"subscriptionType", item.subscriptionType },
                        {"attemptNumber", item.attemptNumber }
                    };

                    var db = DAL.GetInstance();
                    res = db.CreateWehook(jObject);

                    //TODO: create the company/contact objects
                    if (res == 0)
                    {
                        switch (item.subscriptionType)
                        {
                            case "company.creation":
                                {
                                    CreateCompany(item);
                                    break;
                                }

                            case "deal.creation":
                                //WebhookManager.CreateQuoteFromDeal(item.objectId);
                                companyController.GetDeal(item.objectId);
                                break;
                            case "contact.creation":
                                break;
                            case "contact.propertyChange":
                                AssociatedCompany(item);
                                break;
                        }
                    }
                    else
                        return Request.CreateResponse(HttpStatusCode.Forbidden, webhookDTO);

                }


                return res == 0 ? Request.CreateResponse(HttpStatusCode.OK, res) : Request.CreateResponse(HttpStatusCode.Forbidden, webhookDTO);

            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private void AssociatedCompany(WebhookDTO item)
        {
            var contactObj = companyController.GetContactByID(item.objectId);
            if (contactObj.associatedcompany.properties != null)
            {
                CreateCompanyFromAssociatedContact(contactObj.associatedcompany.companyid);
            }
        }

        private void CreateCompanyFromAssociatedContact(long companyid)
        {
            var getCompany = companyController.GetCompany(companyid);
            var getContact = companyController.GetCompanyContact(companyid);
            if (getCompany != null && getContact.contacts.Count > 0)
            {
                //company and contact records exist, create M1 entities
                var json = JsonConvert.SerializeObject(getCompany);
                var obj = JsonConvert.SerializeObject(getContact);
                var companyObj = Company.CreateCompany(json, obj);
                if (companyObj != null)
                {
                    //company and contact got created, do something?
                }
                else
                {
                    //company and contact wasnt created, send email to notify user
                }
            }
            else
            {
                //there is no contact record associated with the company, notify user
            }
        }

        private void CreateCompany(WebhookDTO item)
        {
            var getCompany = companyController.GetCompany(item.objectId);
            var getContact = companyController.GetCompanyContact(item.objectId);
            if (getCompany != null && getContact.contacts.Count > 0)
            {
                //company and contact records exist, create M1 entities
                var json = JsonConvert.SerializeObject(getCompany);
                var obj = JsonConvert.SerializeObject(getContact);
                var companyObj = Company.CreateCompany(json, obj);
                if (companyObj != null)
                {
                    //company and contact got created, do something?
                }
                else
                {
                    //company and contact wasnt created, send email to notify user
                }
            }
            else
            {
                //there is no contact record associated with the company, notify user
            }
        }

    }
}