using API.Data;
using API.Utilities.Auth;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using API.Utilities;
using API.Models;
using API.Data.Builders;
using API.Managers;

namespace API.Controllers
{
    [JWTAuthorization]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class FinancialController : ApiController
    {

        #region DIVISION FINANCIALS
        /// <summary>
        /// Returns Rental Revenue by Division
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/financials/revenue/rental/divisions")]
        public HttpResponseMessage GetRentalRevenueByDivision([FromBody] DateSelector req)
        {
            var json = JObject.FromObject(req);
            var data = Builder.Build(new RentalRevenueByDivisions(), json);
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Returns Sales Revenue by Division
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/financials/revenue/sales/divisions")]
        public HttpResponseMessage GetSalesRevenueByDivision([FromBody] DateSelector req)
        {
            var json = JObject.FromObject(req);
            var res = Builder.Build(new SalesRevenueByDivisions(), json);
            return Request.CreateResponse(HttpStatusCode.OK, res);
        }

        /// <summary>
        /// Returns Total (Rental and Sales) Revenue by Division
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/financials/revenue/all/divisions")]
        public HttpResponseMessage GetTotalRevenueByDivision([FromBody] DateSelector req)
        {
            var json = JObject.FromObject(req);
            var res = Builder.Build(new TotalRevenueByDivisions(), json);
            return Request.CreateResponse(HttpStatusCode.OK, res);
        }

        #endregion

        #region SALESPERSON FINANCIALS

        /// <summary>
        /// Returns Rental Revenue by Salesperson
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/financials/revenue/rental/salesperson")]
        public HttpResponseMessage GetRentalRevenueBySalesPerson([FromBody] DateSelector req)
        {
            var json = JObject.FromObject(req);
            var res = Builder.Build(new RentalRevenueBySalesperson(), json);
            return Request.CreateResponse(HttpStatusCode.OK, res);
        }

        /// <summary>
        /// Returns Sales Revenue by Salesperson
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/financials/revenue/sales/salesperson")]
        public HttpResponseMessage GetSalesRevenueBySalesPerson([FromBody] DateSelector req)
        {
            var json = JObject.FromObject(req);
            var res = Builder.Build(new SalesRevenueBySalesperson(), json);
            return Request.CreateResponse(HttpStatusCode.OK, res);
        }

        /// <summary>
        /// Returns Total (Rental and Sales) Revenue by Salesperson
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/financials/revenue/all/salesperson")]
        public HttpResponseMessage GetTotalRevenueBySalesPerson([FromBody] DateSelector req)
        {
            var json = JObject.FromObject(req);
            var res = Builder.Build(new TotalRevenueBySalesperson(), json);
            return Request.CreateResponse(HttpStatusCode.OK, res);
        }

        #endregion

        #region TIME SERIES FINANCIAL BY SPECIFIC ENTITY (DIVISION, SALESPERSON)

        /// <summary>
        /// Returns Total (Rental and Sales) Revenue by given Entity
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/financials/revenue/all")]
        public HttpResponseMessage GetTotalRevenueBySingleDivision([FromBody] EntityRevenueRequest req)
        {
            var json = new JObject { { "Filter", req.Filter }, { "StartDate", req.StartDate }, { "EndDate", req.EndDate } };
            var res = FinancialManager.GetTotalRevenue(req.Entity, json);
            return Request.CreateResponse(HttpStatusCode.OK, res);
        }

        /// <summary>
        /// Returns Total Rental Revenue by given Entity
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/financials/revenue/rental")]
        public HttpResponseMessage GetRentalRevenueBySingleDivision([FromBody] EntityRevenueRequest req)
        {
            var json = new JObject { { "Filter", req.Filter }, { "StartDate", req.StartDate }, { "EndDate", req.EndDate } };
            var res = FinancialManager.GetRentalRevenue(req.Entity, json);
            return Request.CreateResponse(HttpStatusCode.OK, res);
        }

        /// <summary>
        /// Returns Total Sales Revenue by given Entity
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/financials/revenue/sales")]
        public HttpResponseMessage GetSalesRevenueBySingleDivision([FromBody] EntityRevenueRequest req)
        {
            var json = new JObject { { "Filter", req.Filter }, { "StartDate", req.StartDate }, { "EndDate", req.EndDate } };
            var res = FinancialManager.GetSalesRevenue(req.Entity, json);
            return Request.CreateResponse(HttpStatusCode.OK, res);
        }

        #endregion

    }
}