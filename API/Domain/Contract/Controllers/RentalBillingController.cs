using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API.Data;
using API.Utilities.Auth;
using Newtonsoft.Json.Linq;

namespace API.Controllers
{
    [JWTAuthorization]
    public class RentalBillingController : ApiController
    {
        //[HttpGet]
        //[Route("api/equipment/{id}")]
        //public HttpResponseMessage GetEquipmentByContractID(string id)
        //{
        //     var tokens = new JObject { { "EquipmentID", id } };
        //     //var equipments = DAL.GetInstance().getContractQuoteSummary(tokens);
        //     var equipments = DAL.GetInstance().getContractDetails(tokens);
        //    if (equipments == null)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.NoContent);
        //    } return Request.CreateResponse(HttpStatusCode.OK, equipments);
        //}
    }
}
