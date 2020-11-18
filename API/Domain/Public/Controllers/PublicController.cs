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

namespace API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PublicController : ApiController
    {
        ///<summary>
        ///Returns list of all machines that match search criteria
        ///</summary>
        [HttpGet]
        [EnableQuery()]
        [Route("api/public/machines")]
        public HttpResponseMessage GetPublicEquipment()
        {
            var equipments = Builder.Build(new EquipmentPubWebsite());
            return equipments == null ? Request.CreateResponse(HttpStatusCode.NoContent) : Request.CreateResponse(HttpStatusCode.OK, equipments);
        }

        ///<summary>
        ///Returns current list of all machine categories
        ///</summary>
        [HttpGet]
        [Route("api/public/{site}/filters")]
        public HttpResponseMessage GetMachineCategories(string site)
        {
            var db = DAL.GetInstance();
            var json = new JObject() { { "Website", site } };
            var categories = Builder.Build(new PublicFilters(), json);
            return categories == null ? Request.CreateResponse(HttpStatusCode.NoContent) : Request.CreateResponse(HttpStatusCode.OK, categories);
        }

        ///<summary>
        ///Returns current list of all models of a machine category
        ///</summary>
        [HttpGet]
        [Route("api/public/{site}/filters/{productCategoryID}")]
        public HttpResponseMessage GetMachineCategories(string site, int productCategoryID)
        {
            var db = DAL.GetInstance();
            var json = new JObject() { { "Website", site },
                                       { "ProductCategoryID", productCategoryID}};
            var categories = Builder.Build(new PublicFilters(), json);
            return categories == null ? Request.CreateResponse(HttpStatusCode.NoContent) : Request.CreateResponse(HttpStatusCode.OK, categories);
        }

        ///<summary>
        ///Hides or shows filters based on FilterID
        ///</summary>
        [HttpPost]
        [JWTAuthorization]
        [Route("api/public/{site}/categories/{hideOrShow}")]
        public HttpResponseMessage UpdateCategoryFilterVisibility([FromBody] JObject filterIDs, string site, string hideOrShow)
        {
            try
            {
                DAL.GetInstance().UpdatePublicWesbiteCategoryFilterVisibility(site, hideOrShow, filterIDs);
                return Request.CreateResponse(HttpStatusCode.OK, new JObject(){ {"message", "success"} });
            }

            catch(Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        ///<summary>
        ///Hides or shows filters based on FilterID
        ///</summary>
        [HttpPost]
        [JWTAuthorization]
        [Route("api/public/{site}/categories/{categoryID}/makes/{hideOrShow}")]
        public HttpResponseMessage UpdateCategoryFilterVisibility([FromBody] JObject filterIDs, string site, int categoryID, string hideOrShow)
        {
            try
            {
                DAL.GetInstance().UpdatePublicWesbiteMakeFilterVisibility(site, hideOrShow, categoryID, filterIDs);
                return Request.CreateResponse(HttpStatusCode.OK, new JObject(){ {"message", "success"} });
            }

            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

        }

        ///<summary>
        ///Hides or shows filters based on FilterID
        ///</summary>
        [HttpPost]
        [JWTAuthorization]
        [Route("api/public/{site}/categories/{categoryID}/makes/{makeID}/models/{hideOrShow}")]
        public HttpResponseMessage UpdateCategoryFilterVisibility([FromBody] JObject filterIDs, string site, int categoryID, int makeID, string hideOrShow)
        {
            try
            {
                DAL.GetInstance().UpdatePublicWesbiteModelFilterVisibility(site, hideOrShow, categoryID, makeID, filterIDs);
                return Request.CreateResponse(HttpStatusCode.OK, new JObject() { {"message", "success"} });
            }

            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        ///<summary>
        ///Returns quote detail by quoteHashID
        ///</summary>
        [HttpGet]
        [Route("api/public/quotedetail/{quoteHashID}")]
        public HttpResponseMessage GetQuotesDetailByQuoteIDgag(string quoteHashID)
        {

            var sqlParams = new JObject { { "HashID", quoteHashID } };
            var quote = Builder.Build(new QuoteDetails(), sqlParams);
            return quote == null ? Request.CreateResponse(HttpStatusCode.NoContent) : Request.CreateResponse(HttpStatusCode.OK, quote);
        }

        ///<summary>
        ///Returns active for sales pdf
        ///</summary>
        [HttpPost]
        [Route("api/public/activeForSale")]
        public HttpResponseMessage ActiveForSalePublic(JObject json)
        {
            var db = DAL.GetInstance();
            var activeForSale = json.ToObject<ActiveForSale>();
            activeForSale.ListPrice = true;
            activeForSale.DealerPrice = false;
            activeForSale.SpecialPrice = false;

            var sqlParams = new JObject();
            UserPreferences userPreferences = db.Preferences_GET("WWM\\AGreenberg");
             var _preference = Utilities.XML.GetPreferenceName("ForSaleList.Categories", userPreferences.Preferences);
            activeForSale.Categories= Utilities.XML.Deserialize<List<string>>(_preference.Data);

            sqlParams.Add("Categories", JArray.FromObject(activeForSale.Categories));

            activeForSale.Data = db.getMachinesAndAttachmentsForSale(sqlParams);

            if (activeForSale.IsPreview)
            {
                var preview = activeForSale.GetPreview();
                return Utilities.ResponseMessage.ReturnBytes(preview);
            }

            byte[] pdf = activeForSale.DistributeListPublic();
            var contentLength = pdf.Length;
            //200
            //successful
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(new MemoryStream(pdf));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            response.Content.Headers.ContentLength = contentLength;
            ContentDispositionHeaderValue contentDisposition = null;
            if (ContentDispositionHeaderValue.TryParse("inline; filename=" + "ActiveForSale" + ".pdf", out contentDisposition))
            {
                response.Content.Headers.ContentDisposition = contentDisposition;
            }
            return response;
        }

        ///<summary>
        ///Returns  category by website and distribution make
        ///</summary>
        [HttpGet]
        [Route("api/public/{site}/category/{makeID}")]
        public HttpResponseMessage GetCategoriesBySiteandMake(string site, int makeID)
        {
            var db = DAL.GetInstance();
            var json = new JObject() { { "Website", site },
                                       { "ManufacturerId", makeID}};
            var categories = db.getCategoryBySiteAndMake(json);
            return categories == null ? Request.CreateResponse(HttpStatusCode.NoContent) : Request.CreateResponse(HttpStatusCode.OK, categories);
        }

    }

    
}
