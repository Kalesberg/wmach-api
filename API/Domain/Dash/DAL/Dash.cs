using System;
using API.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;


namespace API.Data
{
    public partial class DAL
    {
        public IEnumerable<Dash> GetDashes(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["getDashboardUtilization"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<Dash>(cmdText, sqlParams);
        }
    }
}