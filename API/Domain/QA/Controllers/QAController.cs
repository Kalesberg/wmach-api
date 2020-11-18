using API.Data;
using API.Utilities.Auth;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace API.Controllers
{

    [QAAuthorization]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class QAController : ApiController
    {
        /// <summary>
        /// Delete all customer records for QA i.e Contracts, Quotes, Transportation, Shipping, Rental Billing
        /// </summary>
        /// <param name="companyName"></param>
        /// <returns></returns>
        ///<response code="500">Server Error</response>
        ///<response code="204">No Content</response>
        ///<response code="401">Unauthorized</response>
        ///<response code="404">Not Found</response>
        ///<response code="400">Bad Request</response>
        [HttpDelete]
        [Route("api/QA/DeleteTestRecord")]
        public HttpResponseMessage DeleteQaRecords([FromUri] string companyName)
        {
            try
            {
                if (string.IsNullOrEmpty(companyName))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Parameter cannot be empty");
                }
                var db = DAL.GetInstance();
                var res = db.DeleteQARecords(companyName);
                return res ? Request.CreateResponse(HttpStatusCode.OK, res) : Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            catch (System.Exception ex)
            {

                throw new System.Exception(ex.Message);
            }
            
        }
    }
}