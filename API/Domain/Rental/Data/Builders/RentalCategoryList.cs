
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models;
using Newtonsoft.Json.Linq;

namespace API.Data
{
    public class RentalCategoryList : IBuild<IEnumerable<RentalCategories>>
    {
        private IEnumerable<RentalCategories> _RentalCategories;
        public void Build()
        {
            _RentalCategories = DAL.GetInstance().GetRentalCategoryList();
            foreach (RentalCategories cate in _RentalCategories)
            {
                cate.RentalCategoryModels = GetModelList(cate.CategoryID);
                cate.Industries = GetCategoryIndustries(cate.CategoryID);
                if (cate.RentalCategoryModels != null)
                {
                    foreach (RentalCategoryModels model in cate.RentalCategoryModels)
                    {
                        model.RentalCategoryModelsSpecs = GetModelSpecList(model.ModelGroupID);
                    }
                }
            }
        }

        public IEnumerable<RentalCategories> GetResult()
        {
            return _RentalCategories;
        }

        private List<RentalCategoryModels> GetModelList(int categoryID)
        {
            var json = new JObject { { "CategoryID", categoryID } };
            return DAL.GetInstance().GetRentalCategoryModelList(json);
        }
        private List<RentalCategoryModelsSpecs> GetModelSpecList(int modelID)
        {
            var json = new JObject { { "ModelGroupID", modelID } };
            return DAL.GetInstance().GetRentalCategoryModelSpecList(json);
        }
        private List<string> GetCategoryIndustries(int categoryID)
        {
            var json = new JObject { { "CategoryID", categoryID } };
            return DAL.GetInstance().GetRentalCategoryIndustryList(json);
        }
    }
}