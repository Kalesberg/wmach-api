using API.Services.HubspotService.Deals.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using API.Services.HubspotService.Company.Models;
using System.Web;
using System.Net;
using API.Data;
using API.Models;

namespace API.Services.HubspotService.Deals.Helper
{
    public class DealsHelper
    {
        HttpClient httpClient;
        private readonly string baseUrl = ConfigurationManager.AppSettings["HubspotBaseURL"];
        private readonly string apiKey = ConfigurationManager.AppSettings["HubspotHapiKey"];


        public DealsHelper()
        {
            httpClient = new HttpClient
            {
                MaxResponseContentBufferSize = 956000
            };
        }

        public List<DealDTO> GetDeals(string sourceId, List<string> allowedStages)
        {
            
            var allFilteredDeals = new List<DealDTO>();
            string offset = null;
            ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;
          
            do
            {
                string offsetParam = string.IsNullOrEmpty(offset) ? null : "offset=" + offset;
                var endpoint = string.Format("{0}deals/v1/deal/paged/?hapikey={1}&includeAssociations=true&limit=250&properties=dealname&properties=dealstage&properties=hubspot_owner_id&{2}", baseUrl, apiKey, offsetParam);
                Task<HttpResponseMessage> responseMessage = Task.Run(async () => await httpClient.GetAsync(endpoint));

                Task<string> result = Task.Run(async () => await responseMessage.Result.Content.ReadAsStringAsync());
                if (!responseMessage.Result.IsSuccessStatusCode)
                    return new List<DealDTO>();


                var data = JsonConvert.DeserializeObject<DealResponse>(result.Result);
                var filteredDeals = data.deals.Where(d => d.properties.hubspot_owner_id != null 
                    && d.properties.hubspot_owner_id.value != null
                    && d.properties.hubspot_owner_id.value.Equals(sourceId, StringComparison.InvariantCultureIgnoreCase)
                    && d.properties.dealstage != null
                    && allowedStages.Contains(d.properties.dealstage.value)).ToList();
                allFilteredDeals.AddRange(filteredDeals);

                if (data.hasMore)
                    offset = data.offset;
                else
                    offset = null;
            }
            while (offset != null);
            ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => false;

            return allFilteredDeals;
        }

        public long GetDealOwnerId(string email)
        {
            var endpoint = string.Format("{0}crm/v3/owners/?hapikey={1}&email={2}", baseUrl, apiKey, email);
            ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;
            Task<HttpResponseMessage> responseMessage = Task.Run(async () => await httpClient.GetAsync(endpoint));
            Task<string> result = Task.Run(async () => await responseMessage.Result.Content.ReadAsStringAsync());
            
            if (!responseMessage.Result.IsSuccessStatusCode)
                return 0;

            JObject jObjResponse = JObject.Parse(result.Result); 
            JArray array = (JArray)jObjResponse["results"];
            var owner = array[0];
            long id = long.Parse(owner.Value<string>("id"));
            ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => false;
            return id;
        }

        public string GetADealbyDealID(long dealID)
        {
            var endpoint = string.Format("{0}deals/v1/deal/{1}?hapikey={2}", baseUrl, dealID, apiKey);
            ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;
            Task<HttpResponseMessage> responseMessage = Task.Run(async () => await httpClient.GetAsync(endpoint));
            Task<string> result = Task.Run(async () => await responseMessage.Result.Content.ReadAsStringAsync());

            if (!responseMessage.Result.IsSuccessStatusCode)
                return "";

            var data = JsonConvert.DeserializeObject<DealDTO>(result.Result);
            string dealName = data.properties.dealname.value;
            ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => false;
            return dealName;
        }

