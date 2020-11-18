using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{

    public class Rate
    {
        public string model { get; set; }
        public string monthly { get; set; }
        public string weekly { get; set; }
    }

    public class RateData
    {
        public List<RateCurrency> Currencies { get; set; }
        public List<RateContainer> Rates { get; set;}
    }

    public class RateContainer
    {
        public string EquipmentType { get; set; }
       // public List<RateCurrency> Currencies { get; set; }
        public List<RateCategory> EquipmentCategories { get; set; }
    }

    public class RateCategory
    {
        public string EquipmentCategory { get; set; }
        public List<dynamic> Values { get; set; }
    }

    public class RateSubCategory
    {
        public string SubCategory { get; set; }
        public List<Rate> Rates { get; set; }
    }

    public class RateCurrency
    {
        public string Prefix { get; set; }
        public string Region { get; set; }
        public decimal Multiplier { get; set; }
    }
}