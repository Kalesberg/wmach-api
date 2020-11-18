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
    /// <summary>
    /// Controller for Currency
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [JWTAuthorization]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CurrencyController : ApiController
    {
        /// <summary>
        /// Gets the division default currency.
        /// </summary>
        /// <param name="divisionID">The division identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/currency/{divisionID}/getdivisiondefaultcurrency")]
        public HttpResponseMessage GetDivisionDefaultCurrency(string divisionID)
        {
            if (divisionID == "undefined") return null;
            var tokens = new JObject { { "DivisionID", divisionID } };

            var db = DAL.GetInstance();
            var divisioncurrency = db.getDivisionDefaultCurrency(tokens);
            if (divisioncurrency.Count > 0)
                return Request.CreateResponse(HttpStatusCode.OK, divisioncurrency);
            else
                return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Gets all currency data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/currency/")]
        public HttpResponseMessage GetCurrencies()
        {
            var db = DAL.GetInstance();
            var currencies = db.getCurrencies();

            if (currencies.Count > 0)
                return Request.CreateResponse(HttpStatusCode.OK, currencies);
            else
                return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}