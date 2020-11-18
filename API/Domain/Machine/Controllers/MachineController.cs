using API.Data;
using API.Models;
using API.Utilities.Auth;
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
    [JWTAuthorization]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MachineController : ApiController
    {
        ///<summary>
        ///Returns list of all machines that match search criteria
        ///</summary>
        [HttpPost] //BREAKING REST..TOO MANY KEY/VALUE PAIRS TO SHOVE IN HTTP GET
        [Route("api/machine/search")]
        public HttpResponseMessage GetEquipment(MachineSearch search)
        {
            var json = JObject.FromObject(search);
            var equipments = Builder.Build(new EquipmentSimple(), json);
            return equipments == null ? Request.CreateResponse(HttpStatusCode.NoContent) : Request.CreateResponse(HttpStatusCode.OK, equipments);
        }

        ///<summary>
        ///Returns list of all machines that match search criteria
        ///</summary>
        [HttpPost] //BREAKING REST..TOO MANY KEY/VALUE PAIRS TO SHOVE IN HTTP GET
        [Route("api/public/machine/search")]
        public HttpResponseMessage GetPublicEquipment(MachineSearch search)
        {
            var json = JObject.FromObject(search);
            var equipments = Builder.Build(new EquipmentPub(), json);
            return equipments == null ? Request.CreateResponse(HttpStatusCode.NoContent) : Request.CreateResponse(HttpStatusCode.OK, equipments);
        }

        ///<summary>
        ///Returns equipment info by serial number
        ///</summary>
        [HttpGet]
        [Route("api/machine/")]
        public HttpResponseMessage GetEquipmentBySerialNumUrl([FromUri] string serialNum)
        {
            var tokens = new JObject { { "SerialNum", serialNum } };
            var equipment = Builder.Build(new EquipmentDetail(), tokens);
            if (equipment == null) return Request.CreateResponse(HttpStatusCode.NoContent);
            return Request.CreateResponse(HttpStatusCode.OK, equipment);
        }

        ///<summary>
        ///Returns equipment info by serial number
        ///</summary>
        [HttpGet]
        [Route("api/machine/{serialNum}")]
        public HttpResponseMessage GetEquipmentBySerialNum(string serialNum)
        {
            var tokens = new JObject { { "SerialNum", serialNum } };
            var equipment = Builder.Build(new EquipmentDetail(), tokens);
            if (equipment == null) return Request.CreateResponse(HttpStatusCode.NoContent);
            return Request.CreateResponse(HttpStatusCode.OK, equipment);
        }

        ///<summary>
        ///Returns list of all machines that match search criteria for use in the Admin Grid in M1
        ///</summary>
        [HttpPost]
        [Route("api/machine/adminsearch")]
        public HttpResponseMessage GetEquipmentAdmin(MachineSearch search)
        {
            var json = JObject.FromObject(search);
            var equipments = DAL.GetInstance().getEquipmentAdmin(json, "machine");
            return equipments == null ? Request.CreateResponse(HttpStatusCode.NoContent) : Request.CreateResponse(HttpStatusCode.OK, equipments);
        }

        ///<summary>
        ///Returns current list of machine rental rates
        ///</summary>
        [HttpGet]   
        [Route("api/machine/rates")]
        public HttpResponseMessage GetMachineRates()
        {
            var db = DAL.GetInstance();
            var rates = db.getMachineRates();
            return rates == null ? Request.CreateResponse(HttpStatusCode.NoContent) : Request.CreateResponse(HttpStatusCode.OK, rates);
        }

        ///<summary>
        ///Returns current list of machines that are actively for sale
        ///</summary>
        [HttpGet]
        [Route("api/machine/sale")]
        public HttpResponseMessage GetMachinesForSale()
        {
            var db = DAL.GetInstance();
            var rates = db.getMachinesForSale();
            return rates == null ? Request.CreateResponse(HttpStatusCode.NoContent) : Request.CreateResponse(HttpStatusCode.OK, rates);
        }

        ///<summary>
        ///Returns current list of all machine categories
        ///</summary>
        [HttpGet]
        [Route("api/machine/categories")]
        public HttpResponseMessage GetMachineCategories()
        {
            var db = DAL.GetInstance();
            var categories = db.getEquipmentCategories("machine");
            return Request.CreateResponse(HttpStatusCode.OK, categories);
        }

        ///<summary>
        ///Returns current list of all machine categories
        ///</summary>
        [HttpPost]
        [Route("api/public/machine/categories")]
        public HttpResponseMessage GetMachineCategoriesByModel([FromBody] JObject json)
        {
            var db = DAL.GetInstance();
            var categories = db.getMachineDataByModelNum(json);
            return Request.CreateResponse(HttpStatusCode.OK, categories);
        }

        ///<summary>
        ///Returns list of all applicable machine manufacturers
        ///</summary>
        [HttpPost]
        [Route("api/machine/makes")]
        public HttpResponseMessage GetMachineManufacturers([FromBody] JObject json)
        {
            var db = DAL.GetInstance();
            var makes = db.getEquipmentManufacturers(json, "machine");
            return Request.CreateResponse(HttpStatusCode.OK, makes);
        }

        ///<summary>
        ///Returns list of all applicable machine models
        ///</summary>
        [HttpPost]
        [Route("api/machine/models")]
        public HttpResponseMessage GetMachineModels([FromBody] JObject json)
        {
            var db = DAL.GetInstance();
            var models = db.getEquipmentModels(json, "machine");
            return Request.CreateResponse(HttpStatusCode.OK, models);
        }

        ///<summary>
        ///Returns list of years from 1961 to current year. M1 Standard.
        ///</summary>
        [HttpGet]
        [Route("api/machine/years")]
        public HttpResponseMessage GetAvailableMachineYears()
        {
            var years = Enumerable.Range(1961, DateTime.Now.Year - 1960);
            var strYears = years.Select(x => x.ToString()); //client already expects strings.
            return Request.CreateResponse(HttpStatusCode.OK, strYears);
        }

        [HttpPost]
        [Route("api/machine/machineupdate")]
        public HttpResponseMessage UpdateEquipment(EquipmentLegacy toUpdateEquipment)
        {
            var db = DAL.GetInstance();
            var parameters = JObject.FromObject(toUpdateEquipment);
            var response = db.updateEquipment(parameters);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }


        ///<summary>
        ///Returns list of price intervals for dropdowns. M1 Standard.
        ///</summary>
        [HttpGet]
        [Route("api/machine/prices")]
        public HttpResponseMessage GetAvailableMachinePrices()
        {
            var prices = new List<int>() { 10000, 25000, 50000, 100000, 150000, 200000, 250000, 300000, 350000, 400000, 450000, 500000, 600000, 700000, 800000, 900000, 1000000, 1500000 };
            var strPrices = prices.Select(x => x.ToString()); //client already expects strings.
            return Request.CreateResponse(HttpStatusCode.OK, strPrices);
        }
    }
}