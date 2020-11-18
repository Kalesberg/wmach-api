using API.Data;
using API.Templates;
using API.Utilities.Auth;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Cors;

namespace API.Controllers
{
    [JWTAuthorization]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class NotificationController : ApiController
    {
        [HttpPost]
        [Route("api/notifications/activeForSale")]
        public async Task<HttpResponseMessage> ActiveForSale(JObject json)
        {
            var db = DAL.GetInstance();
            var activeForSale = json.ToObject<ActiveForSale>();
            
            var sqlParams = new JObject();
            sqlParams.Add("Categories", JArray.FromObject(activeForSale.Categories));

            activeForSale.Data = db.getMachinesAndAttachmentsForSale(sqlParams);
            
            if (activeForSale.IsPreview)
            {
                var preview = activeForSale.GetPreview();
                return Utilities.ResponseMessage.ReturnBytes(preview);
            }

            await activeForSale.DistributeList();

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
