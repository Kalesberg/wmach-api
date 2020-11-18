using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models;
using Newtonsoft.Json.Linq;
using System.Data;

namespace API.Data
{
    public class IssueSimple : IBuildParams<IEnumerable<Issue>>
    {
        public IEnumerable<Issue> _issues;
        public void Build(JObject sqlParams = null)
        {
            _issues = DAL.GetInstance(DB.IssueTrack).GetIssues(sqlParams);

            var array = new JArray();
            foreach (var issue in _issues)
                array.Add(issue.ID);

            var lookupIDs = new JObject { { "LookupIDs", array } };

            GetCreatedBy();
            GetImageFileNames(lookupIDs);
            GetComments(lookupIDs);
        }

        public IEnumerable<Issue> GetResult()
        {
            return _issues;
        }

        private void GetCreatedBy()
        {
            foreach (var issue in _issues)
            {
                issue.CreatedBy = DAL.GetInstance(DB.IssueTrack).GetIssueTrackUserByUserName(issue.UserName);
            }         
        }

        private void GetImageFileNames(JObject sqlParams)
        {
            var data = DAL.GetInstance(DB.IssueTrack).GetIssueImagesByLookupIDs(sqlParams);
            var images = data.AsEnumerable().GroupBy(r => 
                                            Int32.Parse(r["IssueID"].ToString()),
                                            r => r["Path"].ToString()
                                            ).ToDictionary(d => d.Key, d => d.ToList());

            foreach (var issue in _issues)
            {
                var exists = images.ContainsKey(issue.ID);
                if (exists) issue.Images = images[issue.ID];
                else issue.Images = new List<string>();
            }
        }

        private void GetComments(JObject sqlParams)
        {
            var data = DAL.GetInstance(DB.IssueTrack).GetIssueCommentsByLookupIDs(sqlParams);
            var comments = data.AsEnumerable().GroupBy(r => Int32.Parse(r["IssueID"].ToString()),
                                                   r => new Comment
                                                  {
                                                      ID = Int32.Parse(r["ID"].ToString()),
                                                      Text = r["CommentText"].ToString(),
                                                      UserName = r["CreatedBy"].ToString(),
                                                      Created = DateTime.Parse(r["Created"].ToString())
                                                  }).ToDictionary(d => d.Key, d => d.ToList());


            foreach (var issue in _issues)
            {
                var exists = comments.ContainsKey(issue.ID);
                if (exists) issue.Comments = comments[issue.ID];
                else issue.Comments = new List<Comment>();

                foreach (var cmt in issue.Comments)
                {
                    cmt.CreatedBy = DAL.GetInstance(DB.IssueTrack).GetIssueTrackUserByUserName(cmt.UserName);
                }
            }

        }
    }
}