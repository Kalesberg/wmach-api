using API.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace API.Data
{
    public partial class DAL
    {
        public Issue GetIssue(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["GetIssueByID"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<Issue>(cmdText, sqlParams).FirstOrDefault();
        }

        public IEnumerable<Issue> GetIssues(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["GetIssues"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<Issue>(cmdText, sqlParams);
        }

        public int CreateIssue(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["CreateIssue"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return InsertRecord(cmdText, sqlParams);
        }

        public int UpdateIssue(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["UpdateIssue"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return InsertRecord(cmdText, sqlParams);
        }

        public int SendIssueTrackMessage(JObject sqlparams)
        {
            string cmdText = ConfigurationManager.AppSettings["SendMessage"];
            if (string.IsNullOrWhiteSpace(cmdText)) return 0;
            return InsertRecord(cmdText, sqlparams);
        }

        public DataTable GetIssueImagesByLookupIDs(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["GetIssueImagesByLookupIDs"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords(cmdText, sqlParams);
        }

        public DataTable GetIssueImagesByIssueID(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["GetIssueImagesByIssueID"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords(cmdText, sqlParams);
        }

        public DataTable GetIssueCommentsByLookupIDs(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["GetIssueCommentsByLookupIDs"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords(cmdText, sqlParams);
        }

        public List<Comment> GetIssueCommentsByIssueID(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["GetIssueCommentsByIssueID"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<Comment>(cmdText, sqlParams);
        }

        public Contact GetIssueTrackUserByUserName(string userName)
        {
            var sqlParams = new JObject { { "UserName", userName } };
            string cmdText = ConfigurationManager.AppSettings["GetUserByUserName"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<Contact>(cmdText, sqlParams).FirstOrDefault();
        }

        public bool IsIssueTrackUser(string userName)
        {
            var sqlParams = new JObject { { "UserName", userName } };
            string cmdText = ConfigurationManager.AppSettings["VerifyIssueTrackUser"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return ExecuteBooleanScalar(cmdText, userName);
        }

        public bool IsUserDeveloper(string userName)
        {
            var sqlParams = new JObject { { "UserName", userName } };
            string cmdText = ConfigurationManager.AppSettings["IsUserDeveloper"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return ExecuteBooleanScalar(cmdText, userName);
        }

        public int CreateIssueTrackComment(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["CreateIssueComment"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return InsertRecord(cmdText, sqlParams);
        }

        public int CreateIssueTrackUser(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["CreateIssueTrackUser"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return InsertRecord(cmdText, sqlParams);
        }

        public int CreateIssueTrackPicture(JObject tokens)
        {
            string cmdText = ConfigurationManager.AppSettings["CreateIssueImages"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return InsertRecord(cmdText, tokens);
        }

        public bool DeactivateIssueTrackPicture(JObject tokens)
        {
            string cmdText = ConfigurationManager.AppSettings["DeactivateIssueImage"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecord(cmdText, tokens);
        }
    }
}