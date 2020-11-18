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
        public List<RentalCategories> GetRentalCategoryList()
        {
            string cmdText = ConfigurationManager.AppSettings["Category_List"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<RentalCategories>(cmdText);
        }

        public List<RentalCategoryModels> GetRentalCategoryModelList(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["Model_List"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<RentalCategoryModels>(cmdText, sqlParams);
        }
        public List<string> GetRentalCategoryIndustryList(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["CategoryIndustries"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<string>(cmdText, sqlParams);
        }
        public List<RentalCategoryModelsSpecs> GetRentalCategoryModelSpecList(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["ModelSpec_List"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<RentalCategoryModelsSpecs>(cmdText, sqlParams);
        }
        public List<RentalRelatedCategory> GetRentalModelRelatedCategories(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["ModelRelatedCategories"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<RentalRelatedCategory>(cmdText, sqlParams);
        }
        public RentalCategoryModelDetail GetRentalModelGroupDetail(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["Model_Select"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<RentalCategoryModelDetail>(cmdText, sqlParams).FirstOrDefault();
        }
        public bool CategoryImageInsertUrl(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["CategoryImage_InsertUrl"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false; 
            return UpdateRecord(cmdText, sqlParams);
        }
        public bool ModelGroupImageInsertUrl(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["ModelGroupImage_InsertUrl"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecord(cmdText, sqlParams);
        }
        
    }
}