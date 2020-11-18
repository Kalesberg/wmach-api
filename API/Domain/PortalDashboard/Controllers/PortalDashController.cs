using API.Data;
using API.Models;
using API.Utilities.Auth;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace API.Controllers
{
    [JWTAuthorization]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PortalDashController : ApiController
    {
        /// <summary>
        /// Get Portal Dashboard Metrics
        /// </summary>
        /// <param name="jobject">Token</param>
        /// <returns></returns>
        ///<response code="500">Server Error</response>
        ///<response code="200">Success</response>
        ///<response code="204">No Content</response>
        ///<response code="401">Unauthorized</response>
        [HttpGet]
        [ResponseType(typeof(PortalMetrics))]
        [Route("api/portaldash/metrics")]
        public HttpResponseMessage GetPortalMetrics(JObject jobject)
        {
            try
            {
                var db = DAL.GetInstance();
                var pu = db.GetPortalMetrics(jobject);
                return pu == null ? Request.CreateResponse(HttpStatusCode.NoContent) : Request.CreateResponse(HttpStatusCode.OK, pu);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Get all the latest Quote
        /// </summary>
        /// <param name="jObject">Token</param>
        /// <returns></returns>
        ///<response code="500">Server Error</response>
        ///<response code="200">Success</response>
        ///<response code="204">No Content</response>
        ///<response code="401">Unauthorized</response>
        [HttpGet]
        [ResponseType(typeof(LatestQuotes))]
        [Route("api/portaldash/latestquotes")]
        public HttpResponseMessage GetLatestQuotes(JObject jObject)
        {
            try
            {
                var db = DAL.GetInstance();
                var result = db.GetLatestQuotes(jObject);
                return result == null ? Request.CreateResponse(HttpStatusCode.NoContent) : Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Get all the portal requests
        /// </summary>
        /// <param name="jObject">Token</param>
        /// <returns></returns>
        ///<response code="500">Server Error</response>
        ///<response code="200">Success</response>
        ///<response code="204">No Content</response>
        ///<response code="401">Unauthorized</response>
        [HttpGet]
        [ResponseType(typeof(PortalAccessRequest))]
        [Route("api/portaldash/portalrequests")]
        public HttpResponseMessage GetPortalAccessRequests(JObject jObject)
        {
            try
            {
                var db = DAL.GetInstance();
                var result = db.GetPortalAccessRequests(jObject);
                return result == null ? Request.CreateResponse(HttpStatusCode.NoContent) : Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Get list of transportation Request
        /// </summary>
        /// <param name="jObject">Token</param>
        /// <returns></returns>
        ///<response code="500">Server Error</response>
        ///<response code="200">Success</response>
        ///<response code="204">No Content</response>
        ///<response code="401">Unauthorized</response>
        [HttpGet]
        [ResponseType(typeof(TransportationRequests))]
        [Route("api/portaldash/transportrequests")]
        public HttpResponseMessage GetTransportationRequests(JObject jObject)
        {
            try
            {
                var db = DAL.GetInstance();
                var result = db.GetTransportRequest(jObject);
                return result == null ? Request.CreateResponse(HttpStatusCode.NoContent) : Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Get a list of all outstanding invoices
        /// </summary>
        /// <param name="jObject"></param>
        /// <returns></returns>
        ///<response code="500">Server Error</response>
        ///<response code="200">Success</response>
        ///<response code="204">No Content</response>
        ///<response code="401">Unauthorized</response>
        [HttpGet]
        [ResponseType(typeof(OverdueInvoices))]
        [Route("api/portaldash/outstandinginvoices")]
        public HttpResponseMessage GetOutstandingInvoices(JObject jObject)
        {
            try
            {
                var db = DAL.GetInstance();
                var res = db.GetOutInvoice(jObject);
                return res == null ? Request.CreateResponse(HttpStatusCode.NoContent) : Request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        ///<summary>
        ///Get the sequence reports for accounting
        ///</summary>
        ///<param name="jObject"></param>
        ///<returns>A data list</returns>
        ///<response code="500">Server Error</response>
        ///<response code="200">Success</response>
        ///<response code="204">No Content</response>
        ///<response code="401">Unauthorized</response>
        [HttpGet]
        [ResponseType(typeof(SequenceTracking))]
        [Route("api/portaldash/report/sequence/")]
        public HttpResponseMessage GetSequenceReport(JObject jObject)
        {
            try
            {
                var db = DAL.GetInstance();
                var res = db.GetSequenceTrackingReport(jObject);
                return res == null ? Request.CreateResponse(HttpStatusCode.InternalServerError) : Request.CreateResponse(HttpStatusCode.OK, res);
                //return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        ///<summary>
        ///Get the sequence reports for accounting with a specified limit
        ///</summary>
        /// <param name="limit"></param>
        ///<returns>A data list</returns>
        ///<response code="500">Server Error</response>
        ///<response code="200">Success</response>
        ///<response code="204">No Content</response>
        ///<response code="401">Unauthorized</response>
        [HttpGet]
        [ResponseType(typeof(SequenceTracking))]
        [Route("api/portaldash/report/sequence/{limit}")]
        public HttpResponseMessage GetSequenceReport(int limit)
        {
            try
            {
                var db = DAL.GetInstance();
                var res = db.GetSequenceTrackingReport(limit);
                return res == null ? Request.CreateResponse(HttpStatusCode.InternalServerError) : Request.CreateResponse(HttpStatusCode.OK, res);
                //return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        ///<summary>
        ///Get the sequence reports for accounting with a specified limit
        ///</summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        ///<returns>A data list</returns>
        ///<response code="500">Server Error</response>
        ///<response code="200">Success</response>
        ///<response code="204">No Content</response>
        ///<response code="401">Unauthorized</response>
        [HttpGet]
        [ResponseType(typeof(SequenceTracking))]
        [Route("api/portaldash/report/sequence/{dateFrom}/{dateTo}")]
        public HttpResponseMessage GetSequenceReport(DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                var db = DAL.GetInstance();
                var res = db.GetSequenceTrackingReport(dateFrom, dateTo);
                return res == null ? Request.CreateResponse(HttpStatusCode.InternalServerError) : Request.CreateResponse(HttpStatusCode.OK, res);
                //return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Get a list of all pending contract
        /// </summary>
        ///<response code="500">Server Error</response>
        ///<response code="200">Success</response>
        ///<response code="204">No Content</response>
        ///<response code="401">Unauthorized</response>
        [HttpGet]
        [ResponseType(typeof(PendinContractDetails))]
        [Route("api/portaldash/contract")]
        public HttpResponseMessage GetPendingContracts([FromUri] int offset = 0, int limit = 1000, string status = "Open", string contractNum = null, string Division = null)
        {
            try
            {
                var db = DAL.GetInstance();
                var res = db.GetAllPendingContracts(offset, limit, status.ToLower(), contractNum, Division);
                return res.Count == 0 ? Request.CreateResponse(HttpStatusCode.NoContent) : Request.CreateResponse(HttpStatusCode.OK, res);

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Gets the contract by Contract ID
        /// </summary>
        /// <param name="contractID"></param>
        /// <returns>The contract details</returns>
        ///<response code="500">Server Error</response>
        ///<response code="200">Success</response>
        ///<response code="204">No Content</response>
        ///<response code="401">Unauthorized</response>
        [HttpGet]
        [ResponseType(typeof(ContractDetail))]
        [Route("api/portaldash/contract/{contractID}")]
        public HttpResponseMessage GetContractDetails(int contractID)
        {
            try
            {
                var sqlParams = new JObject { { "contractID", contractID } };
                var contract = Builder.Build(new ContractDetail(), sqlParams);
                return contract == null ? Request.CreateResponse(HttpStatusCode.NoContent) : Request.CreateResponse(HttpStatusCode.OK, contract);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// Get a list of all the quotes for internal users
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <param name="status"></param>
        /// <param name="Quotenum"></param>
        /// <param name="Division"></param>
        /// <returns>A list of Quotes for internal users</returns>
        ///<response code="500">Server Error</response>
        ///<response code="200">Success</response>
        ///<response code="204">No Content</response>
        ///<response code="401">Unauthorized</response>
        [HttpGet]
        [ResponseType(typeof(QuoteDetailsPortal))]
        [Route("api/portaldash/quote")]
        public HttpResponseMessage GetAllQuotes([FromUri] int offset = 0, int limit = 1000, string status = "Pending", string Quotenum = null, string Division = null)
        {
            try
            {
                var db = DAL.GetInstance();
                var res = db.GetAllQuotes(offset, limit, status.ToLower(), Quotenum, Division);
                return res.Count == 0 ? Request.CreateResponse(HttpStatusCode.NoContent) : Request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Gets the quote by quote ID
        /// </summary>
        /// <param name="quoteId"></param>
        /// <returns>The quote details</returns>
        ///<response code="500">Server Error</response>
        ///<response code="200">Success</response>
        ///<response code="204">No Content</response>
        ///<response code="401">Unauthorized</response>
        [HttpGet]
        [ResponseType(typeof(QuoteDetails))]
        [Route("api/portaldash/quote/{quoteID}")]
        public HttpResponseMessage GetQuoteDetails(int quoteId)
        {
            try
            {
                var sqlParams = new JObject { { "QuoteID", quoteId } };
                var quote = Builder.Build(new QuoteDetails(), sqlParams);
                return quote == null ? Request.CreateResponse(HttpStatusCode.NoContent) : Request.CreateResponse(HttpStatusCode.OK, quote);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
