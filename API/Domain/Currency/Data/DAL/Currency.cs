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

        public List<Currency> getDivisionDefaultCurrency(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["GetDivisionDefaultCurrency"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<Currency>(cmdText, sqlParams);
        }

        public List<Currency> getCurrencies()
        {
            string cmdText = ConfigurationManager.AppSettings["GetCurrencies"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<Currency>(cmdText);
        }
    }
}