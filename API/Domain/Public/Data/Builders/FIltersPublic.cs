using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models;
using Newtonsoft.Json.Linq;

namespace API.Data
{
    public class PublicFilters : IBuildParams<IEnumerable<Categories>>
    {
        public IEnumerable<Categories> categories;

        public void Build(JObject json)
        {
            categories = DAL.GetInstance().GetPublicCategoryFilters(json);
            GetMakesAndModels(json["Website"].Value<string>());
            GetResult();
        }

        private void GetMakesAndModels(string Website)
        {
            foreach (var cat in categories)
            {
                cat.Makes = DAL.GetInstance().GetPublicMakeFilters(cat.ID, cat.Visible, Website);

                foreach (var make in cat.Makes)
                {
                   make.Models = DAL.GetInstance().GetPublicModelFilters(cat.ID, make.ID, make.Visible, Website);
                }
            }
        }

        public IEnumerable<Categories> GetResult()
        {
            return categories;
        }
    }
}