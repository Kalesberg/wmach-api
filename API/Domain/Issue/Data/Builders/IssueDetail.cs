using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models;
using Newtonsoft.Json.Linq;
using System.Data;

namespace API.Data
{
    public class IssueDetail : IBuildParams<Issue>
    {
        public Issue _issue;
        public void Build(JObject sqlParams)
        {
            _issue = DAL.GetInstance(DB.IssueTrack).GetIssue(sqlParams);

            GetCreatedBy();
            GetImageFileNames(sqlParams);
            GetComments(sqlParams);
        }

        public Issue GetResult()
        {
            return _issue;
        }

        private void GetCreatedBy()
        {
            _issue.CreatedBy = DAL.GetInstance(DB.IssueTrack).GetIssueTrackUserByUserName(_issue.UserName);
        }

        private void GetImageFileNames(JObject sqlParams)
        {
            var data = DAL.GetInstance(DB.IssueTrack).GetIssueImagesByIssueID(sqlParams);
            var images = data.AsEnumerable().Select(r => r["Path"].ToString()).ToList();
            _issue.Images = images;
        }

        private void GetComments(JObject sqlParams)
        {
            _issue.Comments = DAL.GetInstance(DB.IssueTrack).GetIssueCommentsByIssueID(sqlParams);

            foreach (var cmt in _issue.Comments)
            {
                cmt.CreatedBy = DAL.GetInstance(DB.IssueTrack).GetIssueTrackUserByUserName(cmt.UserName);
            }
        }
    }
}