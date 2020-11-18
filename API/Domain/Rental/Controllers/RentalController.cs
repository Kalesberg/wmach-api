
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
using Microsoft.AspNet.OData;
using API.Templates;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class RentalController : ApiController
    {
        ///<summary>
        ///Returns list of all rental category
        ///</summary>
        [HttpGet]
        [Route("api/public/rental/category")]
        public HttpResponseMessage GetRentalCategory()
        {
            var rentalCategories = Builder.Build(new RentalCategoryList());
            return rentalCategories == null ? Request.CreateResponse(HttpStatusCode.NoContent) : Request.CreateResponse(HttpStatusCode.OK, rentalCategories);
        }

        ///<summary>
        ///Returns detail of modelgroup for website
        ///</summary>
        [HttpGet]
        [Route("api/public/rental/modelgroup/{id}")]
        public HttpResponseMessage GetModelGroupDetail(int id)
        {
            var sqlParams = new JObject { { "ModelGroupID", id } };
            var rentalModel = DAL.GetInstance().GetRentalModelGroupDetail(sqlParams);
            if (rentalModel != null)
            {
                var json = new JObject { { "CategoryID", rentalModel.CategoryID } };
                rentalModel.RelatedCategories = DAL.GetInstance().GetRentalModelRelatedCategories(json);
                rentalModel.RentalCategoryModelsSpecs = DAL.GetInstance().GetRentalCategoryModelSpecList(sqlParams);
            }

            return rentalModel == null ? Request.CreateResponse(HttpStatusCode.NoContent) : Request.CreateResponse(HttpStatusCode.OK, rentalModel);
        }

        ///<summary>
        ///save category image
        ///</summary>
        [JWTAuthorization]
        [HttpPost]
        [Route("api/rental/category/images/upload")]
        public async Task<HttpResponseMessage> SaveRentalCartCategoryImageToAzure(List<Files> json)
        {
            var db = DAL.GetInstance();
            var results = await db.getFileURLFromCloud(json, "rentalcartcategory", "photos/original/", true, true);
            return Request.CreateResponse(HttpStatusCode.OK, results);
        }
        ///<summary>
        ///save model image
        ///</summary>
        [JWTAuthorization]
        [HttpPost]
        [Route("api/rental/modelgroup/images/upload")]
        public async Task<HttpResponseMessage> SaveRentalCartModelGroupImageToAzure(List<Files> json)
        {
            var db = DAL.GetInstance();
            var results = await db.getFileURLFromCloud(json, "rentalcartmodelgroup", "photos/original/", true, true);
            return Request.CreateResponse(HttpStatusCode.OK, results);
        }

        ///<summary>
        ///update image record of category/model to save url of image
        ///</summary>
        [JWTAuthorization]
        [HttpPost]
        [Route("api/rental/imageurl/update")]
        public HttpResponseMessage UpdaterentalCartImageUrl(RentalCartImageUpload json)
        {
            var db = DAL.GetInstance();
            var update = false;
            if (json.RentalCartImage.Count > 0)
            {
                foreach (RentalCartImage r in json.RentalCartImage)
                {
                    string url = r.ImageUrl.Replace("original", "medium");
                    var sqlpara = new JObject { { "ImageName", r.ImageName },
                                                  { "ImageUrl", url },
                     { "EditUserStr", r.EditUserStr },};
                    update = url.Contains("rentalcartcategory") ? db.CategoryImageInsertUrl(sqlpara) : db.ModelGroupImageInsertUrl(sqlpara); ;
                }
            }
            else
                return Request.CreateResponse(HttpStatusCode.OK);
            return update ? Request.CreateResponse(HttpStatusCode.OK) : Request.CreateResponse(HttpStatusCode.InternalServerError);

        }

        ///<summary>
        ///get all industry
        ///</summary>
        [HttpGet]
        [Route("api/public/rental/industry")]
        public HttpResponseMessage getAllIndustry()
        {
            var db = DAL.GetInstance();
            var data = db.getAllIndustry();
            return data != null ? Request.CreateResponse(HttpStatusCode.OK, data) : Request.CreateResponse(HttpStatusCode.InternalServerError);
        }
    }
}