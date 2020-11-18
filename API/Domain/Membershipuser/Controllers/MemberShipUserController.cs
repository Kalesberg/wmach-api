using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using API.Data;
using API.Utilities.EmailSystem.BusinessLayer.Classes;
using API.Models;
using Newtonsoft.Json.Linq;
using API.Utilities.Auth;
using API.Utilities;
using System.Configuration;
using System.IO;
using System.Text;
using API.Utilities.Password;
using System.Web.Hosting;


namespace API.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MemberShipUserController : ApiController
    {
        ///<summary>
        ///Returns Guid store in CustomerPortalUser Table
        ///Guid conterns informationabout user
        ///</summary>
        [HttpGet]
        [Route("api/ext/getusers/{guid}")]
        public HttpResponseMessage GetMemberShipUser(String guid)
        {

            try
            {

                string TimeStampCrypted = string.Empty;
                string EmailFromDB = string.Empty;
                var secretKey = ConfigurationManager.AppSettings["CustomerPortalAuth"];
                var PasswordObj = new API.Utilities.Password.Password();
                var CustomerPortalObj = new CustomerPortalAuth();
                //Check if the link is stored in db

                var db = DAL.GetInstance();

                var CustomerPortalUser = db.GetCustomerPortalUserByGuid(guid);

                if (CustomerPortalUser.Count() > 0)
                {
                    //Get TimeStampEncrypted  from Db
                    foreach (var col in CustomerPortalUser)
                    {
                        TimeStampCrypted = col.TimeStampEncrypted;

                    }

                    //Get TimeStamp from link

                    DateTime TimeStamp = DateTime.Parse(PasswordObj.Decrypt(TimeStampCrypted, secretKey));

                    //Check if link is expired

                    if ((DateTime.Now - TimeStamp).TotalHours > 24)
                    {
                        return Request.CreateResponse(HttpStatusCode.Unauthorized, "Link is expired");
                    }
                    else
                    {

                        var MemberShipUser = db.GetCustomerPortalUserByGuid(guid);

                        if (MemberShipUser.Count() > 0)
                        {
                            var MemberShipUserJsonObject = new JObject();

                            foreach (var col in MemberShipUser)
                            {
                                //Build JsonObject with MemberShipUser Values
                                MemberShipUserJsonObject = new JObject { 

                          
                           { "Email", col.Email }, 
                           { "FirstName", col.FirstName }, 
                           { "LastName", col.LastName },
                           { "AccessToPortal", col.AccesstoPortal }, 
                           { "ResetPwd", col.ResetPwd },
                           { "Role", col.Role },
                           { "CustomerId", col.CustomerID }
                           };

                            }

                            return Request.CreateResponse(HttpStatusCode.OK, MemberShipUserJsonObject);
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.NotFound, "No user has been found");
                        }
                    }
                }

                else
                {
                    //Access unauthorize

                    return Request.CreateResponse(HttpStatusCode.Unauthorized, "this link is not stored");
                }

            }
            catch (Exception e)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }

        }



        ///<summary>
        ///Handle Reset Password or registration request
        ///Update CustomerPortalUser table with new password  
        ///</summary>
        [HttpPost]
        [Route("api/ext/resetpwd/{guid}")]
        public async System.Threading.Tasks.Task<HttpResponseMessage> ResetPassword(string guid, [FromBody] JObject json)
        {
            string TimeStampCrypted = null;
            string EmailFromDB = string.Empty;
            var secretKey = ConfigurationManager.AppSettings["CustomerPortalAuth"];
            var PasswordObj = new API.Utilities.Password.Password();
            var CustomerPortalObj = new CustomerPortalAuth();
            //Check if the link is stored in db

            var db = DAL.GetInstance();

            var CustomerPortalUser = db.GetCustomerPortalUserByGuid(guid);

            if (CustomerPortalUser.Count() > 0)
            {
                //Get TimeStampEncrypted and Email from Db
                foreach (var col in CustomerPortalUser)
                {
                    TimeStampCrypted = col.TimeStampEncrypted;
                    EmailFromDB = col.Email;
                }

                //Get TimeStamp from link

                DateTime TimeStamp = DateTime.Parse(PasswordObj.Decrypt(TimeStampCrypted, secretKey));

                //Check if link is expired

                if ((DateTime.Now - TimeStamp).TotalHours > 24)
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, "Link is expired");
                }
                else
                {

                    //Get pwd from Json Object

                    string pwd = json["Password"].ToString();

                    string TimeStampLink = string.Empty;

                    //Generate a new password

                    //Hash Password

                    var PwdHashed = PasswordObj.HashPassword(pwd);

                    //Generate a new Guid value

                    //Step 1 . Encryption of TimeStamp

                    var TimeStampEncrypted = PasswordObj.Encrypt(DateTime.Now.ToString(), secretKey);

                    //Step 2 . Get TimeStamp Correct link from TimeStampEncrypted

                    TimeStampLink = CustomerPortalObj.FormatLink(TimeStampEncrypted);


                    //Update CustomerPortalUser Table with new password, New link value ....

                    try
                    {

                        var update = db.ResetPassword(EmailFromDB, PwdHashed, DateTime.Now, TimeStampEncrypted, TimeStampLink);

                        //WG-161
                        //Email Customer
                        if (update == true)
                        {
                            string link = string.Empty;
                            string Email = EmailFromDB;
                            //Get Environment

                            string Env = ConfigurationManager.ConnectionStrings["mach1"].ConnectionString;

                            //Build the link by Environnement


                            if (Env.Contains("galsql01"))
                            {
                                link = "https://staging.customers.wwmach.com/login/";
                                Email = "testsupports@wwmach.com";
                            }

                            else if (Env.Contains("localhost"))
                            {
                                link = "https://staging.customers.wwmach.com/login/";
                                Email = "testsupports@wwmach.com";
                            }
                            else
                            {
                                link = "https://customers.wwmach.com/login/";

                            }

                            string CurrentYear = DateTime.Now.Year.ToString();
                            string body = string.Empty;
                            //using streamreader for reading my htmltemplate   
                            using (StreamReader reader = new StreamReader(HostingEnvironment.MapPath(("~/Templates/HTML/PasswordResetSuccess.html"))))
                            {
                                body = reader.ReadToEnd();
                            }
                            body = body.Replace("{link}", link);
                            body = body.Replace("{CurrentYear}", CurrentYear);

                            //Send Email

                            await new Email("Customer Portal", "DoNotReply@wwmach.com", Email,
                                           "Reset Password Success", body).Send();


                            return Request.CreateResponse(HttpStatusCode.OK);
                        }

                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.Unauthorized);
                        }

                    }
                    catch (Exception e)
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
                    }

                }

            }

            else
            {
                //Access unauthorize

                return Request.CreateResponse(HttpStatusCode.Unauthorized, "this link is not stored");
            }

        }



        /// <summary>
        /// Handle Authentication to customer portal
        /// </summary>
        /// <param name="json"></param>
        // <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/ext/auth/")]

        public HttpResponseMessage Authentification([FromBody] JObject json)
        {

            string token = null;

            //Get Email

            string Email = json["Email"].ToString();

            //Get Password

            string Pwd = json["Password"].ToString();

            //Check if Account user is valide

            var CustomerPortalAuthObj = new CustomerPortalAuth();

            var AuthObj = CustomerPortalAuthObj.IsAuthenticate(Email, Pwd);

            if (AuthObj["AcoountValide"] == true && AuthObj["AccestoPortal"] == true)
            {

                //Get CustomerPortalUser
                var db = DAL.GetInstance();

                var CustomerPortalUser = db.GetCustomerPortalUserByEmail(Email);

                token = CustomerPortalAuthObj.GetToken(CustomerPortalUser);

                var response = new HttpResponseMessage();
                response.StatusCode = HttpStatusCode.OK;

                //update portal with login date
                var sqlparams = new JObject();
                DateTime dtLogin = DateTime.Now;
                sqlparams.Add("Email", Email);
                sqlparams.Add("LoginDate", dtLogin);
                var cpUser = db.GetCustomerPortalUserByEmail(Email);
                if (cpUser.Count() > 0)
                {
                    db.UpdatePortalWithLoginDate(sqlparams);
                    //return Request.CreateResponse(HttpStatusCode.OK, "Updated with Login Date");
                }

                //Expose response header
                response.Headers.Add("Access-Control-Allow-Origin", "*");
                response.Headers.Add("Access-Control-Allow-Methods", "POST,GET");
                response.Headers.Add("Authorization", "Bearer " + token);
                response.Headers.Add("Access-Control-Allow-Headers", "Authorization");
                response.Headers.Add("Access-Control-Expose-Headers", "Authorization");

                return response;


            }
            else if (AuthObj["AcoountValide"] == true && AuthObj["AccestoPortal"] == false)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden, "Your access to customer portal has been disabled");
            }
            else if (AuthObj["AcoountValide"] == false && AuthObj["AccestoPortal"] == false)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, "Wrong user name or password");
            }

            return Request.CreateResponse(HttpStatusCode.Unauthorized, "Wrong user name or password");

        }

        ///<summary>
        ///Handle reset password request from CustomerPortal Side
        ///<paramref name="json"/>
        ///<return>HttpResponseMessage</return>
        ///</summary>

        [HttpPost]
        [Route("api/ext/forgotpassword/")]
        public async System.Threading.Tasks.Task<HttpResponseMessage> ResetPasswordRequestFormCustomerPortal([FromBody] JObject json)
        {

            var PasswordObj = new API.Utilities.Password.Password();
            var secretKey = ConfigurationManager.AppSettings["CustomerPortalAuth"];
            string TimeStampLink = string.Empty;
            var CustomerPortalObj = new CustomerPortalAuth();

            //Get Email

            string Email = json["Email"].ToString();

            //Check if Email existe in customerPortalUser
            //Search username in CustomerPortalUser table

            var db = DAL.GetInstance();

            var oMembershipUser = db.GetCustomerPortalUserByEmail(Email);

            if (oMembershipUser.Count() > 0)
            {
                //Create a new guid

                //Generate a new link value

                //Step 1 . Encryption of TimeStamp

                var TimeStampEncrypted = PasswordObj.Encrypt(DateTime.Now.ToString(), secretKey);

                //Step 2 . Get TimeStamp Correct link from TimeStampEncrypted

                TimeStampLink = CustomerPortalObj.FormatLink(TimeStampEncrypted);


                //Update CustomerPortalUser with new guid , DateTimeNow and Resetpwd =1

                var update = db.ResetPwdRequestCustomerPortaluser(Email, TimeStampEncrypted, TimeStampLink);

                if (update == true)
                {
                    string link = string.Empty;

                    //Get Environment

                    string Env = ConfigurationManager.ConnectionStrings["mach1"].ConnectionString;

                    //Build the link by Environnement


                    if (Env.Contains("galsql01"))
                    {
                        link = "https://staging.customers.wwmach.com/login/" + TimeStampLink;
                        Email = "testsupports@wwmach.com";
                    }

                    else if (Env.Contains("localhost"))
                    {
                        link = "https://staging.customers.wwmach.com/login/" + TimeStampLink;
                        Email = "testsupports@wwmach.com";
                    }
                    else
                    {
                        link = "https://customers.wwmach.com/login/" + TimeStampLink;

                    }

                    string CurrentYear = DateTime.Now.Year.ToString();
                    string body = string.Empty;
                    //using streamreader for reading my htmltemplate   
                    using (StreamReader reader = new StreamReader(HostingEnvironment.MapPath(("~/Templates/HTML/ForgotPasswordTemplate.html"))))
                    {
                        body = reader.ReadToEnd();
                    }
                    body = body.Replace("{link}", link);
                    body = body.Replace("{CurrentYear}", CurrentYear);

                    //Send Email

                    await new Email("Customer Portal", "DoNotReply@wwmach.com", Email,
                                   "Reset Password Request", body).Send();

                    return Request.CreateResponse(HttpStatusCode.OK);


                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "An error occured during update");
                }


            }

            else
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, "That email doesn't exist in CustomerPortalUser");
            }

        }


        /// <summary>
        /// Check Email When customer Request Access and create a record for every new customer access request
        /// </summary>
        /// <param name="json">Form Object</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/ext/checkemail")]
        public HttpResponseMessage CheckEmailWhenRequestAccess([FromBody] JObject json)
        {
            var db = DAL.GetInstance();
            var email = json.Value<string>("Email");
            JObject jObject = new JObject { { "Email", email } };
            var result = db.CheckEmailWhenRequestAccess(jObject);
            if (result == 3)
            {
                //Creates a record if its a new customer request
                var res = db.CreatePortalAccessRequestObject(json);
                //if (res == 0)
                //    return Request.CreateResponse(HttpStatusCode.OK, res);
            } 
            return result != 0 ? Request.CreateResponse(HttpStatusCode.OK, result) : Request.CreateResponse(HttpStatusCode.InternalServerError);

        }


        /// <summary>
        /// customer request access and has account in m1 auto create account for them by email
        /// </summary>
        [HttpPost]
        [Route("api/customers/requestaccess")]
        public async System.Threading.Tasks.Task<HttpResponseMessage> AutoCreateCustomerportalAccount([FromBody] JObject json)
        {
            try
            {
                var db = DAL.GetInstance();
                //var tokenContact = new JObject { { "ContactID", json["CustomerId"].ToString() } };
                //var contact = db.getContactByContactID(tokenContact);
                var tokens = new JObject();
                //tokens.Add("Email", contact.Email);
                //if (db.CheckifCustomerPortalAccountExist(tokens) == 1)
                //{
                tokens.Add("EnterUserStr", "IT");
                var customerID = db.GetCustomerIDByEmail(json);
                tokens.Add("CustomerID", customerID);
                if(customerID !=0)
                {
                    var customerResult = db.CreateNewCustomerPortalAccount(tokens);

                    if (customerResult)
                    {
                        //send passwordreset email
                        await db.SendPasswordResetEmail(tokens["TimeStampLink"].ToString(), json["Email"].ToString());
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                        return Request.CreateResponse(HttpStatusCode.InternalServerError); 
                }
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError); 

            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e);
            }
        }

    }

}

