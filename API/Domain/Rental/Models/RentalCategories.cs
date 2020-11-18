using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
namespace API.Models
{
    public class RentalCategories
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public int ProductType { get; set; }
        public string AssetType { get; set; }
        public int SortOrder { get; set; }
        public string ImageName { get; set; }
        public string ImageUrl { get; set; }
        public List<string> Industries { get; set; }
        public List<RentalCategoryModels> RentalCategoryModels { get; set; }
    }
    public class RentalCategoryModels
    {
        public int ModelGroupID { get; set; }
        public int CategoryID { get; set; }
        public string ModelGroupName { get; set; }
        public string ModelGroupMetaKeywords { get; set; }
        public string ModelGroupMetaDescription { get; set; }
        public string ImageName { get; set; }
        public string ImageUrl { get; set; }
        public List<RentalCategoryModelsSpecs> RentalCategoryModelsSpecs { get; set; }

    }

    public class RentalCategoryModelsSpecs
    {
        public int ModelGroupSpecID { get; set; }
        public int ModelGroupID { get; set; }
        public string SpecGroupName { get; set; }
        public int SortOrder { get; set; }
        public string SpecLabel { get; set; }
        public string SpecValue { get; set; }
    }

    public class RentalCategoryModelDetail : RentalCategoryModels
    {
        public string CategoryName { get; set; }
        public string ModelGroupDescription { get; set; }
        public List<RentalRelatedCategory> RelatedCategories { get; set; }
    }

    public class RentalCartImageUpload
    {
        public List<RentalCartImage> RentalCartImage { get; set; }
    }

    public class RentalCartImage
    {
        public string ImageName { get; set; }
        public string ImageUrl { get; set; }
        public string EditUserStr { get; set; }
    }
    public class RentalRelatedCategory
    {
        public string CategoryName { get; set; }
        public int CategoryID { get; set; }
        public int DisplayOrder { get; set; }
        public string ImageName { get; set; }
        public string ImageUrl { get; set; }
    }
}