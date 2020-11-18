using API.Data;
using API.Models;
using API.Utilities;
using API.Utilities.Auth;
using API.Utilities.EmailSystem.BusinessLayer.Classes;
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
    /// <summary>
    /// Controller for Currency
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [JWTAuthorization]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CustomerController : ApiController
    {
        ///<summary>
        ///Here's customers and here's a summary to get that annoying warning to go away.
        ///</summary>
        [HttpPost]
        [Route("api/customers/getallcustomers")]
        public HttpResponseMessage GetAllCustomers([FromBody] JObject data)
        {

            var db = DAL.GetInstance();
            var customers = db.getCustomers(data);

            if (customers.Count > 0)
                return Request.CreateResponse(HttpStatusCode.OK, customers);
            else
                return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [HttpGet]
        [Route("api/customers/{contactId}/getcustomerinfo")]
        public HttpResponseMessage GetCustomerInfo(int contactId)
        {
            var tokens = new JObject { { "ContactID", contactId } };
            var db = DAL.GetInstance();
            var customer = Builder.Build(new CustomerDetail(), tokens);

            if (customer != null)
                return Request.CreateResponse(HttpStatusCode.OK, customer);
            else
                return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [HttpPost]
        [Route("api/customers/updatecustomerdetails")]
        public HttpResponseMessage UpdateCustomerDetails([FromBody] JObject json)
        {
            try
            {
                var db = DAL.GetInstance();
                var customerResult = db.updateCustomerInfo(json);

                if (customerResult)
                    return Request.CreateResponse(HttpStatusCode.OK, customerResult);
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpGet]
        [Route("api/customers/{contactId}/getcustomerdetails")]
        public HttpResponseMessage GetCustomerDetailsByContactRelationshipID(CustomerSearch search)
        {
            var db = DAL.GetInstance();
            var customers = db.getCustomers(new JObject(search));
            if (customers != null && customers.Count > 0)
                return Request.CreateResponse(HttpStatusCode.OK, customers);
            else
                return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Creates quote and sends quote email
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/customers/sendquickcontactemail")]
        public async System.Threading.Tasks.Task<HttpResponseMessage> SendQuickContactEmail([FromBody] JObject json)
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

                fromEmailAddress = fromEmailAddress != null ? fromEmailAddress : "info@wwmach.com";
                var bccEmailAddress = fromEmailAddress;

                List<string> photos = null;
                var emailSender = new MandrillEmailSender();
                var jsonToEmailAddresses = json["to"].Values().ToList();
                var toEmailAddresses = jsonToEmailAddresses.Select(toEmailAddress => toEmailAddress.Value<string>()).ToList();
                var subject = json["subject"] != null ? json["subject"].Value<string>() : "";
                var body = json["body"] != null ? json["body"].Value<string>() : "";

                await emailSender.SendEmail(toEmailAddresses, subject, body, fromEmailAddress, photos, bccEmailAddress);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e);
            }
        }

        /// <summary>
        /// Creates quote and sends quote email
        /// </summary>
        [HttpPost]
        [Route("api/customers/newcustomerportalaccount")]
        public async System.Threading.Tasks.Task<HttpResponseMessage> CreateCustomerportalAccount([FromBody] JObject json)
        {
            try
            {
                var db = DAL.GetInstance();
                var tokenContact = new JObject { { "ContactID", json["CustomerId"].ToString() } };
                var contact = db.getContactByContactID(tokenContact);
                var tokens = new JObject();
                tokens.Add("Email", contact.Email);
                if (db.CheckifCustomerPortalAccountExist(tokens) == 1)
                {

                    var customerResult = db.CreateNewCustomerPortalAccount(json);

                    if (customerResult)
                    {
                        //send passwordreset email
                        await db.SendPasswordResetEmail(json["TimeStampLink"].ToString(), contact.Email);
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }

                    else
                        return Request.CreateResponse(HttpStatusCode.InternalServerError);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.Conflict);
                }

            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        /// <summary>
        /// Rental Billing Invoice
        /// </summary>
        [OverrideActionFilters]
        [CustomerAuthorization]
        [HttpGet]
        [Route("api/customer/allinvoice")]
        public HttpResponseMessage GetAllInvoiceForCustomer()
        {
            var CustomerPortalAuthObj = new CustomerPortalAuth();
            int customerID = CustomerPortalAuthObj.GetCustomerIDFromToken(this.ActionContext);
            if (customerID > 0)
            {
                var db = DAL.GetInstance();
                var tokens = new JObject();
                tokens.Add("CustomerID", customerID);
                var bills = db.GetListInvoiceXMLForCustomer(tokens);
                string list = String.Join(String.Empty, bills);
                list = "<myroot>" + list + "</myroot>";
                List<Invoice> BillList = db.GetListOfInvoice(list);
                return Request.CreateResponse(HttpStatusCode.OK, BillList);
            }

            else
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        /// <summary>
        /// Rental Billing Invoice Detail
        /// </summary>
        [OverrideActionFilters]
        [CustomerAuthorization]
        [HttpPost]
        [Route("api/customer/invoice/detail")]
        public HttpResponseMessage GetInvoiceDetailForCustomer([FromBody] JObject json)
        {
            var CustomerPortalAuthObj = new CustomerPortalAuth();
            int customerID = CustomerPortalAuthObj.GetCustomerIDFromToken(this.ActionContext);
            if (customerID > 0)
            {
                var db = DAL.GetInstance();

                json.Add("CustomerID", customerID);
                var bills = db.GetInvoiceDetailForCustomer(json);
                return bills.Count == 0 ? Request.CreateResponse(HttpStatusCode.BadRequest) : Request.CreateResponse(HttpStatusCode.OK, bills);
            }

            else
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        /// <summary>
        /// Rental Billing Invoice
        /// </summary>
        [OverrideActionFilters]
        [CustomerAuthorization]
        [HttpGet]
        [Route("api/customer/salesmaninfo")]
        public HttpResponseMessage GetSalemanForCustomer()
        {
            var CustomerPortalAuthObj = new CustomerPortalAuth();
            int customerID = CustomerPortalAuthObj.GetCustomerIDFromToken(this.ActionContext);
            if (customerID > 0)
            {
                var db = DAL.GetInstance();
                var tokens = new JObject();
                tokens.Add("CustomerID", customerID);
                var salesperson = db.GetListofSalesmanforCustomer(tokens);
                return Request.CreateResponse(HttpStatusCode.OK, salesperson);
            }

            else
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }
        ///<summary>
        ///create customer quote request from wwm website
        ///</summary>
        [HttpPost]
        [Route("api/customer/quoterequest")]
        public HttpResponseMessage CreateQuoteRequest([FromBody] QuoteRequest data)
        {

            var db = DAL.GetInstance();
            JObject sqlParams = new JObject{
                         {"Name", data.Name},
                         {"Email",data.Email},
                         {"Phone", data.Phone},
                         {"Company", data.Company},
                         {"Message", data.Message}
                        };
            int quoteRequestID = db.createCustomerQuoteRequest(sqlParams);
            if (quoteRequestID != 0)
            {
                foreach (QuoteRequestEquipment qr in data.Machines)
                {
                    JObject sqlParamsequipment = new JObject{
                         {"CustomerQuoteRequestID", quoteRequestID},
                         {"ID",qr.ID},
                         {"Price", qr.Price}
                        };
                    int quoteRequestEquipmentID = db.createCustomerQuoteRequestEquipment(sqlParamsequipment);
                }
            }
            return quoteRequestID != 0 ? Request.CreateResponse(HttpStatusCode.OK) : Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        /// <summary>
        ///Get the Insurance and Credit Status of the customer
        ///</summary>
        ///<param name="contactID"></param>
        ///<returns>The status and Dates of the Customer</returns>
        ///<response code="500">Server Error</response>
        ///<response code="200">Success</response>
        ///<response code="401">Unauthorized</response>
        [HttpGet]
        [Route("api/customer/insurancestatus/{ContactID}")]
        public HttpResponseMessage GetInsuranceAndCreditStatus(int contactID)
        {
            var db = DAL.GetInstance();
            var data = db.getStatus(contactID);
            return data == null ? Request.CreateResponse(HttpStatusCode.NoContent, new JObject { { "Message", "No Records found" } }) : Request.CreateResponse(HttpStatusCode.OK, data);
        }


        //[HttpPost]
        //[Route("api/customer/activedate")]
        //public HttpResponseMessage AvtiveDate([FromBody] JObject json)
        //{
        //    try
        //    {
        //        var db = DAL.GetInstance();
        //        var UpdateResult = db.UpdatePortalWithLoginDate(json);
        //        if (UpdateResult) { return Request.CreateResponse(HttpStatusCode.OK); }
        //        else return Request.CreateResponse(HttpStatusCode.NotModified);

        //    }
        //    catch (Exception e)
        //    {

        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, e);
        //    }
        //}
    }
}