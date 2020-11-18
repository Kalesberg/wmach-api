using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using API.Data;
using Newtonsoft.Json.Linq;
using API.Utilities.Auth;

namespace API.Controllers
{
    [JWTAuthorization]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ServiceController : ApiController
    {
        /// <summary>
        /// Returns service by Service ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Service</returns>
        [HttpGet]
        [Route("api/service/{id}")] //SERVICE ID
        public HttpResponseMessage GetServiceOrderById(string id)
        {
            DAL db = DAL.GetInstance();
            var tokens = new JObject();
            tokens.Add("serviceID", id);
            var svcHistory = db.getService(tokens);
            return Request.CreateResponse(HttpStatusCode.OK, svcHistory);
        }
        
        /// <summary>
        /// Return service by work order number
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/service/workorder/{id}")]
        public HttpResponseMessage GetServiceOrderByWorkOrderNum(string id)
        {
            DAL db = DAL.GetInstance();
            var tokens = new JObject();
            tokens.Add("workOrderNum", id);
            var svcHistory = db.getService(tokens);
            return Request.CreateResponse(HttpStatusCode.OK, svcHistory);
        }

        /// <summary>
        /// Returns list of services by Equipment ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/service/equipment/{id}")] 
        public HttpResponseMessage GetEquipmentServiceHistory(string id)
        {
            if (id == "undefined") return null;
            var db = DAL.GetInstance();
            var tokens = new JObject();
            tokens.Add("equipmentID", id);
            var svcHistory = db.getEquipmentServiceHistory(tokens);
            if (svcHistory == null) return Request.CreateResponse(HttpStatusCode.NoContent);
            return Request.CreateResponse(HttpStatusCode.OK, svcHistory);
        }

        /// <summary>
        /// Returns list of components by Equipment ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/service/component/{id}")]
        public HttpResponseMessage GetEquipmentComponentHistory(string id)
        {
            if (id == "undefined") return null;
            var db = DAL.GetInstance();
            var tokens = new JObject();
            tokens.Add("equipmentID", id);
            var componentHistory = db.getEquipmentComponentHistory(tokens);
            if (componentHistory == null) return Request.CreateResponse(HttpStatusCode.NoContent);
            return Request.CreateResponse(HttpStatusCode.OK, componentHistory);
        }

        /// <summary>
        /// Returns service by contractDtlID
        /// </summary>
        [HttpGet]
        [Route("api/service/contractDtl/{id}")] //SERVICE ID
        public HttpResponseMessage GetServiceOrderByContractDtlId(string id)
        {
            DAL db = DAL.GetInstance();
            var tokens = new JObject();
            tokens.Add("ContractDtlID", id);
            var service = db.getServiceByContractDtlID(tokens);
            return Request.CreateResponse(HttpStatusCode.OK, service);
        }
        /// <summary>
        /// Returns checkout machine list
        /// </summary>
        [HttpGet]
        [Route("api/service/checkout")] 
        public HttpResponseMessage GetCheckoutlist()
        {
            DAL db = DAL.GetInstance();
            var checkout = db.GetCheckoutlist();
            return checkout == null ? Request.CreateResponse(HttpStatusCode.InternalServerError) : Request.CreateResponse(HttpStatusCode.OK, checkout);
        }
    }
}
