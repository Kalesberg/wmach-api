using API.Data;
using API.Managers;
using API.Models;
using API.Utilities;
using API.Utilities.Auth;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace API.Controllers
{
    [JWTAuthorization]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class IssueController : ApiController
    {
        ///<summary>
        ///Returns issue by issue ID
        ///</summary>
        [HttpGet] 
        [Route("api/issues/{issueID}")]
        public HttpResponseMessage GetIssue(string issueID)
        {
            var tokens = new JObject { { "IssueID", issueID } };
            var data = Builder.Build(new IssueDetail(), tokens);
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

        ///<summary>
        ///Returns all issues
        ///</summary>
        [HttpGet]
        [Route("api/issues")]
        public HttpResponseMessage GetAllIssues()
        {
            var data = Builder.Build(new IssueSimple(), null);
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

        ///<summary>
        ///Returns active issues
        ///</summary>
        [HttpGet]
        [Route("api/issues/active")]
        public HttpResponseMessage GetActiveIssues()
        {
            var tokens = new JObject { { "IssueStatus", "Open" } };
            var data = Builder.Build(new IssueSimple(), tokens);
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

        ///<summary>
        ///Returns resolved issues
        ///</summary>
        [HttpGet]
        [Route("api/issues/resolved")]
        public HttpResponseMessage GetResolvedIssues()
        {
            var tokens = new JObject { { "IssueStatus", "Completed" } };
            var data = Builder.Build(new IssueSimple(), tokens);
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

        ///<summary>
        ///Creates new issue
        ///</summary>
        [HttpPost]
        [Route("api/issues")]
        public async Task<HttpResponseMessage> CreateIssue([FromBody] Issue issue)
        {
            try
            {
                var token = Request.Headers.Authorization.Parameter;
                var data = await IssueManager.CreateIssue(issue, token);
                return data != null ? Request.CreateResponse(HttpStatusCode.OK, data) : Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            catch(Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e);
            }

        }

        ///<summary>
        ///updates issue
        ///</summary>
        [HttpPost]
        [Route("api/issues/{issueID}/update")]
        public async Task<HttpResponseMessage> UpdateIssue([FromBody] Issue issue)
        {
            var token = Request.Headers.Authorization.Parameter;
            var data = await IssueManager.UpdateIssue(issue, token);
            return data != null ? Request.CreateResponse(HttpStatusCode.OK, data) : Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        ///<summary>
        ///Send issue as Email
        ///</summary>
        [HttpPost]
        [Route("api/issues/{issueID}/sendEmail")]
        public async Task<HttpResponseMessage> SendEmail([FromBody] Issue issue)
        {
            var token = Request.Headers.Authorization.Parameter;
            var data = await IssueManager.SendEmail(issue, token);
            return data != null ? Request.CreateResponse(HttpStatusCode.OK, data) : Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        ///<summary>
        ///Updates existing issue with a new comment
        ///</summary>
        [HttpPost]
        [Route("api/issues/{issueID}/comment")]
        public async Task<HttpResponseMessage> CreateIssueComment([FromBody] Comment comment, string issueID)
        {    
            //GRAB USERNAME FROM JWT AND POPULATE MISSING DATA ON COMMENT OBJECT. DO THIS SERVER SIDE TO VERIFY THE AUTHENTICITY OF THE JWT FIRST
            var userName = Authentication.GetUserName(Request.Headers.Authorization.Parameter);
            comment.UserName = userName;
            comment.IssueID = Int32.Parse(issueID);
            
            //RETURNS CREATED COMMENT ON SUCCESS
            var token = Request.Headers.Authorization.Parameter;
            var createdComment = await IssueManager.CreateIssueComments(comment, token);
            return createdComment != null ? Request.CreateResponse(HttpStatusCode.OK, createdComment) : Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        ///<summary>
        ///Updates existing issue with a new screenshot
        ///</summary>
        [HttpPost]
        [Route("api/issues/{issueID}/photo")]
        public HttpResponseMessage CreateIssueImage([FromBody] JObject data, string issueID)
        {
            var byteArr = IssueManager.ConvertImagesToByteArrays(data["data"].ToString());
            var photoIDs = IssueManager.CreateIssuePhotos(byteArr);
            var userName = Authentication.GetUserName(Request.Headers.Authorization.Parameter);

            //POPULATE JARRAY OF PHOTOID. DB METHOD TAKES ARRAY BUT WE ONLY HAVE ONE PHOTOID HERE
            var array = new JArray();
            photoIDs.ForEach(photoID =>
            {
                array.Add(photoID);
            });

            //POPULATE SQL PARAMS
            var sqlParams = new JObject();
            sqlParams.Add("PhotoIDs", array);
            sqlParams.Add("User", userName);
            sqlParams.Add("IssueID", issueID);

            //MAKE DB CALLS
            var db = DAL.GetInstance(DB.IssueTrack);
            db.CreateIssueTrackPicture(sqlParams);

            //CREATE IMAGE OBJECT TO RETURN TO CALLER
            var img = new IssueImage();
            img.FileName = photoIDs.First();
            img.IssueID = Int32.Parse(issueID);
            return Request.CreateResponse(HttpStatusCode.OK, img);
        }

        ///<summary>
        ///Deactivates existing image on issue
        ///</summary>
        [HttpGet]
        [Route("api/issues/{issueID}/photo/remove/{fileName}")]
        public HttpResponseMessage CreateIssueImage(string issueID, string fileName)
        {
            //BUILD PARAM COLLECTION
            var sqlParams = new JObject();
            sqlParams.Add("FileName", fileName);
            sqlParams.Add("IssueID", issueID);

            //MAKE DB CALLS
            var db = DAL.GetInstance(DB.IssueTrack);
            var deactivated = db.DeactivateIssueTrackPicture(sqlParams);

            //POPULATE RETURN OBJECT
            var img = new IssueImage();
            img.IssueID = Int32.Parse(issueID);
            img.FileName = fileName;

            return deactivated ? Request.CreateResponse(HttpStatusCode.OK, img) : Request.CreateResponse(HttpStatusCode.InternalServerError);
        }
    }
}