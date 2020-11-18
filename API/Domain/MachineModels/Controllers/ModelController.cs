using System.Data;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using API.Data;
using Newtonsoft.Json.Linq;
using API.Utilities.Auth;
using API.Managers;
using System.Text;
using System;
using API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace API.Controllers
{
        [JWTAuthorization]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public class ModelController : ApiController
        {
            #region List Rates
            ///<summary>
            ///Returns Equipment List Rates by InventoryMasterID
            ///</summary>
            [HttpGet]
            [Route("api/model/{InventoryMasterID}/listrates")]
            public HttpResponseMessage GetListRatesByModelID(string InventoryMasterID)
            {
                var tokens = new JObject { { "InventoryMasterID", InventoryMasterID } };
                if (InventoryMasterID == null) return Request.CreateResponse(HttpStatusCode.NoContent);
                var db = DAL.GetInstance();
                var rates = db.getModelListRates(tokens);
                return Request.CreateResponse(HttpStatusCode.OK, rates);
            }

            ///<summary>
            ///Update ListRates by InventoryMasterID
            ///</summary>
            [HttpPost]
            [Route("api/model/updatelistrates")]
            public HttpResponseMessage UpdateModelListRates([FromBody] JObject json)
            {
                try
                {
                    var db = DAL.GetInstance();
                    var updated = db.updateModelListRates(json);
                    return updated ? Request.CreateResponse(HttpStatusCode.OK, updated) : Request.CreateResponse(HttpStatusCode.InternalServerError);
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
                }
            }

            ///<summary>
            ///Returns Equipment List Rates by InventoryMasterID
            ///</summary>
            [HttpPost]
            [Route("api/model/GetAllModelFields")]
            public HttpResponseMessage GetAllModelFields([FromBody] JObject json)
            {
                try
                {
                    var db = DAL.GetInstance();
                    var modelData = db.getModelFields(json);
                    return Request.CreateResponse(HttpStatusCode.OK, modelData);
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
                }

            }

            ///<summary>
            ///Returns all data for wrs and mobile rental rate
            ///</summary>
            [HttpGet]
            [Route("api/model/GetModelData")]
            public HttpResponseMessage GetAllModelFields()
            {
                try
                {
                    var db = DAL.GetInstance();
                    var modelData = db.getModelData();
                    return Request.CreateResponse(HttpStatusCode.OK, modelData);
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
                }

            }
            #endregion
        

            #region Additional Model Columns
            ///<summary>
            ///Returns Additional Columns by InventoryMasterID
            ///</summary>
            [HttpGet]
            [Route("api/model/{InventoryMasterID}/additionalModelColumns")]
            public HttpResponseMessage GetAdditionalModelColByModelID(string InventoryMasterID)
            {
                var tokens = new JObject { { "InventoryMasterID", InventoryMasterID } };
                if (InventoryMasterID == null) return Request.CreateResponse(HttpStatusCode.NoContent);
                var db = DAL.GetInstance();
                var rates = db.getAdditionalModelColumns(tokens);
                return Request.CreateResponse(HttpStatusCode.OK, rates);
            }

            ///<summary>
            ///Update Additional model columns by InventoryMasterID
            ///</summary>
            [HttpPost]
            [Route("api/model/updateAdditionalModelColumns")]
            public HttpResponseMessage UpdateAdditionalModelColumns([FromBody] JObject json)
            {
                try
                {
                    var db = DAL.GetInstance();
                    var updated = db.updateAdditionalModelColumns(json);
                    return updated ? Request.CreateResponse(HttpStatusCode.OK, updated) : Request.CreateResponse(HttpStatusCode.InternalServerError);
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
                }
            }
         #endregion

            ///<summary>
            ///Returns New Model detail by inventoroymasterid
            ///</summary>
            [HttpGet]
            [Route("api/model/{InventoryMasterID}")]
            public HttpResponseMessage GetNewModelDetailbyInventoroymasterid(string InventoryMasterID)
            {
                var tokens = new JObject { { "InventoryMasterID", InventoryMasterID } };
                if (InventoryMasterID == null) return Request.CreateResponse(HttpStatusCode.NoContent);
                var db = DAL.GetInstance();
                var model = Builder.Build(new ModelDetail(), tokens);
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }

            ///<summary>
            ///Returns New Model detail by model name
            ///</summary>
            [HttpPost]
            [Route("api/model/detail")]
            public HttpResponseMessage GetNewModelDetailbyInventoroymasterid([FromBody] JObject json)
            {
                var db = DAL.GetInstance();
                int InventoryMasterID = db.GetModelIDByModelNum(json);
                var tokens = new JObject { { "InventoryMasterID", InventoryMasterID } };
                if (InventoryMasterID == 0) return Request.CreateResponse(HttpStatusCode.NoContent);
                var model = Builder.Build(new ModelDetail(), tokens);
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }


            ///<summary>
            ///Returns image url, save image to azure cloud storage 
            ///</summary>
            [HttpPost]
            [Route("api/model/images/upload")]
            public async Task<HttpResponseMessage> SaveModelImageToAzure(List<Files> json)
            {
                var db = DAL.GetInstance();
                var results = await db.getFileURLFromCloud(json, "models", "photos/original/", true, true);
                return  Request.CreateResponse(HttpStatusCode.OK, results);
            }

            ///<summary>
            ///Returns file url, save file to azure cloud storage 
            ///</summary>
            [HttpPost]
            [Route("api/model/files/upload")]
            public async Task<HttpResponseMessage> SaveModelFileToAzure(List<Files> json)
            {
                var db = DAL.GetInstance();
                var results = await db.getFileURLFromCloud(json, "models", "files/", true, false);
                return Request.CreateResponse(HttpStatusCode.OK, results);
            }

            ///<summary>
            ///create new model
            ///</summary>
            [HttpPost]
            [Route("api/model/create")]
            public HttpResponseMessage CreateNewModel(InventoryMaster json)
            {
                var db = DAL.GetInstance();
                var created = MachineModelsManager.Create(json);
                return created ? Request.CreateResponse(HttpStatusCode.Created) : Request.CreateResponse(HttpStatusCode.InternalServerError);
             
            }

            ///<summary>
            ///Update new model
            ///</summary>
            [HttpPost]
            [Route("api/model/update")]
            public HttpResponseMessage UpdateModel(InventoryMaster json)
            {
                var db = DAL.GetInstance();
                var updated = MachineModelsManager.Update(json);
                return updated ? Request.CreateResponse(HttpStatusCode.OK) : Request.CreateResponse(HttpStatusCode.InternalServerError);

            }

            ///<summary>
            ///create new  machine categrory
            ///</summary>
            [HttpPost]
            [Route("api/category/create")]
            public HttpResponseMessage CreateCategory([FromBody]JObject json)
            {
                var db = DAL.GetInstance();
                json["EnterUserStr"] = "WWM\\" + json["EnterUserStr"].ToString();
                var created = db.CreateCategory(json);
                return created != 0 ? Request.CreateResponse(HttpStatusCode.OK, new JObject { { "ID", created }, { "Value", json["CategoryName"].ToString() }, { "Visible", true } }) : Request.CreateResponse(HttpStatusCode.InternalServerError);

            }

            ///<summary>
            ///return list of rentalreservationgroup
            ///</summary>
            [HttpGet]
            [Route("api/rentalreservationgroup/list")]
            public HttpResponseMessage CreateRentalreservationgroup()
            {
                var db = DAL.GetInstance();
                var list = db.getRentalReservationGroupList();
                return list!=null ? Request.CreateResponse(HttpStatusCode.OK,list) : Request.CreateResponse(HttpStatusCode.InternalServerError);

            }

            ///<summary>
            ///get other model on  this rental group
            ///</summary>
            [HttpGet]
            [Route("api/rentalreservationgroup/{name}")]
            public HttpResponseMessage CreateRentalreservationgroup(string name)
            {
                var db = DAL.GetInstance();
                var sqlParams = new JObject { { "RentalReservationGroup", name } };
                var list = db.getOtherModelonRentalReservationGroup(sqlParams);
                return list != null ? Request.CreateResponse(HttpStatusCode.OK, list) : Request.CreateResponse(HttpStatusCode.InternalServerError);

            }

            ///<summary>
            ///create new  manufacturer
            ///</summary>
            [HttpPost]
            [Route("api/manufacturer/create")]
            public HttpResponseMessage CreateManufacturer([FromBody]JObject json)
            {
                var db = DAL.GetInstance();
                json["EnterUserStr"] = "WWM\\" + json["EnterUserStr"].ToString();
                var created = db.CreateManufacturer(json);
                return created != 0 ? Request.CreateResponse(HttpStatusCode.OK, new JObject { { "ID", created }, { "Value", json["ManufacturerName"].ToString() }, { "Visible", true } }) : Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            ///<summary>
            ///create new rentalreservation include rate 
            ///</summary>
            [HttpPost]
            [Route("api/rentalreservationgroup/create")]
            public HttpResponseMessage CreateRentalreservationgroupDetail(RentalReservationGroupData RentalReservationGroup)
            {
                var db = DAL.GetInstance();
                var created = RentalReservationGroupManager.Create(RentalReservationGroup);
                return created ? Request.CreateResponse(HttpStatusCode.Created) : Request.CreateResponse(HttpStatusCode.InternalServerError);

            }

            ///<summary>
            ///Update new rentalreservation include rate 
            ///</summary>
            [HttpPost]
            [Route("api/rentalreservationgroup/update")]
            public HttpResponseMessage UpdateRentalreservationgroupDetail(RentalReservationGroupData RentalReservationGroup)
            {
                var db = DAL.GetInstance();
                var updated = RentalReservationGroupManager.Update(RentalReservationGroup);
                return updated ? Request.CreateResponse(HttpStatusCode.OK) : Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            ///<summary>
            ///get new rentalreservation detail
            ///</summary>
            [HttpGet]
            [Route("api/rentalreservationgroup/view/{rentalreservationgroupid}")]
            public HttpResponseMessage getRentalreservationgroupDetail(int rentalreservationgroupid)
            {
                var db = DAL.GetInstance();
                var tokens = new JObject { { "RentalReservationGroupID", rentalreservationgroupid } };
                var data = RentalReservationGroupManager.View(tokens);
                return data != null ? Request.CreateResponse(HttpStatusCode.OK, data) : Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            ///<summary>
            ///get new rentalreservation detail
            ///</summary>
            [HttpPost]
            [Route("api/rentalreservationgroup/deactivate")]
            public HttpResponseMessage getRentalreservationgroupDetail(ShowHide showhide)
            {
                var db = DAL.GetInstance();
                var updated = RentalReservationGroupManager.Deactivate(showhide);
                return updated ? Request.CreateResponse(HttpStatusCode.OK) : Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            ///<summary>
            ///get new rentalreservation model list
            ///</summary>
            [HttpGet]
            [Route("api/rentalreservationgroup/model/list")]
            public HttpResponseMessage getRentalreservationgroupModelList()
            {
                var db = DAL.GetInstance();
                List<RentalReservationGroupModelList> List = db.RentalReservationGroupModelList();
                var Group = List.GroupBy(r => r.RentalReservationGroup).Select(
                             allgroup => new
                             {
                                 ID = allgroup.FirstOrDefault().RentalReservationGroupID, 
                                 Value = allgroup.Key,
                                 Visible = allgroup.FirstOrDefault().RentalReservationActive,
                                 Makes = allgroup.GroupBy(x => x.ManufacturerName).Where(f=>f.Key != "").Select(formgroup => new
                                    {
                                        ID = formgroup.FirstOrDefault().ManufacturerID,
                                        Value = formgroup.Key,
                                        Visible = true,
                                        Models = formgroup.Select(m => new
                                        {
                                          
                                            ID = m.InventoryMasterID,
                                            Value = m.ModelNum,
                                            Visible = true,
                                        })
                                    })
                             }
                         );
                return Group != null ? Request.CreateResponse(HttpStatusCode.OK, Group) : Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            ///<summary>
            ///get new rentalreservation model by id
            ///</summary>
            [HttpGet]
            [Route("api/rentalreservationgroup/model/{id}")]
            public HttpResponseMessage getRentalreservationgroupModelList(int id)
            {
                var db = DAL.GetInstance();
                var tokens = new JObject { { "RentalReservationGroup", id } };
                List<RentalReservationGroupModelList> List = db.RentalReservationGroupModelList(tokens);
                var Group = List.GroupBy(r => r.RentalReservationGroup).Select(
                             allgroup => new
                             {
                                 ID = allgroup.FirstOrDefault().RentalReservationGroupID,
                                 Value = allgroup.Key,
                                 Visible = allgroup.FirstOrDefault().RentalReservationActive,
                                 Makes = allgroup.GroupBy(x => x.ManufacturerName).Select(formgroup => new
                                 {
                                     ID = formgroup.FirstOrDefault().ManufacturerID,
                                     Value = formgroup.Key,
                                     Visible = true,
                                     Models = formgroup.Select(m => new
                                     {

                                         ID = m.InventoryMasterID,
                                         Value = m.ModelNum,
                                         Visible = true,
                                     })
                                 })
                             }
                         );
                return Group != null ? Request.CreateResponse(HttpStatusCode.OK, Group) : Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            ///<summary>
            ///get model grid for cp admin
            ///</summary>
            [HttpGet]
            [Route("api/model/list")]
            public HttpResponseMessage ModelList()
            {
                var db = DAL.GetInstance();
                var data = db.ModelList();
                return data!=null ? Request.CreateResponse(HttpStatusCode.OK, data) : Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            ///<summary>
            ///get rentalreservationgroup grid for cp admin
            ///</summary>
            [HttpGet]
            [Route("api/rentalreservationgroup/grid/list")]
            public HttpResponseMessage RentalReservationGroupGridList()
            {
                var db = DAL.GetInstance();
                var data = db.RentalReservationGroupGridList();
                return data != null ? Request.CreateResponse(HttpStatusCode.OK, data) : Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            ///<summary>
            ///save category image
            ///</summary>
            [HttpPost]
            [Route("api/category/images/upload")]
            public async Task<HttpResponseMessage> SaveCategoryImageToAzure(List<Files> json)
            {
                var db = DAL.GetInstance();
                var results = await db.getFileURLFromCloud(json, "category", "photos/original/", true, true);
                return Request.CreateResponse(HttpStatusCode.OK, results);
            }


            ///<summary>
            ///create machine category portal
            ///</summary>
            [HttpPost]
            [Route("api/categorymachine/create")]
            public HttpResponseMessage CreateCategoryMachine(ProductCategory json)
            {
                var db = DAL.GetInstance();
                var created = CategoryMachineManager.Create(json);
                return created  ? Request.CreateResponse(HttpStatusCode.Created) : Request.CreateResponse(HttpStatusCode.InternalServerError);

            }


            ///<summary>
            ///create machine category portal
            ///</summary>
            [HttpPost]
            [Route("api/categorymachine/update")]
            public HttpResponseMessage UpdateCategoryMachine(ProductCategory json)
            {
                var db = DAL.GetInstance();
                var update = CategoryMachineManager.Update(json);
                return update ? Request.CreateResponse(HttpStatusCode.OK) : Request.CreateResponse(HttpStatusCode.InternalServerError);

            }
            ///<summary>
            ///create prodcutcategory view detail by id
            ///</summary>
            [HttpGet]
            [Route("api/categorymachine/view/{ProductCategoryID}")]
            public HttpResponseMessage UpdateCategoryMachine(int ProductCategoryID)
            {
                  var db = DAL.GetInstance();
                  var tokens = new JObject { { "ProductCategoryID", ProductCategoryID } };
                  var data = CategoryMachineManager.View(tokens);
                  return data != null ? Request.CreateResponse(HttpStatusCode.OK, data) : Request.CreateResponse(HttpStatusCode.InternalServerError);

            }

            ///<summary>
            ///get category machine grid 
            ///</summary>
            [HttpGet]
            [Route("api/categorymachine/grid/list")]
            public HttpResponseMessage CategoryMachineGridList()
            {
                var db = DAL.GetInstance();
                var data = db.CategoryGridList();
                return data != null ? Request.CreateResponse(HttpStatusCode.OK, data) : Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            ///<summary>
            ///Returns image url, save image to azure cloud storage 
            ///</summary>
            [HttpPost]
            [Route("api/rentalreservationgroup/images/upload")]
            public async Task<HttpResponseMessage> SaverentalreservationgroupImageToAzure(List<Files> json)
            {
                var db = DAL.GetInstance();
                var results = await db.getFileURLFromCloud(json, "rentalreservationgroup", "photos/original/", true, true);
                return Request.CreateResponse(HttpStatusCode.OK, results);
            }

            ///<summary>
            ///Returns file url, save file to azure cloud storage 
            ///</summary>
            [HttpPost]
            [Route("api/rentalreservationgroup/files/upload")]
            public async Task<HttpResponseMessage> SaverentalreservationgroupFileToAzure(List<Files> json)
            {
                var db = DAL.GetInstance();
                var results = await db.getFileURLFromCloud(json, "rentalreservationgroup", "files/", true, false);
                return Request.CreateResponse(HttpStatusCode.OK, results);
            }

        ///<summary>
        ///get category machine grid 
        ///</summary>
        [HttpGet]
        [Route("api/rentalreservation/list")]
        public HttpResponseMessage RentalReservationList()
        {
            var db = DAL.GetInstance();
            var data = db.RentalReservationList();
            return data != null ? Request.CreateResponse(HttpStatusCode.OK, data) : Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

    }
}