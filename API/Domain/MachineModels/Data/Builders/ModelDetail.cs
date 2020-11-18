using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models;
using Newtonsoft.Json.Linq;

namespace API.Data
{
    public class ModelDetail : IBuildParams<ModelMobileView>
    {
        public ModelMobileView ModelDetails;


        public void Build(JObject sqlParams)
        {
            ModelDetails = DAL.GetInstance().getModelDetail(sqlParams);
            if(ModelDetails != null)
            {
                ModelDetails.ModelSpecs = DAL.GetInstance().getModelSpecs(sqlParams);
                ModelDetails.Photos = DAL.GetInstance().getModelPhotos(sqlParams);
                ModelDetails.Files = DAL.GetInstance().getModelFiles(sqlParams);
                ModelDetails.Competitors = DAL.GetInstance().getModelCompetitors(sqlParams);
            }
            
        }

        public ModelMobileView GetResult()
        {
            return ModelDetails;
        }
    }
}