        public bool UpdateDeal(DealUpdateRequestDTO deal)
        {
            try
            {
                var endpoint = string.Format("{0}deals/v1/deal/{1}/?hapikey={2}", baseUrl, deal.DealId, apiKey);
                string Uri = ConfigurationManager.AppSettings["CustomerPortalUrl"];
                deal.QuoteUrl = Uri + "quote/" + deal.QuoteID;
                var dataToUpdate = new DealUpdateJson
                {
                    properties = new List<Props>
                   {
                       new Props
                       {
                           name = "amount",
                           value = deal.Amount.ToString()
                       },
                       new Props
                       {
                           name = "quote_type",
                           value = deal.QuoteType
                       },
                       new Props
                       {
                           name = "quote_number",
                           value = deal.QuoteNumber
                       },
                       new Props
                       {
                           name = "deal_equipment_description",
                           value = deal.DealEquipmentDescription
                       },
                       new Props
                       {
                           name = "dealstage",
                           value = deal.DealStage
                       },
                       new Props
                       {
                           name = "closedate",
                           value = deal.Closedate.ToString()
                       },
                       new Props
                       {
                           name = "quote_url",
                           value = deal.QuoteUrl
                       },
                       new Props
                       {
                           name = "max_expected_amount",
                           value = deal.MaxExpectedAmount.ToString()
                       },
                      
    }
                };

                if (deal.QuoteType == "Sales")
                    dataToUpdate.properties.RemoveAt(5);
                             var stringPayload = JsonConvert.SerializeObject(dataToUpdate);
                HttpContent content = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;
                Task<HttpResponseMessage> responseMessage = Task.Run(async () => await httpClient.PutAsync(endpoint, content));

                Task<string> result = Task.Run(async () => await responseMessage.Result.Content.ReadAsStringAsync());
                
                if (!responseMessage.Result.IsSuccessStatusCode)
                {
                    ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => false;
                    return false;
                }
              
                else
                {
                    ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => false;
                    return true;
                }
               

            }
            catch (Exception)
            {

                throw;
            }
        }

        public long CreateADeal(DealCreateRequestDTO deal)
        {
            try
            {
                // hubspot_owner_email
                deal.hubspot_owner_id = GetDealOwnerId(deal.hubspot_owner_email).ToString();
                ContactHubspot contact = new ContactHubspot();
                contact = GetContactIDandCompanyIDbycontactEmail(deal.contactHubspotEmail);
                var endpoint = string.Format("{0}deals/v1/deal?hapikey={1}", baseUrl, apiKey);
                string Uri = ConfigurationManager.AppSettings["CustomerPortalUrl"];
                deal.QuoteUrl = Uri + "quote/" + deal.QuoteID;
                var dataToCreate = new DealUpdateJson
                {
                    associations = new AssociationsContact
                    {
                        associatedCompanyIds = new List<long> { },
                        associatedVids = new List<long> { },
                    },
                    properties = new List<Props>
                   {
                       new Props
                       {
                           name = "amount",
                           value = deal.Amount.ToString()
                       },
                       new Props
                       {
                           name = "quote_type",
                           value = deal.QuoteType
                       },
                       new Props
                       {
                           name = "quote_number",
                           value = deal.QuoteNumber
                       },
                       new Props
                       {
                           name = "deal_equipment_description",
                           value = deal.DealEquipmentDescription
                       },
                       new Props
                       {
                           name = "dealstage",
                           value = deal.DealStage
                       },
                       new Props
                       {
                           name = "closedate",
                           value = deal.Closedate.ToString()
                       },
                       new Props
                       {
                           name = "quote_url",
                           value = deal.QuoteUrl
                       },
                       new Props
                       {
                           name = "max_expected_amount",
                           value = deal.MaxExpectedAmount.ToString()
                       },
                        new Props
                       {
                           name = "dealname",
                           value = deal.dealname
                       },
                         new Props
                       {
                           name = "Pipeline",
                           value = deal.Pipeline
                       },
                          new Props
                       {
                           name = "hubspot_owner_id",
                           value = deal.hubspot_owner_id.ToString()
            }}
            
                };

                if (deal.QuoteType == "Sales")
                    dataToCreate.properties.RemoveAt(5);

                if (contact.vid > 0)
                { 
                dataToCreate.associations.associatedVids = new List<long> { contact.vid };
                dataToCreate.associations.associatedCompanyIds = new List<long> { Convert.ToInt64(contact.properties.associatedcompanyid.value) };
                }

                var stringPayload = JsonConvert.SerializeObject(dataToCreate);
                HttpContent content = new StringContent(stringPayload, Encoding.UTF8, "application/json");


                ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;
                Task<HttpResponseMessage> responseMessage = Task.Run(async () => await httpClient.PostAsync(endpoint, content));
                Task<string> result = Task.Run(async () => await responseMessage.Result.Content.ReadAsStringAsync());

                if (!responseMessage.Result.IsSuccessStatusCode)
                {
                    ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => false;
                    return 0;
                }

                else
                {
                    ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => false;
                    var data = JsonConvert.DeserializeObject<DealDTO>(result.Result);
                    return data.dealId;
                }


            }
            catch (Exception)
            {

                throw;
            }
        }


