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
        public Opportunity GetOpportunity(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["GetOpportunity"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            Func<DataTable, List<Opportunity>> transform = transformOpportunity;
            return getRecords<Opportunity>(cmdText, transform, sqlParams).First();
        }

        public List<OpportunityMetricAgg> getLostOpportunityMetricsByReason(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["OpportunityMetricsByReasonAggregated"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            Func<DataTable, List<OpportunityMetricAgg>> transform = opportunityAggregateTransform;
            return getRecords<OpportunityMetricAgg>(cmdText, transform, sqlParams);
        }

        public List<OpportunityMetricAgg> getLostOpportunityMetricsByModel(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["OpportunityMetricsByModelAggregated"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            Func<DataTable, List<OpportunityMetricAgg>> transform = opportunityAggregateTransform;
            return getRecords<OpportunityMetricAgg>(cmdText, transform, sqlParams);
        }

        public List<OpportunityLostReasons> GetOpportunityReasons()
        {
            string cmdText = ConfigurationManager.AppSettings["OpportunityReasons"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            Func<DataTable, List<OpportunityLostReasons>> transform = transformLostOpportunityReasons;
            return getRecords<OpportunityLostReasons>(cmdText, transform);
        }

        public bool CreateOpportunity(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["CreateOpportunity"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return InsertData(cmdText, sqlParams);
        }

        public List<Opportunity> transformOpportunity(DataTable data)
        {
            var opp = data.AsEnumerable().GroupBy(r => r["OpportunityID"].ToString())
                                              .Select(r => new Opportunity
                                              {
                                                  opportunityType = r.Select(row => row["opportunityType"].ToString()).First(),
                                                  customer = r.Select(row => row["customer"].ToString()).First(),
                                                  jobLocation = r.Select(row => row["JobLocation"].ToString()).First(),
                                                  remarks = r.Select(row => row["remarks"].ToString()).First(),
                                                  equipment = r.Select(rec => new OpportunityItem
                                                  {
                                                      quantity = Int32.Parse(rec["Quantity"].ToString()),
                                                      category = rec["Category"].ToString(),
                                                      manufacturer = rec["Manufacturer"].ToString(),
                                                      model = rec["ModelNum"].ToString(),
                                                      reason = rec["Reason"].ToString()
                                                  }).ToList()
                                              }).ToList();


            return opp;

        }

        public List<OpportunityMetricAgg> opportunityAggregateTransform(DataTable data)
        {
            if (data.Rows.Count == 0) return null;

            var oppAgg = data.AsEnumerable().Select(dr => new OpportunityMetricAgg
            {
                Aggregate = dr["aggregated"].ToString(),
                Count = Int32.Parse(dr["count"].ToString()),
                Total = Int32.Parse(dr["total"].ToString())
            }).ToList();
            return oppAgg;
        }

        public List<OpportunityLostReasons> transformLostOpportunityReasons(DataTable data)
        {
            var opp = data.AsEnumerable().GroupBy(r => r["OpportunityType"].ToString())
                                         .Select(r => new OpportunityLostReasons
                                         {
                                             opportunityType = r.Key,
                                             reasons = r.Select(rec => new OpportunityLostReason
                                             {
                                                 ID = (int)rec["ID"],
                                                 reason = rec["reason"].ToString()
                                             }).ToList()
                                         }).ToList();

            return opp;
        }

        public List<OpportunityLost> GetAllOpportunities()
        {
            string cmdText = ConfigurationManager.AppSettings["GetAllOpportunities"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<OpportunityLost>(cmdText);
        }

        public int CreateNewOpportunity(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["OpportunityCreate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return InsertRecord(cmdText, sqlParams);
        }

        public int CreateNewOpportunityItem(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["OpportunityItemCreate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return InsertRecord(cmdText, sqlParams);
        }

        public int CreateNewOpportunityItemAttchment(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["OpportunityAttachmentsOfItemCreate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return InsertRecord(cmdText, sqlParams);
        }

        public OpportunityMobile getOpportunitySelect(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["OpportunitySelect"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<OpportunityMobile>(cmdText, sqlParams).FirstOrDefault();
        }
        public List<OpportunityItemMobile> getOpportunityItemSelect(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["OpportunityItemSelect"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<OpportunityItemMobile>(cmdText, sqlParams);
        }
        public List<OpportunityAttachmentsOfItem> getOpportunityAttachmentsOfItemSelect(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["OpportunityAttachmentsOfItemSelect"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<OpportunityAttachmentsOfItem>(cmdText, sqlParams);
        }

        public bool UpdateOpportunity(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["OpportunityUpdate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecord(cmdText, sqlParams);
        }

        public bool UpdateOpportunityItem(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["OpportunityItemUpdate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecord(cmdText, sqlParams);
        }

        public bool UpdateOpportunityItemAttchment(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["OpportunityAttachmentsOfItemUpdate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecord(cmdText, sqlParams);
        }

        public Boolean DeactiveOpportunity(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["DeactiveOpportunity"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecord(cmdText, sqlParams);
        }
        public Boolean DeactiveOpportunityItem(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["DeactiveOpportunityItem"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecord(cmdText, sqlParams);
        }
        public Boolean DeactiveOpportunityAttachemntofItem(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["DeactiveOpportunityAttachmentofItem"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecord(cmdText, sqlParams);
        }
    }
}