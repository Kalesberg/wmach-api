
using API.Models;
using API.Services.HubspotService.Deals.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace API.Data
{
    public partial class DAL
    {
        public int CreateWehook(JObject webhookDTO)
        {
            string cmdText = ConfigurationManager.AppSettings["HubspotWebhooks"];
            if (string.IsNullOrWhiteSpace(cmdText)) return 1;
            return InsertRecord(cmdText, webhookDTO);
        }

        public int GetContactRelationshipForHubspot(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["GetContactRelationshipForHubspot"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return getRecords<int>(cmdText, sqlParams).FirstOrDefault();
        }

        public CateAndManu GetModelCategoryByModelNum(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["GetModelCategoryByModelNum"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<CateAndManu>(cmdText, sqlParams).FirstOrDefault();
        }
        public string GetHubspotEmailByContactId(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["GetHubspotEmailByContactId"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<string>(cmdText, sqlParams).FirstOrDefault();
        }
        public List<DealStages> GetAllDealStageValue(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["GetAllDealStageValue"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<DealStages>(cmdText, sqlParams);
        }
        public List<long> GetAllAssociatedDealIds(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["GetAllAllAssociatedHubspotDealIds"];
            if (String.IsNullOrWhiteSpace(cmdText)) return new List<long>();
            return getRecords<long>(cmdText, sqlParams);
        }

        public List<long> GetAllAssociatedDealIdsForContract(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["GetAllAssociatedHubspotDealIdsForContract"];
            if (String.IsNullOrWhiteSpace(cmdText)) return new List<long>();
            return getRecords<long>(cmdText, sqlParams);
        }
    }
}