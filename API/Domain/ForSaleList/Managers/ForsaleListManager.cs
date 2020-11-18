using API.Data;
using API.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace API.Domain.ForSaleList.Managers
{
    public class ForsaleListManager
    {
        public static ForSaleListData UpdatePreferences([FromBody] JObject json)
        {
            var db = DAL.GetInstance();
            var jobject = json.ToObject<ForSaleListData>();
            var userName = json["username"].ToString();
            var textdata = json["TextData"].ToString();
            var sqlparams = new JObject();

            sqlparams.Add("Username", userName);
            sqlparams.Add("TextData", textdata);
            if (userName != null && userName == "agreenberg")
            {
                var data = db.ForsaleList_UpdatePreference(sqlparams);
                return jobject;
            }
            else
                return null;
        }
    }
}