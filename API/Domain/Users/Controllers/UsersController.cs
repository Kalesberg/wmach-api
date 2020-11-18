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

namespace API.Controllers
{
    [JWTAuthorization]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UsersController : ApiController
    {
        string _domain = @"WWM\";

        #region Preferences

        [HttpGet]
        [Route("api/users/{username}/preferences")]
        public HttpResponseMessage Preferences_GET(string username)
        {
            var db = DAL.GetInstance();
            var preferences = db.Preferences_GET(_domain + username);

            return Request.CreateResponse(HttpStatusCode.OK, preferences);
        }

        [HttpPost]
        [Route("api/users/{username}/preferences")]
        public HttpResponseMessage Preferences_POST(string username, JObject json)
        {
            var db = DAL.GetInstance();
            var id = db.Preferences_POST(_domain + username, json);

            return Request.CreateResponse(HttpStatusCode.OK, id);
        }

        [HttpPost]
        [Route("api/users/{username}/preferences/{id}")]
        public HttpResponseMessage Preferences_PUT(string username, string id, JObject json)
        {
            var db = DAL.GetInstance();
            db.Preferences_PUT(_domain + username, id, json);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
        
        [HttpDelete]
        [Route("api/users/{username}/preferences/{id}")]
        public HttpResponseMessage Preferences_DELETE(string username, string id)
        {
            var db = DAL.GetInstance();
            db.Preferences_DELETE(_domain + username, id);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        #endregion

        #region Mobile Preferences
        [HttpPost]
        [Route("api/preferences/newpreference")]
        public HttpResponseMessage NewPreference([FromBody]JObject json)
        {
            var db = DAL.GetInstance();
            string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(json["JsonString"]);
            json["JsonString"] = jsonString;
            var PreferenceID = db.CreateNewPreference(json);
            return PreferenceID != 0 ? Request.CreateResponse(HttpStatusCode.OK, PreferenceID) : Request.CreateResponse(HttpStatusCode.InternalServerError);

        }

        [HttpPost]
        [Route("api/preferences/getpreference")]
        public HttpResponseMessage GetPreferenceByContactIDAndPreferenceType([FromBody]JObject json)
        {
            var db = DAL.GetInstance();
            var PreferenceInfo = db.GetPreferenceInfo(json);

            var result = new List<dynamic>();
            foreach(var Preference in PreferenceInfo)
            {
                var JonString = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(Preference.JsonString);
                Preference.JsonString = JonString;
            }
            return PreferenceInfo != null ? Request.CreateResponse(HttpStatusCode.OK, PreferenceInfo) : Request.CreateResponse(HttpStatusCode.InternalServerError);

        }

        [HttpPost]
        [Route("api/preferences/deactivepreference")]
        public HttpResponseMessage DeactivePreference([FromBody]JObject json)
        {
            var db = DAL.GetInstance();
            var PreferenceDeavativeResult = db.DeactivePreference(json);
            return PreferenceDeavativeResult ? Request.CreateResponse(HttpStatusCode.OK, PreferenceDeavativeResult) : Request.CreateResponse(HttpStatusCode.InternalServerError);

        }
        [HttpPost]
        [Route("api/preferences/updatepreference")]
        public HttpResponseMessage UpdatePreference([FromBody]JObject json)
        {
            var db = DAL.GetInstance();
            string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(json["JsonString"]);
            json["JsonString"] = jsonString;
            var PreferenceEditResult = db.EditPreference(json);
            return PreferenceEditResult ? Request.CreateResponse(HttpStatusCode.OK, PreferenceEditResult) : Request.CreateResponse(HttpStatusCode.InternalServerError);

        }
        [HttpPost]
        [Route("api/preferences/defaultpreference")]
        public HttpResponseMessage MakeDefaultPreference([FromBody]JObject json)
        {
            var db = DAL.GetInstance();
            var PreferenceDefaultResult = db.MakeDefaultPreference(json);
            return PreferenceDefaultResult ? Request.CreateResponse(HttpStatusCode.OK, PreferenceDefaultResult) : Request.CreateResponse(HttpStatusCode.InternalServerError);

        }

        [HttpGet]
        [Route("api/emailgroup/list")]
        public HttpResponseMessage GetEmailGroupList()
        {
            var db = DAL.GetInstance();
            var emailGroupResult = db.GetEmailGroupList();
            if(emailGroupResult != null)
            {
                var emailGroup = emailGroupResult.GroupBy(r => r.GroupOwnerName).Select(
                        allgroup => new{
                            groupOwnerName = allgroup.Key,
                            groupCount = allgroup.GroupBy(x => x.EmailGroupName).Count(),
                            emailGroup =
                               allgroup.GroupBy(x => x.EmailGroupName).Select(formgroup => new
                               {
                                   groupName = formgroup.Key,
                                   groupEmail = formgroup.Select(x => x.Email),
                                   groupEmailCount = formgroup.Count()
                               })
                        }
                    );


                return Request.CreateResponse(HttpStatusCode.OK, emailGroup);
            }
            else
                return Request.CreateResponse(HttpStatusCode.InternalServerError);

        }
            

        #endregion
    }
}
