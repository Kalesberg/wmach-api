using API.Data;
using API.Models;
using API.Utilities;
using API.Utilities.Auth;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace API.Managers
{
    public static class JobInfoSheetManager
    {
        public static bool Save(JObject token)
        {
            var db = DAL.GetInstance();
            try
            {
                db.BeginSqlTranscation();
                bool updated = db.JobInfoSheetSave(token);
                db.CommitSqlTranscation();
                return updated;
            }
            catch (Exception e)
            {
                db.RollBackSqlTranscation();
                return false;
            }
        }
    }
}