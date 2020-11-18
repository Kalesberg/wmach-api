using System.Data;
using System.Net;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using API.Data;
using API.Models;
using Newtonsoft.Json.Linq;
using API.Utilities.Auth;
using API.Managers;
using System.Text;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;


namespace API.Controllers
{
    [JWTAuthorization]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EquipmentController : ApiController
    {

        ///<summary>
        ///Returns equipment photos by EquipmentID and photo size (SM, MD, LG)
        ///</summary>
        [HttpGet]
        [Route("api/equipment/{id}/photos/{size}")]
        public HttpResponseMessage GetEquipmentPhotos(string id, string size)
        {
            var db = DAL.GetInstance();
            if (id == "undefined") return null;
            var tokens = new JObject {{"equipmentID", id}, {"size", size}};
            var photoUrLs = db.getPictureFileNames(tokens);
            if (photoUrLs == null) return Request.CreateResponse(HttpStatusCode.NoContent);
            return Request.CreateResponse(HttpStatusCode.OK, photoUrLs);
        }

        ///<summary>
        ///Deactivates equipment photos by PhotoID
        ///</summary>
        [HttpPost]
        [Route("api/equipment/photos/{id}")]
        public HttpResponseMessage GetEquipmentPhotos(string id)
        {
            var db = DAL.GetInstance();
            if (id == "undefined") return null;
            var tokens = new JObject { { "PhotoID", id } };
            var deactivated = db.deactivateEquipmentPhoto(tokens);
            return deactivated ? Request.CreateResponse(HttpStatusCode.OK, id) : Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        ///<summary>
        ///Creates equipment photos by EquipmentID and photo size (SM, MD, LG)
        ///</summary>
        [HttpPost]
        [Route("api/equipment/{serialNum}/photos")]
        public HttpResponseMessage GetEquipmentPhotos(string serialNum, [FromBody] JObject data)
        {
            try
            {
                if (serialNum == "undefined") return null;
                var tokens = new JObject { { "SerialNum", serialNum } };
                var byteArr = Convert.FromBase64String(data["image"].ToString());
                var photoID = MachineManager.CreateEquipmentPhotos(tokens, byteArr);
                return photoID != 0 ? Request.CreateResponse(HttpStatusCode.Created, photoID) : Request.CreateResponse(HttpStatusCode.InternalServerError, photoID);
            }
            catch(Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        ///<summary>
        ///Returns distinct list of all Manufacturers
        ///</summary>
        [HttpGet]
        [Route("api/equipment/makes")]
        public HttpResponseMessage GetEquipmentManufacturers()
        {
            var db = DAL.GetInstance();
            var makes = db.getEquipmentManufacturers();
            
            return Request.CreateResponse(HttpStatusCode.OK, makes);
        }

        ///<summary>
        ///Returns distinct list of all equipment models
        ///</summary>
        [HttpGet]
        [Route("api/equipment/models")]
        public HttpResponseMessage GetEquipmentModels()
        {
            var db = DAL.GetInstance();
            var models = db.getEquipmentModels();
            return Request.CreateResponse(HttpStatusCode.OK, models);
        }

        ///<summary>
        ///Returns distinct list of all current machine locations
        ///</summary>
        [HttpGet]
        [Route("api/equipment/locations")]
        public HttpResponseMessage GetEquipmentLocations()
        {
            var db = DAL.GetInstance();
            var models = db.getEquipmentLocations();
            return Request.CreateResponse(HttpStatusCode.OK, models);
        }

        //ACCEPTS JSON OF SELECTED MAKE FILTERS. WILL RETURN MODEL FILTERS THAT CAN STILL BE APPLIED WITH SUPPLIED MAKE FILTERS
        [HttpPost]
        [Route("api/equipment/locations")]
        public HttpResponseMessage GetEquipmentLocations([FromBody] JObject json)
        {
            var db = DAL.GetInstance();
            var models = db.getEquipmentLocations(json);
            return Request.CreateResponse(HttpStatusCode.OK, models);
        }

        ///<summary>
        ///Returns distinct list of all equipment rental statuses
        ///</summary>
        [HttpGet]
        [Route("api/equipment/statuses")]
        public HttpResponseMessage GetRentalStatuses()
        {
            var db = DAL.GetInstance();
            var statuses = db.getRentalStatuses();
            return Request.CreateResponse(HttpStatusCode.OK, statuses);
        }

        [HttpPost]
        [Route("api/equipment/statuses")]
        public HttpResponseMessage GetRentalStatuses([FromBody] JObject json)
        {
            var db = DAL.GetInstance();
            var statuses = db.getRentalStatuses(json);
            return Request.CreateResponse(HttpStatusCode.OK, statuses);
        }

        [HttpGet]
        [Route("api/equipment/division")]
        public HttpResponseMessage GetEquipmentDivisions()
        {
            var db = DAL.GetInstance();
            var divisions = db.getDivisions();
            return Request.CreateResponse(HttpStatusCode.OK, divisions);
        }

        [HttpPost]
        [Route("api/equipment/division")]
        public HttpResponseMessage GetEquipmentDivisions([FromBody] JObject json)
        {
            var db = DAL.GetInstance();
            var divisions = db.getDivisions(json);
            return Request.CreateResponse(HttpStatusCode.OK, divisions);
        }

        [HttpGet]
        [Route("api/equipment/filters")]
        public HttpResponseMessage GetAllFilters()
        {
            var db = DAL.GetInstance();
            var filters = db.getAllFilters();
            return Request.CreateResponse(HttpStatusCode.OK, filters);
        }

        [HttpPost]
        [Route("api/equipment/updateequipment")]
        public HttpResponseMessage UpdateEquipment([FromBody] JObject json)
        {
            try
            {
                var db = DAL.GetInstance();
                var updated = db.updateEquipment(json);
                return updated ? Request.CreateResponse(HttpStatusCode.OK, updated) : Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("api/equipment/{equipmentID}/getequipmentaudit")]
        public HttpResponseMessage GetEquipmentAudit(string EquipmentID)
        {
            var tokens = new JObject { { "EquipmentID", EquipmentID } };
            if (EquipmentID == null) return Request.CreateResponse(HttpStatusCode.NoContent);
            var db = DAL.GetInstance();
            var equipAudit = db.getEquipmentAudit(tokens);
            return Request.CreateResponse(HttpStatusCode.OK, equipAudit);
        }

        [HttpGet]
        [Route("api/equipment/{equipmentID}/getequipmentownershiphistory")]
        public HttpResponseMessage GetOwnershipHistoryTable(string EquipmentID)
        {
            var tokens = new JObject { { "EquipmentID", EquipmentID } };
            if (EquipmentID == null) return Request.CreateResponse(HttpStatusCode.NoContent);
            var db = DAL.GetInstance();
            var equipOwnershipHistory = db.getEquipmentOwnerHistory(tokens);
            return Request.CreateResponse(HttpStatusCode.OK, equipOwnershipHistory);
        }


        #region Total Rental Revenue
        /// <summary>
        /// Gets the total rental revenue.
        /// </summary>
        /// <param name="equipmentID">The equipment identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/equipment/{equipmentID}/gettotalrentalrevenue")]
        public HttpResponseMessage GetTotalRentalRevenue(string equipmentID)
        {
            if (equipmentID == "undefined") return null;
            var tokens = new JObject { { "equipmentID", equipmentID } };

            var db = DAL.GetInstance();
            var rentalrevenue = db.getTotalRentalRevenue(tokens);

            return Request.CreateResponse(HttpStatusCode.OK, rentalrevenue);
        }

        /// <summary>
        /// Gets all total rental revenue grouped by EquipmentID
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/equipment/getalltotalrentalrevenue")]
        public HttpResponseMessage GetTotalRentalRevenue()
        {
           
            var db = DAL.GetInstance();
            var rentalrevenue = db.getAllTotalRentalRevenue();

            return Request.CreateResponse(HttpStatusCode.OK, rentalrevenue);
        }
        #endregion

        #region List Rates
        ///<summary>
        ///Returns Equipment List Rates by EquipmentID
        ///</summary>
        [HttpGet]
        [Route("api/equipment/{equipmentID}/listrates")]
        public HttpResponseMessage GetListRatesByID(string EquipmentID)
        {
            var tokens = new JObject { { "EquipmentID", EquipmentID } };
            if (EquipmentID == null) return Request.CreateResponse(HttpStatusCode.NoContent);
            var db = DAL.GetInstance();
            var rates = db.getEquipmentListRates(tokens);
            return Request.CreateResponse(HttpStatusCode.OK, rates);
        }

        ///<summary>
        ///Update ListRates by EquipmentID
        ///</summary>
        [HttpPost]
        [Route("api/equipment/updatelistrates")]
        public HttpResponseMessage UpdateEquipmentListRates([FromBody] JObject json)
        {
            try
            {
                var db = DAL.GetInstance();
                var updated = db.updateEquipmentListRates(json);
                return updated ? Request.CreateResponse(HttpStatusCode.OK, updated) : Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        
        ///<summary>
        ///Returns Equipment Rate History by EquipmentID
        ///</summary>
        [HttpGet]
        [Route("api/equipment/{equipmentID}/listrateshistory")]
        public HttpResponseMessage GetListRateHistoryByID(string EquipmentID)
        {
            var tokens = new JObject { { "EquipmentID", EquipmentID } };
            if (EquipmentID == null) return Request.CreateResponse(HttpStatusCode.NoContent);
            var db = DAL.GetInstance();
            var history =  db.getEquipmentListRateHistory(tokens);
            return Request.CreateResponse(HttpStatusCode.OK, history);
        }

         ///<summary>
        ///Returns Last Equipment Rate Changed Date by EquipmentID
        ///</summary>
        [HttpGet]
        [Route("api/equipment/{equipmentID}/listratechangedates")]
        public HttpResponseMessage GetLastRateChangeDatesByID(string EquipmentID)
        {
            var tokens = new JObject { { "EquipmentID", EquipmentID } };
            if (EquipmentID == null) return Request.CreateResponse(HttpStatusCode.NoContent);
            var db = DAL.GetInstance();
            var history =  db.getLastRateChangeDates(tokens);
            return Request.CreateResponse(HttpStatusCode.OK, history);
        }

        ///<summary>
        ///Returns modelmonthly rate
        ///</summary>
        [HttpPost]
        [Route("api/equipment/model/monthlyrate")]
        public HttpResponseMessage GetModelMonthlyRate([FromBody] JObject json)
        {
            var db = DAL.GetInstance();
            var monthlyRate = db.getModelMonthlyRate(json);
            return Request.CreateResponse(HttpStatusCode.OK, monthlyRate);
        }

        #endregion

        #region List Prices
        ///<summary>
        ///Returns Equipment List Prices by EquipmentID
        ///</summary>
        [HttpGet]
        [Route("api/equipment/{equipmentID}/listprices")]
        public HttpResponseMessage GetListPricesByID(string EquipmentID)
        {
            var tokens = new JObject { { "EquipmentID", EquipmentID } };
            if (EquipmentID == null) return Request.CreateResponse(HttpStatusCode.NoContent);
            var db = DAL.GetInstance();
            var rates = db.getEquipmentListPrices(tokens);
            return Request.CreateResponse(HttpStatusCode.OK, rates);
        }

        ///<summary>
        ///Update ListPrices by EquipmentID
        ///</summary>
        [HttpPost]
        [Route("api/equipment/updatelistprices")]
        public HttpResponseMessage UpdateEquipmentListPrices([FromBody] JObject json)
        {
            try
            {
                var db = DAL.GetInstance();
                var updated = db.updateEquipmentListPrices(json);
                return updated ? Request.CreateResponse(HttpStatusCode.OK, updated) : Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        ///<summary>
        ///CP-193
        ///Returns all equipments on rent by customer
        ///</summary>
        [OverrideActionFilters]
        [CustomerAuthorization]
        [HttpGet]
        [Route("api/equipment/customer/allequipment")]
        public HttpResponseMessage GetAllEquipmentOnRent()
        {       
                var db = DAL.GetInstance();
                var CustomerPortalAuthObj = new CustomerPortalAuth();
                int customerID = CustomerPortalAuthObj.GetCustomerIDFromToken(this.ActionContext);
                if(customerID > 0)
                {
                    var tokens = new JObject();
                    tokens.Add("CustomerID", customerID);
                    var EquipmentOnRents = db.GetAllEquipmentOnRentByEmail(tokens);
                    return EquipmentOnRents == null ? Request.CreateResponse(HttpStatusCode.NoContent) : Request.CreateResponse(HttpStatusCode.OK, EquipmentOnRents);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        #endregion
        [OverrideActionFilters]
        [CustomerAuthorization]
        [HttpPost]
        [Route("api/equipment/customer/equipmentdetail")]
        public HttpResponseMessage GetEquipmentDetailonCustomerContract([FromBody] JObject sqlParams)
        {
            try
            {
                var db = DAL.GetInstance();
                var CustomerPortalAuthObj = new CustomerPortalAuth();
                int customerID = CustomerPortalAuthObj.GetCustomerIDFromToken(this.ActionContext);
                //check customerid match contractid
                JObject check = new JObject { { "ContractID", sqlParams["ContractID"].ToString() }, { "CustomerID", customerID } };
                if (!db.CheckContractIDMatchCustomerID(check))
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
               
                var equipmentdetail = Builder.Build(new EquipmentDetailCustomer(), sqlParams);
                return equipmentdetail.ContractNum >0 ? Request.CreateResponse(HttpStatusCode.OK, equipmentdetail) : Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        ///<summary>
        ///Returns Equipment Price History by EquipmentID
        ///</summary>
        [HttpGet]
        [Route("api/equipment/{equipmentID}/pricehistory")]
        public HttpResponseMessage GetPriceHistoryByID(string EquipmentID)
        {
            var tokens = new JObject { { "EquipmentID", EquipmentID } };
            if (EquipmentID == null) return Request.CreateResponse(HttpStatusCode.NoContent);
            var db = DAL.GetInstance();
            var history = db.getEquipmentPriceHistory(tokens);
            List<PriceHistory> data = new List<PriceHistory>();
            for (int i = 0; i < history.Rows.Count; i++)
            {
                if (i > 0)
                {
                    // Compare with previous row using index
                    if (history.Rows[i]["price"].ToString() != history.Rows[i - 1]["price"].ToString() && ((history.Rows[i]["price"].ToString() != "" && history.Rows[i]["price"].ToString() != "0.0000" && history.Rows[i]["price"].ToString() != "1.0000")
                        || (history.Rows[i-1]["price"].ToString() != "" && history.Rows[i-1]["price"].ToString() != "0.0000" && history.Rows[i-1]["price"].ToString() != "1.0000")))
                    {
                        data.Add(new PriceHistory
                        {
                            OldPrice = history.Rows[i]["price"].ToString(),
                            NewPrice = history.Rows[i - 1]["price"].ToString(),
                            UpdateBy = history.Rows[i - 1]["EditUserStr"].ToString(),
                            DateChanged = history.Rows[i - 1]["EditDateTime"].ToString(),
                            PriceChanged = "List Price"
                        });
                    }
                    if (history.Rows[i]["minprice"].ToString() != history.Rows[i - 1]["minprice"].ToString() && ((history.Rows[i]["minprice"].ToString() != "" && history.Rows[i]["minprice"].ToString() != "0.0000" && history.Rows[i]["minprice"].ToString() != "1.0000")
                        || (history.Rows[i - 1]["minprice"].ToString() != "" && history.Rows[i - 1]["minprice"].ToString() != "0.0000" && history.Rows[i - 1]["minprice"].ToString() != "1.0000")))
                    {
                        data.Add(new PriceHistory
                        {
                            OldPrice = history.Rows[i]["minprice"].ToString(),
                            NewPrice = history.Rows[i - 1]["minprice"].ToString(),
                            UpdateBy = history.Rows[i - 1]["EditUserStr"].ToString(),
                            DateChanged = history.Rows[i - 1]["EditDateTime"].ToString(),
                            PriceChanged = "Dealer Price"
                        });
                    }
                    if (history.Rows[i]["brokerprice"].ToString() != history.Rows[i - 1]["brokerprice"].ToString() && ((history.Rows[i]["brokerprice"].ToString() != "" && history.Rows[i]["brokerprice"].ToString() != "0.0000" && history.Rows[i]["brokerprice"].ToString() != "1.0000")
                        || (history.Rows[i - 1]["brokerprice"].ToString() != "" && history.Rows[i - 1]["brokerprice"].ToString() != "0.0000" && history.Rows[i - 1]["brokerprice"].ToString() != "1.0000")))
                    {
                        data.Add(new PriceHistory
                        {
                            OldPrice = history.Rows[i]["brokerprice"].ToString(),
                            NewPrice = history.Rows[i - 1]["brokerprice"].ToString(),
                            UpdateBy = history.Rows[i - 1]["EditUserStr"].ToString(),
                            DateChanged = history.Rows[i - 1]["EditDateTime"].ToString(),
                            PriceChanged = "Special Price"
                        });
                    }
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

    }
}