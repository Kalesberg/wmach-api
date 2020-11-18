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
        public IEnumerable<EquipmentPublic> getEquipmentPublic()
        {
            string cmdText = ConfigurationManager.AppSettings["Public_MachinesForSale"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<EquipmentPublic>(cmdText);
        }

        public IEnumerable<Categories> GetPublicCategoryFilters(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["Public_GetCategoryFilters"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<Categories>(cmdText, sqlParams);
        }

        public List<Makes> GetPublicMakeFilters(int ID, bool Visible, string Website)
        {
            string cmdText = ConfigurationManager.AppSettings["Public_GetMakeFilters"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            cmd.Parameters.AddWithValue("@CategoryID", ID);
            cmd.Parameters.AddWithValue("@ParentVisible", Visible);
            cmd.Parameters.AddWithValue("@Website", Website);
            return getRecords<Makes>(cmdText);
        }

        public List<FilterPublic> GetPublicModelFilters(int CategoryID, int MakeID, bool Visible, string Website)
        {
            string cmdText = ConfigurationManager.AppSettings["Public_GetModelFilters"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
            cmd.Parameters.AddWithValue("@ManufacturerID", MakeID);
            cmd.Parameters.AddWithValue("@ParentVisible", Visible);
            cmd.Parameters.AddWithValue("@Website", Website);
            return getRecords<FilterPublic>(cmdText);
        }

        public void UpdatePublicWesbiteCategoryFilterVisibility(string website, string action, JObject categoryIDs)
        {
            string cmdText = action.ToLower() == "hide" ? ConfigurationManager.AppSettings["Public_HideFilters"] : ConfigurationManager.AppSettings["Public_ShowFilters"];
            if (String.IsNullOrWhiteSpace(cmdText)) return;

            foreach (var item in categoryIDs["IDs"].Values())
            {
                cmd.Parameters.AddWithValue("@FilterType", "category");
                cmd.Parameters.AddWithValue("@Website", website);
                cmd.Parameters.AddWithValue("@CategoryID", item.Value<int>());
                InsertData(cmdText);
                cmd.Parameters.Clear();
            }
                
        }

        public void UpdatePublicWesbiteMakeFilterVisibility(string website, string action, int categoryID, JObject makeIDs)
        {
            string cmdText = action.ToLower() == "hide" ? ConfigurationManager.AppSettings["Public_HideFilters"] : ConfigurationManager.AppSettings["Public_ShowFilters"];
            if (String.IsNullOrWhiteSpace(cmdText)) return;

            foreach (var item in makeIDs["IDs"].Values())
            {
                cmd.Parameters.AddWithValue("@FilterType", "make");
                cmd.Parameters.AddWithValue("@Website", website);
                cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                cmd.Parameters.AddWithValue("@MakeID", item.Value<int>());
                InsertData(cmdText);
                cmd.Parameters.Clear();
            }
        }

        public void UpdatePublicWesbiteModelFilterVisibility(string website, string action, int categoryID, int makeID, JObject modelIDs)
        {
            string cmdText = action.ToLower() == "hide" ? ConfigurationManager.AppSettings["Public_HideFilters"] : ConfigurationManager.AppSettings["Public_ShowFilters"];
            if (String.IsNullOrWhiteSpace(cmdText)) return;

            foreach (var item in modelIDs["IDs"].Values())
            {
                cmd.Parameters.AddWithValue("@FilterType", "model");
                cmd.Parameters.AddWithValue("@Website", website);
                cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                cmd.Parameters.AddWithValue("@MakeID", makeID);
                cmd.Parameters.AddWithValue("@ModelID", item.Value<int>());
                InsertData(cmdText);
                cmd.Parameters.Clear();
            }
        }

        public IEnumerable<PublicRentalCategory> getCategoryBySiteAndMake(JObject json)
        {
            string cmdText = ConfigurationManager.AppSettings["WebsitesCategory"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<PublicRentalCategory>(cmdText, json);
        }
    }
}