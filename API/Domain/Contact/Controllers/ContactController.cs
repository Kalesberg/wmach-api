using API.Data;
using API.Models;
using API.Utilities.Auth;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace API.Controllers
{
    [JWTAuthorization]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ContactController : ApiController
    {
        ///<summary>
        ///Returns list of all contacts owned by sales manager
        ///</summary>
        [HttpGet]
        [Route("api/contact/salesmanContactID/{salemanContactId}")]
        public HttpResponseMessage GetMyClients(string salemanContactId)
        {
            var db = DAL.GetInstance();
            var tokens = new JObject();
            tokens.Add("userContactID", salemanContactId);
            var myClients = db.getMyClients(tokens);
            if (myClients == null) return Request.CreateResponse(HttpStatusCode.NoContent);
            return Request.CreateResponse(HttpStatusCode.OK, myClients);
        }

        [HttpGet]
        [Route("api/contact/accountManager/{accountManagerID}")]
        public HttpResponseMessage GetContactsByAccountManager(string accountManagerID)
        {
            var db = DAL.GetInstance();
            var tokens = new JObject();
            tokens.Add("accountManagerID", accountManagerID);
            var myClients = db.getContactByAccountManagerID(tokens);
            if (myClients == null) return Request.CreateResponse(HttpStatusCode.NoContent);
            return Request.CreateResponse(HttpStatusCode.OK, myClients);
        }



        [HttpGet]
        [Route("api/contact/{contactNTLogin}/getemailgroups")]
        public HttpResponseMessage GetEmailGroupsForContact(string contactNTLogin)
        {
            var db = DAL.GetInstance();
            var tokens = new JObject();
            tokens.Add("ContactNTLogin", contactNTLogin);
            var emailGroups = db.getEmailGroups(tokens);
            if (emailGroups == null) return Request.CreateResponse(HttpStatusCode.NoContent);
            return Request.CreateResponse(HttpStatusCode.OK, emailGroups);
        }

        [HttpGet]
        [Route("api/contact/getbccategories")]
        public HttpResponseMessage GetContactBCCategories()
        {
            var db = DAL.GetInstance();
            var tokens = new JObject();
            var bcCats = db.getBusinessCodeCategories();
            if (bcCats == null) return Request.CreateResponse(HttpStatusCode.NoContent);
            return Request.CreateResponse(HttpStatusCode.OK, bcCats);
        }

        [HttpGet]
        [Route("api/contact/getaccountmanagers")]
        public HttpResponseMessage GetAccountManagers()
        {
            var db = DAL.GetInstance();
            var accountManagers = db.getAccountManagerContacts();
            if (accountManagers == null) return Request.CreateResponse(HttpStatusCode.NoContent);
            return Request.CreateResponse(HttpStatusCode.OK, accountManagers);
        }

        [HttpPost]
        [Route("api/contact/getcontactbyname")]
        public HttpResponseMessage GetContactByName(Contact ctWithName)
        {
            var db = DAL.GetInstance();
            IEnumerable<Contact> contactList = db.getContactByName(JObject.FromObject(ctWithName));
            if (contactList != null)
                return Request.CreateResponse(HttpStatusCode.OK, contactList);
            else
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new List<Contact>());
        }

        [HttpPost]
        [Route("api/contact/getcontactbycompanyname")]
        public HttpResponseMessage GetContactByCompanyName(Contact ctWithCompanyName)
        {
            var db = DAL.GetInstance();
            IEnumerable<Contact> contactList = db.getContactByName(JObject.FromObject(ctWithCompanyName));
            if (contactList != null)
                return Request.CreateResponse(HttpStatusCode.OK, contactList);
            else
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new List<Contact>());
        }

        [HttpPost]
        [Route("api/contact/{contactId}/getcontactdetails")]
        public HttpResponseMessage GetContactDetails(int contactId)
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
        [Route("api/contact/updatecontactdetails")]
        public HttpResponseMessage UpdateContactDetails([FromBody] JObject json)
        {
            try
            {
                var db = DAL.GetInstance();
                var contactDetails = db.updateContactDetails(json);

                if (contactDetails)
                    return Request.CreateResponse(HttpStatusCode.OK, contactDetails);
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpGet]
        [Route("api/contact/salespeople")]
        public HttpResponseMessage GetSalespeople()
        {
            var db = DAL.GetInstance();
            var data = db.getSalespeople();
            return data == null ? Request.CreateResponse(HttpStatusCode.NoContent) : Request.CreateResponse(HttpStatusCode.OK, data);
        }

        [HttpGet]
        [Route("api/contact/coordinators")]
        public HttpResponseMessage GetCoordinator()
        {
            var db = DAL.GetInstance();
            var data = db.getCoordinator();
            return data == null ? Request.CreateResponse(HttpStatusCode.NoContent) : Request.CreateResponse(HttpStatusCode.OK, data);
        }
        [HttpGet]
        [Route("api/contact/salesmanagers")]
        public HttpResponseMessage GetSalesManager()
        {
            var db = DAL.GetInstance();
            var data = db.getSalesManager();
            return data == null ? Request.CreateResponse(HttpStatusCode.NoContent) : Request.CreateResponse(HttpStatusCode.OK, data);
        }
    }
}