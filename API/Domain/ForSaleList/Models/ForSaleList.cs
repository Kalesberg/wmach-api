using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class ForSaleList
    {
        public string SerialNum { get; set; }
        public string EquipmentDescription { get; set; }
        public decimal ListPrice { get; set; }
        public decimal DealerPrice { get; set; }
        public string ManufacturedYear { get; set; }
        public int Hours { get; set; }
        public string Make { get; set; }
        public string ModelNum { get; set; }
        public string ProductCategory { get; set; }
        public string ProductType { get; set; }
        public bool ActiveCategory { get; set; }
        public string Categories { get; set; }
    }

    public class ForSaleListData
    {
        public string Username { get; set; }
        public string TextData { get; set; }
    }
}