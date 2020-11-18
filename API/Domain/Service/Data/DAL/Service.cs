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
        public IEnumerable<Service> getService(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["Service"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<Service>(cmdText, sqlParams);
        }

        public IEnumerable<Service> getEquipmentServiceHistory(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["ServiceHistory"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<Service>(cmdText, sqlParams);
        }

        public DataTable getServiceReport(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["ServiceSearch"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords(cmdText);
        }

        public DataTable getServiceEquipmentHistoryByWO(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["ServiceEquipmentHistoryByWO"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords(cmdText);
        }
        public IEnumerable<Service> getServiceByContractDtlID(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["GetServiceByContractDtlID"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<Service>(cmdText, sqlParams);
        }
        public IEnumerable<Checkout> GetCheckoutlist()
        {
            string cmdText = ConfigurationManager.AppSettings["GetCheckoutList"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<Checkout>(cmdText);
        }
      
    }
}