using API.Data;
using API.Services.HubspotService.Company.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using API.Services.HubspotService.Deals.Helper;
using API.Services.HubspotService.Deals.Models;
using System.Net;
using System.Text;

namespace API.Services.HubspotService.Company.Controller
{
    public class CompanyController
    {
        HttpClient httpClient;
        private readonly string baseUrl = ConfigurationManager.AppSettings["HubspotBaseURL"];
        private readonly string apiKey = ConfigurationManager.AppSettings["HubspotHapiKey"];

        public CompanyController()
        {
            httpClient = new HttpClient
            {
                MaxResponseContentBufferSize = 256000
            };
        }

        public CompanyDTO GetCompany(long companyID)
        {
            try
            {
                var endpoint = string.Format("{0}companies/v2/companies/{1}?hapikey={2}", baseUrl, companyID.ToString(), apiKey);
                Task<HttpResponseMessage> responseMessage = Task.Run(async () => await httpClient.GetAsync(endpoint));

                Task<string> result = Task.Run(async () => await responseMessage.Result.Content.ReadAsStringAsync());

                switch (responseMessage.Result.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        CompanyDTO jobj = JsonConvert.DeserializeObject<CompanyDTO>(result.Result);
                        var res = GetCompanyContact(jobj.companyId);
                        if (res != null)
                        {
                            if (res.contacts.Count > 0)
                            {
                                return jobj;
                            }
                        }
                        else
                            return null;

                        return jobj;
                    case System.Net.HttpStatusCode.NotFound:
                        return null;
                    case System.Net.HttpStatusCode.InternalServerError:
                        return null;
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public ContactDTO GetCompanyContact(long companyId)
        {
            try
            {
                var endpoint = string.Format("{0}companies/v2/companies/{1}/contacts?hapikey={2}", baseUrl, companyId.ToString(), apiKey);
                Task<HttpResponseMessage> responseMessage = Task.Run(async () => await httpClient.GetAsync(endpoint));

                Task<string> result = Task.Run(async () => await responseMessage.Result.Content.ReadAsStringAsync());

                switch (responseMessage.Result.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        ContactDTO contactDTO = JsonConvert.DeserializeObject<ContactDTO>(result.Result);
                        if (contactDTO.contacts.Count > 0)
                        {
                            return contactDTO;
                        }
                        return contactDTO;
                    case System.Net.HttpStatusCode.NotFound:
                        return null;
                    default:
                        return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Deal GetDeal(long dealID)
        {
            try
            {
                List<LineItem> items = new List<LineItem>();
                var endpoint = string.Format("{0}deals/v1/deal/{1}?hapikey={2}", baseUrl, dealID.ToString(), apiKey);
                Task<HttpResponseMessage> responseMessage = Task.Run(async () => await httpClient.GetAsync(endpoint));
                responseMessage.Wait();

                Task<string> result = Task.Run(async () => await responseMessage.Result.Content.ReadAsStringAsync());
                result.Wait();

                switch (responseMessage.Result.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        var db = DAL.GetInstance();
                        Deal jobj = JsonConvert.DeserializeObject<Deal>(result.Result);
                        items = GetLineItemID(dealID);
                        var contact = GetContactByID(jobj.associations.associatedVids[0]);
                        var company = GetCompany(jobj.associations.associatedCompanyIds[0]);
                        var owner = GetOwnerByOwnerID(jobj.properties.hubspot_owner_id.value);

                        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                        var quoteDate = DateTime.Now;
                        var estStartDate = dateTime.AddMilliseconds(Convert.ToDouble(jobj.properties.est_start_date.value)).ToLocalTime();
                        var expireDate = quoteDate.AddDays(30);
                        var tokens = new JObject
                        {
                            { "Name", jobj.properties.term_and_conditions.value }
                        };
                        var term = db.GetTermAndCondition(tokens);
                        var period = "month";
                        switch (jobj.properties.min_rental_term_period.value)
                        {
                            case "4-Week(s)":

                                period = "four_weeks";
                                break;
                            case "Week(s)":
                                period = "week";
                                break;
                            case "Month(s)":
                                period = "month";
                                break;
                            case "Day(s)":
                                period = "day";
                                break;

                        }
                        var para = new JObject {
                          {"FirstName", contact.properties.firstname.value},
                            {"LastName", contact.properties.lastname.value},
                            {"CompanyName", company.properties.name.value}};

                        var contactrelationshipid = db.GetContactRelationshipForHubspot(para);

                        IEnumerable<API.Models.Contact> accs = db.getAccountManagerContacts();

                        var salespersonid = accs.Where(a => a.FirstName + ' ' + a.LastName == owner.firstName + ' ' + owner.lastName);
                        var salesmanagerid = accs.Where(a => a.FirstName + ' ' + a.LastName == jobj.properties.sales_manager.value);
                        var coordinatorid = accs.Where(a => a.FirstName + ' ' + a.LastName == jobj.properties.coordinator.value);
                        int? salesperson = salespersonid.Count() > 0 ? salespersonid.FirstOrDefault().ContactId : 0;
                        int? salesmanager = salesmanagerid.Count() > 0 ? salesmanagerid.FirstOrDefault().ContactId : 0;
                        int? coordinator = coordinatorid.Count() > 0 ? coordinatorid.FirstOrDefault().ContactId : 0;



                        JObject sqlParams = new JObject{
                 {"QuoteStatus", "Active"},
                 {"RentalOrSales", "Rental"},
                 //{"CompanyName", newQuote.account},
                 //{"CustomerName", newQuote},  those are parameter we realy don't need to create a quote and quotedetails
                 //{"CompanyAddress", newQuote},
                 //{"CustomerEmail", newQuote},
                 //{"BuissnessPhone", newQuote},
                 //{"MobielPhone", newQuote},
                 {"DivisionShortName", jobj.properties.division.value},
                 //{"DivisionLocation", newQuote},
                 {"Saleperson","WWM\\"+ owner.firstName[0] + owner.lastName},
                 {"TermsAndConditions", term},
                 {"QuoteEstimatedStartTime", estStartDate},
                 {"QuoteExpireTime", expireDate},
                 // {"QuoteEstimatedStartTime", "2009-05-11 00:00:00.000"},
                 //{"QuoteExpireTime", "2009-05-11 00:00:00.000"},
                 //{"CustomerNotes", newQuote},
                 {"ShowWeight", 1},
                 {"ShowQuantity", 1},
                 {"ShowPicture", 0},
                 {"ShowSerialNumber", 1},
                 {"ShowTotal", 0},
                 {"IncludeCurrentLocation", 0},
                 {"IncludeComponents", 0},
                 {"IncludeMarketingBlurb", 1},
                 {"IncludeMachineSpecifications", 0},
                 {"ShowPhotoLink", 0},
                 {"ShowFreight", 0},
                 {"ShowCell", 1},
                 {"ShowFootRate", 0},
                 {"ContactRelationshipID", contactrelationshipid},
                 {"FOB", ""},
                 //{"ShowInsuranceValue", newQuote.},
                 {"ShowMonthlyRate", 1},
                 {"ShowWeeklyRate", 1},
                 {"ShowDailyRate", 0},
                 {"ShowOvertimeRate", 0},
                 {"Jobsite",jobj.properties.job_site.value},
                 {"Format", ""},
                 {"MinimumTerm", jobj.properties.min_rental_term.value},
                 {"MinimumTermUOM",period},
                 {"CoordinatorID",coordinator},
                  {"SalesManager",salesmanager},
                 {"AccountManagerID", salesperson},
                  {"QuoteDetailType",""},
                  {"CurrencyRegion",""},
                  {"CurrencyType",""},
                   {"RPORate",""},
                  {"RPOOptionTerms",""},
                  {"DealID", dealID}
                 };

                        var QuoteID = db.CreateQuote(sqlParams);

                        foreach (LineItem qd in items)
                        {
                            double weekrate = Math.Ceiling(Convert.ToDouble(qd.properties.price.value) / 3);
                            JObject sqlParamsForQuoteModel = new JObject{
                               {"Model", qd.properties.name.value}};
                            var cateAndManu = db.GetModelCategoryByModelNum(sqlParamsForQuoteModel);
                            string cate = "", manu = "";
                            if (cateAndManu != null)
                            {
                                cate = cateAndManu.ProductCategoryName;
                                manu = cateAndManu.ManufacturerName;
                            }


                            JObject sqlParamsForQuoteDetail = new JObject
                         {
                               {"QuoteID", QuoteID},
                               {"SerialNumber", ""},
                               {"Quantity", qd.properties.quantity.value},
                               {"MonthlyRate", qd.properties.price.value },
                               {"WeeklyRate", weekrate},
                               {"FootRate", null},
                               {"MinFeet", null},
                               {"Model", qd.properties.name.value},
                               {"OvertimeHourlyRate", null},
                               {"Description", manu + " " + cate + " " + qd.properties.name.value},
                               {"ManufacturerName", manu},
                               {"Freight", null},
                               {"ProductCategory",cate},
                               {"ProductCatelog",""}
                         };
                            var quotedetailID = db.CreateQuoteDetail(sqlParamsForQuoteDetail);

                        }



                        return jobj;
                    case System.Net.HttpStatusCode.NotFound:
                        return null;
                    case System.Net.HttpStatusCode.InternalServerError:
                        return null;
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public List<LineItem> GetLineItem(List<long> lineItemIDs)
        {
            try
            {
                List<LineItem> items = new List<LineItem>();
                foreach (long id in lineItemIDs)
                {
                    var endpoint = string.Format("{0}crm-objects/v1/objects/line_items/{1}?hapikey={2}&properties=quantity&properties=price&properties=name", baseUrl, id.ToString(), apiKey);
                    Task<HttpResponseMessage> responseMessage = Task.Run(async () => await httpClient.GetAsync(endpoint));
                    responseMessage.Wait();

                    Task<string> result = Task.Run(async () => await responseMessage.Result.Content.ReadAsStringAsync());
                    result.Wait();
                    if (responseMessage.Result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        LineItem jobj = JsonConvert.DeserializeObject<LineItem>(result.Result);
                        items.Add(jobj);

                    }
                }
                return items;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public List<LineItem> GetLineItemID(long dealID)
        {
            try
            {
                List<LineItem> items = new List<LineItem>();

                var endpoint = string.Format("{0}crm-associations/v1/associations/{1}/HUBSPOT_DEFINED/19?hapikey={2}", baseUrl, dealID.ToString(), apiKey);
                Task<HttpResponseMessage> responseMessage = Task.Run(async () => await httpClient.GetAsync(endpoint));
                responseMessage.Wait();

                Task<string> result = Task.Run(async () => await responseMessage.Result.Content.ReadAsStringAsync());
                result.Wait();
                if (responseMessage.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    LineItemID jobj = JsonConvert.DeserializeObject<LineItemID>(result.Result);
                    if (jobj.results.Count > 0)
                        items = GetLineItem(jobj.results);
                }

                return items;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public ContactDetail GetContactByID(long contactID)
        {
            try
            {
                var endpoint = string.Format("{0}contacts/v1/contact/vid/{1}/profile?hapikey={2}", baseUrl, contactID.ToString(), apiKey);
                Task<HttpResponseMessage> responseMessage = Task.Run(async () => await httpClient.GetAsync(endpoint));
                responseMessage.Wait();

                Task<string> result = Task.Run(async () => await responseMessage.Result.Content.ReadAsStringAsync());
                result.Wait();
                if (responseMessage.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    ContactDetail jobj = JsonConvert.DeserializeObject<ContactDetail>(result.Result);
                    return jobj;
                }
                return null;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public Owner GetOwnerByOwnerID(string id)
        {
            try
            {
                var endpoint = string.Format("{0}owners/v2/owners/{1}?hapikey={2}", baseUrl, id, apiKey);
                Task<HttpResponseMessage> responseMessage = Task.Run(async () => await httpClient.GetAsync(endpoint));
                responseMessage.Wait();

                Task<string> result = Task.Run(async () => await responseMessage.Result.Content.ReadAsStringAsync());
                result.Wait();
                if (responseMessage.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Owner jobj = JsonConvert.DeserializeObject<Owner>(result.Result);
                    return jobj;
                }
                return null;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public List<CompanyHubspotDTO> GetAllHubspotCompany()
        {

            var allFilteredDeals = new List<CompanyHubspotDTO>();
            string offset = null;
            ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;

            do
            {
                string offsetParam = string.IsNullOrEmpty(offset) ? null : "offset=" + offset;
                var endpoint = string.Format("{0}companies/v2/companies/paged?hapikey={1}&limit=250&properties=name&properties=ckms_site_id__c&{2}", baseUrl, apiKey, offsetParam);
                Task<HttpResponseMessage> responseMessage = Task.Run(async () => await httpClient.GetAsync(endpoint));

                Task<string> result = Task.Run(async () => await responseMessage.Result.Content.ReadAsStringAsync());
                if (!responseMessage.Result.IsSuccessStatusCode)
                    return new List<CompanyHubspotDTO>();
               

                var data = JsonConvert.DeserializeObject<CompanyResponse>(result.Result);
                allFilteredDeals.AddRange(data.companies);
               
                if (data.hasMore)
                    offset = data.offset;
                else
                    offset = null;
            }
            while (offset != null);
            ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => false;

            return allFilteredDeals;
        }
        public string convertMillissecondsToDate(string startdatetime)
        {
            double ticks = double.Parse(startdatetime);
            TimeSpan time = TimeSpan.FromMilliseconds(ticks);
            DateTime startdate = new DateTime(1970, 1, 1) + time;
            return startdate.ToString();
        }
        public string convertDateToMilliseconed(string date)
        {
            var date1970 = new DateTime(1970, 1, 1, 0, 0, 0, (Convert.ToDateTime(date)).Kind);
            long mili = System.Convert.ToInt64(((Convert.ToDateTime(date)) - date1970).Ticks / TimeSpan.TicksPerMillisecond);
            return mili.ToString();
        }

        public bool UpdateHubspotCompanyInGroup(List<CompanyHubspotUpdate> companies)
        {

            var allFilteredDeals = new List<CompanyHubspotDTO>();

            ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;
            List<List<CompanyHubspotUpdate>> chunksOf100 = SplitListClass.SplitList(companies,100);
            foreach (List<CompanyHubspotUpdate> chunk in chunksOf100)
            {
                var endpoint = string.Format("{0}companies/v1/batch-async/update?hapikey={1}", baseUrl, apiKey);
                var stringPayload = JsonConvert.SerializeObject(chunk);
                HttpContent content = new StringContent(stringPayload, Encoding.UTF8, "application/json");

                Task<HttpResponseMessage> responseMessage = Task.Run(async () => await httpClient.PostAsync(endpoint, content));

                Task<string> result = Task.Run(async () => await responseMessage.Result.Content.ReadAsStringAsync());
                if (!responseMessage.Result.IsSuccessStatusCode)
                    return false;
            }
            ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => false;
            return true;
        }

        
    }

        public static class SplitListClass
        {
            public static List<List<T>> SplitList<T>(this List<T> me, int size = 50)
            {
                var list = new List<List<T>>();
                for (int i = 0; i < me.Count; i += size)
                    list.Add(me.GetRange(i, Math.Min(size, me.Count - i)));
                return list;
            }
        }
}