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
        public List<IndustryGet> getAllIndustry()
        {
            string cmdText = ConfigurationManager.AppSettings["Industry_Get"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<IndustryGet>(cmdText);
        }
    }
}