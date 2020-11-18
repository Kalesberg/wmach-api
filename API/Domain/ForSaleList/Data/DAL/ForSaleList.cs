using API.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace API.Data
{
    public partial class DAL
    {
        public IEnumerable<ForSaleList> getForSaleListMobile(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["ForSaleListMobile"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<ForSaleList>(cmdText, sqlParams);
        }

        public List<string> getForSaleListMobileProductCategory()
        {
            string cmdText = ConfigurationManager.AppSettings["ForSaleListMobileProductCategory"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<string>(cmdText);
        }

        public IEnumerable<ForSaleListData> ForsaleList_UpdatePreference(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["ForsaleList_Update"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<ForSaleListData>(cmdText, sqlParams);
        }

        public IEnumerable<ForSaleList> getForSaleListMobile_Price(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["ForSaleListMobile_Price"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            cmd.Parameters.AddWithValue("@isDealerPrice", true);
            cmd.Parameters.AddWithValue("@isListPrice", true);
            //cmd.Parameters.AddWithValue("@Categories", null);
            var data = getRecords<ForSaleList>(cmdText, sqlParams);

            //getPictureFileNames(data);
            //getAttachments(data);

            return data;
        }
    }
}