        public ContactHubspot GetContactIDandCompanyIDbycontactEmail(string contactEmail)
        {
            var endpoint = string.Format("{0}contacts/v1/contact/email/{1}/profile?hapikey={2}", baseUrl, contactEmail, apiKey);

            ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;
            Task<HttpResponseMessage> responseMessage = Task.Run(async () => await httpClient.GetAsync(endpoint));
            Task<string> result = Task.Run(async () => await responseMessage.Result.Content.ReadAsStringAsync());

            if (!responseMessage.Result.IsSuccessStatusCode)
                return new ContactHubspot();

            var data = JsonConvert.DeserializeObject<ContactHubspot>(result.Result);
            
            ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => false;
            return data;
        }


        public bool UpdateDealForContract(DealUpdateRequestDTOForContract deal)
        {
            try
            {
                var endpoint = string.Format("{0}deals/v1/deal/{1}/?hapikey={2}", baseUrl, deal.DealId, apiKey);
                string Uri = ConfigurationManager.AppSettings["CustomerPortalUrl"];
                deal.ContractUrl = Uri + "contract/" + deal.ContractID;
                var dataToUpdate = new DealUpdateJson
                {
                    properties = new List<Props>
                   {
                       new Props
                       {
                           name = "amount",
                           value = deal.Amount.ToString()
                       },
                       new Props
                       {
                           name = "agreement_type",
                           value = deal.ContractType
                       },
                       new Props
                       {
                           name = "contract_number",
                           value = deal.ContractNumber
                       },
                       new Props
                       {
                           name = "deal_equipment_description",
                           value = deal.DealEquipmentDescription
                       },
                       new Props
                       {
                           name = "dealstage",
                           value = deal.DealStage
                       },
                       new Props
                       {
                           name = "closedate",
                           value = deal.Closedate.ToString()
                       },
                       new Props
                       {
                           name = "contract_url",
                           value = deal.ContractUrl
                       },
                       new Props
                       {
                           name = "max_expected_amount",
                           value = deal.MaxExpectedAmount.ToString()
                       },

    }
                };

                var stringPayload = JsonConvert.SerializeObject(dataToUpdate);
                HttpContent content = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;
                Task<HttpResponseMessage> responseMessage = Task.Run(async () => await httpClient.PutAsync(endpoint, content));

                Task<string> result = Task.Run(async () => await responseMessage.Result.Content.ReadAsStringAsync());

                if (!responseMessage.Result.IsSuccessStatusCode)
                {
                    ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => false;
                    return false;
                }

                else
                {
                    ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => false;
                    return true;
                }


            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool UpdateDealStageOnly(long dealID)
        {
            try
            {
                var endpoint = string.Format("{0}deals/v1/deal/{1}/?hapikey={2}", baseUrl, dealID, apiKey);
                var db = DAL.GetInstance();
                string Env = ConfigurationManager.ConnectionStrings["mach1"].ConnectionString.Contains("L3SQ") ? "Prod" : "Test";
                JObject para = new JObject { { "Enviroment", Env } };
                List<DealStages> dealstages = db.GetAllDealStageValue(para);
                string accepted = dealstages.Find(s => s.Name.Trim() == @"Quote Accepted--""Order""").value;
                var dataToUpdate = new DealUpdateJson
                {
                    properties = new List<Props>
                   {
                       new Props
                       {
                           name = "dealstage",
                           value = accepted,
                       }
                    }
                };

                var stringPayload = JsonConvert.SerializeObject(dataToUpdate);
                HttpContent content = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;
                Task<HttpResponseMessage> responseMessage = Task.Run(async () => await httpClient.PutAsync(endpoint, content));

                Task<string> result = Task.Run(async () => await responseMessage.Result.Content.ReadAsStringAsync());

                if (!responseMessage.Result.IsSuccessStatusCode)
                {
                    ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => false;
                    return false;
                }

                else
                {
                    ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => false;
                    return true;
                }


            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}