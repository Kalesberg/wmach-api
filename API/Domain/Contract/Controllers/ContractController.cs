using API.Data;
using API.Managers;
using API.Models;
using API.Utilities.Auth;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace API.Controllers
{
    [JWTAuthorization]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ContractController : ApiController
    {

        #region Additional Salespeople
        /// <summary>
        /// Gets all additional salespeople on a contract
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/contract/{contractid}/getcontractadditionalsalespeople")]
        public HttpResponseMessage GetAdditionalSalespeople(string ContractID)
        {
            var tokens = new JObject { { "ContractID", ContractID } };
            if (ContractID == null) return Request.CreateResponse(HttpStatusCode.NoContent);

            var db = DAL.GetInstance();
            var additionalsalespeople = db.getAdditionalSalespeople(tokens);

            return Request.CreateResponse(HttpStatusCode.OK, additionalsalespeople);
        }

        /// <summary>
        /// Gets all additional salespeople on a contract (single comma delimited field)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/contract/{contractid}/getcontractadditionalsalespeopledelimited")]
        public HttpResponseMessage GetAdditionalSalespeopleDelimited(string ContractID)
        {
            var tokens = new JObject { { "ContractID", ContractID } };
            if (ContractID == null) return Request.CreateResponse(HttpStatusCode.NoContent);

            var db = DAL.GetInstance();
            var additionalsalespeople = db.getAdditionalSalespeopleDelimited(tokens);

            return Request.CreateResponse(HttpStatusCode.OK, additionalsalespeople);
        }

        ///<summary>
        /// Insert additional Salespeople
        ///</summary>
        [HttpPost]
        [Route("api/contract/insertadditionalsalesperson")]
        public HttpResponseMessage InsertAdditionalSalesperson([FromBody] JObject json)
        {
            try
            {
                var db = DAL.GetInstance();
                var inserted = db.InsertAdditionalSalesperson(json);
                return inserted ? Request.CreateResponse(HttpStatusCode.OK, inserted) : Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        ///<summary>
        /// Delete additional Salespeople
        ///</summary>
        [HttpPost]
        [Route("api/contract/deleteadditionalsalesperson")]
        public HttpResponseMessage DeleteAdditionalSalesperson([FromBody] JObject json)
        {
            try
            {
                var db = DAL.GetInstance();
                var deleted = db.DeleteAdditionalSalesperson(json);
                return deleted ? Request.CreateResponse(HttpStatusCode.OK, deleted) : Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        #endregion

        ///<summary>
        ///Returns contract detail by contractid
        ///</summary>
        [OverrideActionFilters]
        [CustomerAuthorization]
        [HttpGet]
        [Route("api/contract/{contractID}")]
        public HttpResponseMessage GetQuotesDetailByQuoteID(int contractID)
        {
            var db = DAL.GetInstance();
            var CustomerPortalAuthObj = new CustomerPortalAuth();
            int customerID = CustomerPortalAuthObj.GetCustomerIDFromToken(this.ActionContext);
            //check customerid match contractid
            JObject check = new JObject { { "ContractID", contractID }, { "CustomerID", customerID } };
            if (!db.CheckContractIDMatchCustomerID(check))
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            var sqlParams = new JObject { { "contractID", contractID } };
            var quote = Builder.Build(new ContractDetail(), sqlParams);
            return quote == null ? Request.CreateResponse(HttpStatusCode.NoContent) : Request.CreateResponse(HttpStatusCode.OK, quote);

        }

        ///<summary>
        ///Save Contract signature data
        ///</summary>
        [OverrideActionFilters]
        [CustomerAuthorization]
        [HttpPost]
        [Route("api/contract/signature")]
        public async Task<HttpResponseMessage> PostContractSignature([FromBody] JObject json)
        {
            try
            {
                var db = DAL.GetInstance();
                var CustomerPortalAuthObj = new CustomerPortalAuth();
                int customerID = CustomerPortalAuthObj.GetCustomerIDFromToken(this.ActionContext);
                json.Add("CustomerID", customerID);
                //check customerid match contractid
                JObject check = new JObject { { "ContractID", json["ContractID"].ToString() }, { "CustomerID", customerID } };
                if (!db.CheckContractIDMatchCustomerID(check))
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                if (json["Image"].ToString().Length <= 0 || json["Name"].ToString().Length <= 0 || json["JobTitle"].ToString().Length <= 0)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                string imageString = json["Image"].ToString();
                string imageName = json["SignatureImage"].ToString();
                string imageType = json["ImageType"].ToString();
                if (db.SaveImage(imageString, imageName, imageType))
                {
                    string folder;
                    if (imageName.ToLower().Contains("job"))
                        folder = "jobsheet/";
                    else if (imageName.ToLower().Contains("padding"))
                        folder = "addendum/";
                    else
                        folder = "";

                    List<Files> file = new List<Files>();
                    file.Add(new Files() { ID = 1, Data = "data:application/pdf;base64," + json["Image"].ToString() });
                    var results = await db.getFileURLFromCloud(file, "contract", folder, true, false);

                    json.Remove("Image");
                    json.Add("AzureUrl", results.FirstOrDefault().Data);
                    var inserted = db.InsertContractSignature(json);
                    return inserted ? Request.CreateResponse(HttpStatusCode.OK) : Request.CreateResponse(HttpStatusCode.InternalServerError);
                }
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Save Image Fail");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        ///<summary>
        ///Get Contract signature data
        ///</summary>

        [OverrideActionFilters]
        [CustomerAuthorization]
        [HttpGet]
        [Route("api/contract/signature/{contractID}")]
        public HttpResponseMessage GetContractSigngature(int contractID)
        {
            var db = DAL.GetInstance();
            var CustomerPortalAuthObj = new CustomerPortalAuth();
            int customerID = CustomerPortalAuthObj.GetCustomerIDFromToken(this.ActionContext);
            var sqlParams = new JObject { { "ContractID", contractID } };

            //check customerid match contractid
            JObject check = new JObject { { "ContractID", contractID }, { "CustomerID", customerID } };
            if (!db.CheckContractIDMatchCustomerID(check))
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            var contractSignature = db.GetContractSignatureByContractID(sqlParams);
            //string path = @"C:\Users\admin\Desktop\dbimage" + '\\' + contractSignature.SignatureImage + "." +contractSignature.ImageType;
            if (contractSignature != null)
            {
                foreach (ContractSignature sig in contractSignature)
                {
                    string path = System.Configuration.ConfigurationManager.AppSettings["ContractSignatureDirectory"] + '\\' + sig.SignatureImage + "." + sig.ImageType;
                    byte[] b = System.IO.File.ReadAllBytes(path);

                    sig.Image = Convert.ToBase64String(b);
                }
            }
            return contractSignature == null ? Request.CreateResponse(HttpStatusCode.NoContent) : Request.CreateResponse(HttpStatusCode.OK, contractSignature);

        }

        [HttpGet]
        [Route("api/contract/signature/internal/{ContractID}")]
        public HttpResponseMessage GetContractSignatureForInternalUsers(int contractId)
        {
            var db = DAL.GetInstance();
            JObject sqlParams = new JObject { { "ContractID", contractId } };
            var contractSignature = db.GetContractSignatureByContractID(sqlParams);
            //string path = @"C:\Users\admin\Desktop\dbimage" + '\\' + contractSignature.SignatureImage + "." +contractSignature.ImageType;
            if (contractSignature != null)
            {
                foreach (ContractSignature sig in contractSignature)
                {
                    string path = System.Configuration.ConfigurationManager.AppSettings["ContractSignatureDirectory"] + '\\' + sig.SignatureImage + "." + sig.ImageType;
                    byte[] b = System.IO.File.ReadAllBytes(path);

                    sig.Image = Convert.ToBase64String(b);
                }
            }
            return contractSignature == null ? Request.CreateResponse(HttpStatusCode.NoContent) : Request.CreateResponse(HttpStatusCode.OK, contractSignature);

        }


        [HttpGet]
        [Route("api/contract/viewsignature/{contractID}")]
        public HttpResponseMessage GetContractSigngatureWithoutCheck(int contractID)
        {
            var db = DAL.GetInstance();

            var sqlParams = new JObject { { "ContractID", contractID } };

            var contractSignature = db.GetContractSignatureByContractID(sqlParams);
            //string path = @"C:\Users\admin\Desktop\dbimage" + '\\' + contractSignature.SignatureImage + "." +contractSignature.ImageType;
            if (contractSignature != null)
            {
                foreach (ContractSignature sig in contractSignature)
                {
                    string path = System.Configuration.ConfigurationManager.AppSettings["ContractSignatureDirectory"] + '\\' + sig.SignatureImage + "." + sig.ImageType;
                    byte[] b = System.IO.File.ReadAllBytes(path);

                    sig.Image = Convert.ToBase64String(b);
                }
            }
            return contractSignature == null ? Request.CreateResponse(HttpStatusCode.NoContent) : Request.CreateResponse(HttpStatusCode.OK, contractSignature);

        }

        [OverrideActionFilters]
        [CustomerAuthorization]
        [HttpGet]
        [Route("api/contract/customer/allcontract")]
        public HttpResponseMessage GetAllContractForCustomer()
        {
            var CustomerPortalAuthObj = new CustomerPortalAuth();
            int customerID = CustomerPortalAuthObj.GetCustomerIDFromToken(this.ActionContext);
            if (customerID > 0)
            {
                var db = DAL.GetInstance();
                var tokens = new JObject();
                tokens.Add("CustomerID", customerID);
                var quotes = db.GetListContractForCustomer(tokens);
                return Request.CreateResponse(HttpStatusCode.OK, quotes);
            }

            else
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        ///<summary>
        ///save job sheet info
        ///</summary>
        [OverrideActionFilters]
        [CustomerAuthorization]
        [HttpPost]
        [Route("api/contract/jobsheet")]
        public HttpResponseMessage JobInfoSheetSave([FromBody] JObject json)
        {
            try
            {
                var db = DAL.GetInstance();
                var CustomerPortalAuthObj = new CustomerPortalAuth();
                int customerID = CustomerPortalAuthObj.GetCustomerIDFromToken(this.ActionContext);
                //check customerid match contractid
                JObject check = new JObject { { "ContractID", json["ContractID"].ToString() }, { "CustomerID", customerID } };
                if (!db.CheckContractIDMatchCustomerID(check))
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                bool saved = JobInfoSheetManager.Save(json);
                return saved ? Request.CreateResponse(HttpStatusCode.OK) : Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        ///<summary>
        ///view job sheet by contractnum
        ///</summary>
        [OverrideActionFilters]
        [CustomerAuthorization]
        [HttpGet]
        [Route("api/contract/jobsheet/{contractID}")]
        public HttpResponseMessage JobInfoSheetView(int contractID)
        {
            try
            {
                var db = DAL.GetInstance();
                var CustomerPortalAuthObj = new CustomerPortalAuth();
                int customerID = CustomerPortalAuthObj.GetCustomerIDFromToken(this.ActionContext);
                //check customerid match contractid
                JObject check = new JObject { { "ContractID", contractID }, { "CustomerID", customerID } };
                if (!db.CheckContractIDMatchCustomerID(check))
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                var sqlParams = new JObject { { "ContractID", contractID } };
                var data = db.JobInfoSheetView(sqlParams);
                return data != null ? Request.CreateResponse(HttpStatusCode.OK, data) : Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

    }
}
