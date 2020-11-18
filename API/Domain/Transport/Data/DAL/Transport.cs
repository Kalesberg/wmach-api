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
        public Transport getTransport(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["TransportDetails"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<Transport>(cmdText, sqlParams).FirstOrDefault();
        }

        public Transport getTransportByEquipmentId(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["TransportDetails"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<Transport>(cmdText, sqlParams).FirstOrDefault();
        }
        public List<Transportation> getShipmentInvetoryByContractDtlID(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["GetShipmentInvetoryByContractDtlID"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<Transportation>(cmdText, sqlParams);
        }

        public int createCustomerTransporationQuoteRequest(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["ShipmentQuotesRequestbyCustomer_Create"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return InsertRecord(cmdText, sqlParams);
        }

    }
}