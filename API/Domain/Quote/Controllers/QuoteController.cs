using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using API.Utilities.EmailSystem.BusinessLayer.Classes;
using Newtonsoft.Json.Linq;
using System;
using API.Utilities.Auth;
using API.Utilities;
using API.Data;
using API.Models;
using API.Managers;
using System.IO;
using API.Services.HubspotService.Deals.Helper;


namespace API.Controllers
{
    [JWTAuthorization]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class QuoteController : ApiController
    {
        /// <summary>
        /// Creates quote and sends quote email
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost] 
        [Route("api/quote")]
        public async System.Threading.Tasks.Task<HttpResponseMessage> SendQuickQuoteEmail([FromBody] JObject json)
        {
            try
            {
                string jwt = null;
                string fromEmailAddress = null;

                if (Request.Headers.Authorization != null)
                {
                    jwt = Request.Headers.Authorization.Parameter;
                    fromEmailAddress = ActiveDirectory.GetActiveDirectoryEmail(jwt);
                }

                fromEmailAddress = fromEmailAddress != null ? fromEmailAddress : "quote@wwmach.com";
                var bccEmailAddress = fromEmailAddress;

                List<string> photos = null;
                var emailSender = new MandrillEmailSender();
                var jsonToEmailAddresses = json["to"].Values().ToList();
                var toEmailAddresses = jsonToEmailAddresses.Select(toEmailAddress => toEmailAddress.Value<string>()).ToList();
                var subject = json["subject"] != null ? json["subject"].Value<string>() : "";
                var body = json["body"] != null ? json["body"].Value<string>() : "";

                if (json["photos"] != null)
                {
                    var jsonPhotos = json["photos"].Values().ToList();
                    photos = jsonPhotos.Select(toEmailAddress => toEmailAddress.Value<string>()).ToList();
                }

                await emailSender.SendEmail(toEmailAddresses, subject, body, fromEmailAddress, photos, bccEmailAddress);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e);
            }
        }
         
        ///<summary>
        ///Returns all active quotes
        ///</summary>
        [HttpGet]
        [Route("api/quotes/active")]
        public HttpResponseMessage GetQuotes()
        {
            var db = DAL.GetInstance();
            var quotes = db.GetActiveQuotes();
            return Request.CreateResponse(HttpStatusCode.OK, quotes);
        }

        [HttpGet]
        [Route("api/quotes/active/{quoteType}")]
        public HttpResponseMessage GetQuotesByType(string quoteType)
        {
            var sqlParams = new JObject { { "QuoteType", quoteType } };
            var db = DAL.GetInstance();
            var quotes = db.GetActiveQuotes(sqlParams);
            foreach (Quote q in quotes)
            {
                q.quoteDetails = DAL.GetInstance().getQuoteDetailsByQuoteID(new JObject { { "QuoteID", q.quoteID } });
                foreach (QuoteDetail qd in q.quoteDetails)
                {
                    qd.QuoteDetailMoreFields = DAL.GetInstance().getQuoteDetailExtensionByQuoteDetailID(new JObject { { "QuoteDetailID", qd.QuoteDetailID } });
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, quotes);
        }
        ///<summary>
        ///Returns quote detail by quoteID
        ///</summary>

        [HttpGet]
        [Route("api/quotes/{quoteID}")]
        public HttpResponseMessage GetQuotesDetailByQuoteID(int quoteID)
        {

            var sqlParams = new JObject { { "QuoteID", quoteID } };
            var quote = Builder.Build(new QuoteDetails(), sqlParams);
            return quote == null ? Request.CreateResponse(HttpStatusCode.NoContent) : Request.CreateResponse(HttpStatusCode.OK, quote);

        }
        

        ///<summary>
        ///Returns quotedetailmoreFields info by quotedetailID
        ///</summary>
        [HttpGet]
        [Route("api/quotes/quotedetailextension/{quoteDetailID}")]
        public HttpResponseMessage GetQuotesDetailMoreFieldsByQuoteDetailID(int quoteDetailID)
        {
            var sqlParams = new JObject { { "QuoteDetailID", quoteDetailID } };
            QuoteDetailMoreFields myClients = DAL.GetInstance().getQuoteDetailExtensionByQuoteDetailID(sqlParams);
            if (myClients == null) return Request.CreateResponse(HttpStatusCode.NoContent);
            return Request.CreateResponse(HttpStatusCode.OK, myClients);

        }
      

        ///<summary>
        ///Create a new rental quote including quotedetails
        ///</summary>
        [HttpPost]
        [Route("api/quotes/newrentalquote")]
        public HttpResponseMessage CreateRentalQuote(Quote newRentalQuote)
        {        
            newRentalQuote.QuoteType = "Rental";
            var data = QuoteManager.CreateQuote(newRentalQuote);
            return data != null ? Request.CreateResponse(HttpStatusCode.OK, data) : Request.CreateResponse(HttpStatusCode.InternalServerError);       
        }

        ///<summary>
        ///Create a new sales quote including quotedetails
        ///</summary>
        [HttpPost]
        [Route("api/quotes/newsalesquote")]
        public HttpResponseMessage CreateSalesQuote(Quote newSalesQuote)
        {           
            newSalesQuote.QuoteType = "Sales";
            var data = QuoteManager.CreateQuote(newSalesQuote);
            return data != null ? Request.CreateResponse(HttpStatusCode.OK, data) : Request.CreateResponse(HttpStatusCode.InternalServerError);     
        }

        ///<summary>
        ///Returns list of all machines that match search criteria
        ///</summary>
        [HttpPost] //BREAKING REST..TOO MANY KEY/VALUE PAIRS TO SHOVE IN HTTP GET
        [Route("api/quotes/search/sales/open")]
        public HttpResponseMessage QuoteSearchSalesOpen(QuotesSearch search)
        {
            var json = JObject.FromObject(search);       
            var Quote = DAL.GetInstance().QuoteSearch(json,"Open","Sales");
            return Quote == null ? Request.CreateResponse(HttpStatusCode.NoContent) : Request.CreateResponse(HttpStatusCode.OK, Quote);
        }
        [HttpPost] //BREAKING REST..TOO MANY KEY/VALUE PAIRS TO SHOVE IN HTTP GET
        [Route("api/quotes/search/sales/closed")]
        public HttpResponseMessage QuoteSearchSalesClosed(QuotesSearch search)
        {
            var json = JObject.FromObject(search);
            //var Quote = Builder.Build(new QuoteSimple(), json);
            var Quote = DAL.GetInstance().QuoteSearch(json, "Closed","Sales");
            //because database are not perfect, some closed quotes still show active, so make it empty
            foreach (QuoteSearchResults qsr in Quote)
            {
                if (qsr.Reason == "Active")
                { 
                    qsr.Reason="";
                }
            }
            return Quote == null ? Request.CreateResponse(HttpStatusCode.NoContent) : Request.CreateResponse(HttpStatusCode.OK, Quote);
        }

        [HttpPost] //BREAKING REST..TOO MANY KEY/VALUE PAIRS TO SHOVE IN HTTP GET
        [Route("api/quotes/search/rental/open")]
        public HttpResponseMessage QuoteSearchRentalOpen(QuotesSearch search)
        {
            var json = JObject.FromObject(search);

            var Quote = DAL.GetInstance().QuoteSearch(json, "Open","Rental");
            return Quote == null ? Request.CreateResponse(HttpStatusCode.NoContent) : Request.CreateResponse(HttpStatusCode.OK, Quote);
         }

        [HttpPost] //BREAKING REST..TOO MANY KEY/VALUE PAIRS TO SHOVE IN HTTP GET
        [Route("api/quotes/search/rental/closed")]
        public HttpResponseMessage QuoteSearchRentalClosed(QuotesSearch search)
        {
            var json = JObject.FromObject(search);
            //var Quote = Builder.Build(new QuoteSimple(), json);
            var Quote = DAL.GetInstance().QuoteSearch(json, "Closed", "Rental");
            //because database are not perfect, some closed quotes still show active, so make it empty
            foreach (QuoteSearchResults qsr in Quote)
            {
                if (qsr.Reason == "Active")
                {
                    qsr.Reason = "";
                }
            }
            return Quote == null ? Request.CreateResponse(HttpStatusCode.NoContent) : Request.CreateResponse(HttpStatusCode.OK, Quote);
        }

        [HttpPost] 
        [Route("api/quotes/sendemail")]
        public async System.Threading.Tasks.Task<HttpResponseMessage> QuoteSend([FromBody]JObject json)
        {
            int quoteID = int.Parse(json["quoteID"].ToString());
            string urlAddress = "https://wwmach.com/email/infosheet.aspx?RentalQuoteID=3159970&QuoteType=Rental&month=True&week=false&day=True&freight=True&cell=True&OverTime=False&InsuranceValue=False";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, System.Text.Encoding.GetEncoding(response.CharacterSet));
                }

                string data = readStream.ReadToEnd();
                
                string senderName = json["senderName"].ToString();
                string senderEmail = json["senderEmail"].ToString();
                List<string> to = new List<string>();
              
                JToken[] recipients =json["recipient"].ToArray();
                foreach(JToken recipient in recipients)
                {
                    to.Add(recipient.ToString());
                }
                string subject = json["subject"].ToString();
                string body = json["body"].ToString();

                await new Email(senderName, senderEmail, to.ToArray(), subject, body+'\n'+data).Send();
                
                response.Close();
                readStream.Close();
            }
          return  Request.CreateResponse(HttpStatusCode.OK, json);
        }

        [HttpPost] 
        [Route("api/quotes/sendemailbydb")]
        public HttpResponseMessage QuoteSendByDB([FromBody]JObject json)
        {
            var db = DAL.GetInstance();
            var quotesSendResult = db.SendQuoteEmailByDB(json);
            return quotesSendResult? Request.CreateResponse(HttpStatusCode.OK, quotesSendResult):Request.CreateResponse(HttpStatusCode.InternalServerError, quotesSendResult);
         
        }

        [HttpPost]
        [Route("api/quotes/email/newpreference")]
        public HttpResponseMessage QuoteEmailNewPreference([FromBody]JObject json)
        {
            var db = DAL.GetInstance();
            var emailPreferenceID = db.CreateNewEmailPreference(json);
            return emailPreferenceID != 0 ? Request.CreateResponse(HttpStatusCode.OK, emailPreferenceID) : Request.CreateResponse(HttpStatusCode.InternalServerError);

        }

        [HttpPost]
        [Route("api/quotes/email/getpreference")]
        public HttpResponseMessage QuoteEmailGetPreferenceByContactIDAndPreferenceType([FromBody]JObject json)
        {
            var db = DAL.GetInstance();
            var emailPreferenceInfo = db.GetEmailPreferenceInfo(json);
            return emailPreferenceInfo!=null ? Request.CreateResponse(HttpStatusCode.OK, emailPreferenceInfo) : Request.CreateResponse(HttpStatusCode.InternalServerError);

        }

        [HttpPost]
        [Route("api/quotes/email/deactivepreference")]
        public HttpResponseMessage QuoteEmailDeactivePreference([FromBody]JObject json)
        {
            var db = DAL.GetInstance();
            var emailPreferenceDeavativeResult = db.DeactiveEmailPreference(json);
            return emailPreferenceDeavativeResult ? Request.CreateResponse(HttpStatusCode.OK, emailPreferenceDeavativeResult) : Request.CreateResponse(HttpStatusCode.InternalServerError);

        }
        [HttpPost]
        [Route("api/quotes/email/updatepreference")]
        public HttpResponseMessage QuoteEmailUpdatePreference([FromBody]JObject json)
        {
            var db = DAL.GetInstance();
            var emailPreferenceEditResult = db.EditEmailPreference(json);
            return emailPreferenceEditResult ? Request.CreateResponse(HttpStatusCode.OK, emailPreferenceEditResult) : Request.CreateResponse(HttpStatusCode.InternalServerError);

        }
        [HttpPost]
        [Route("api/quotes/email/defaultpreference")]
        public HttpResponseMessage QuoteEmailMakeDefaultPreference([FromBody]JObject json)
        {
            var db = DAL.GetInstance();
            var emailPreferenceDefaultResult = db.MakeDefaultEmailPreference(json);
            return emailPreferenceDefaultResult ? Request.CreateResponse(HttpStatusCode.OK, emailPreferenceDefaultResult) : Request.CreateResponse(HttpStatusCode.InternalServerError);

        }

        [HttpGet]
        [Route("api/quotes/term/{term}")]
        public HttpResponseMessage GetTermAndCondition(string term)
        {
            var db = DAL.GetInstance();
            var tokens = new JObject();
            tokens.Add("Name", term);
            var quotes = db.GetTermAndCondition(tokens);
            return Request.CreateResponse(HttpStatusCode.OK, quotes);
        }

        [OverrideActionFilters]
        [CustomerAuthorization]
        [HttpGet]
        [Route("api/quotes/customer/allquote")]
        public HttpResponseMessage GetAllQuoteForCustomer()
        {
            var CustomerPortalAuthObj = new CustomerPortalAuth();
            int customerID = CustomerPortalAuthObj.GetCustomerIDFromToken(this.ActionContext);
            if(customerID>0)
            {
                var db = DAL.GetInstance();
                var tokens = new JObject();
                tokens.Add("CustomerID", customerID);
                var quotes = db.GetListQuoteForCustomer(tokens);
                return Request.CreateResponse(HttpStatusCode.OK, quotes);
            }
               
            else
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        [OverrideActionFilters]
        [CustomerAuthorization]
        [HttpGet]
        [Route("api/customers/quotes/{quoteID}")]
        public HttpResponseMessage GetCustomerQuotesDetailByQuoteID(int quoteID)
        {
            var db = DAL.GetInstance();
            var CustomerPortalAuthObj = new CustomerPortalAuth();
            int customerID = CustomerPortalAuthObj.GetCustomerIDFromToken(this.ActionContext);
            //check customerid match contractid
            JObject check = new JObject { { "QuoteID", quoteID }, { "CustomerID", customerID } };
            if (!db.CheckQuoteIDMatchCustomerID(check))
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            var sqlParams = new JObject { { "QuoteID", quoteID } };
            var quote = Builder.Build(new QuoteDetails(), sqlParams);
            return quote == null ? Request.CreateResponse(HttpStatusCode.NoContent) : Request.CreateResponse(HttpStatusCode.OK, quote);

        }

        [OverrideActionFilters]
        [CustomerAuthorization]
        [HttpPost]
        [Route("api/customers/quotes/accept")]
        public HttpResponseMessage CustomerAcceptQuote([FromBody]JObject json)
        {
            var db = DAL.GetInstance();
            var CustomerPortalAuthObj = new CustomerPortalAuth();
            int customerID = CustomerPortalAuthObj.GetCustomerIDFromToken(this.ActionContext);
            //check customerid match contractid
            JObject check = new JObject { { "QuoteID", json["QuoteID"] }, { "CustomerID", customerID } };
            if (!db.CheckQuoteIDMatchCustomerID(check))
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            var sqlParams = new JObject { { "QuoteID", json["QuoteID"] } };
            var quote = db.customerAcceptQuote(json);
            Quote quoteData = db.getQuoteByQuoteID(sqlParams);
            if(quoteData.DealID != 0)
            {
                DealsHelper dealHelp = new DealsHelper();
                dealHelp.UpdateDealStageOnly(quoteData.DealID);
            }
            return quote? Request.CreateResponse(HttpStatusCode.OK) : Request.CreateResponse(HttpStatusCode.InternalServerError);

        }
        
    }
}