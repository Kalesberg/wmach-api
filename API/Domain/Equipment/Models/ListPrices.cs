using System;
using System.Collections.Generic;
using System.Web.SessionState;
using System.Data;

namespace API.Models
{
    public class ListPrices
    {
        public int? EquipmentID{ get; set; }
        public decimal? Price { get; set; }
        public decimal? ListPriceCAN { get; set; }
        public decimal? ListPriceCE { get; set; }
        public decimal? ListPriceLA { get; set; }
        public decimal? ListPriceAUD { get; set; }
        public decimal? ListPriceINT { get; set; }


    }

    public class PriceHistory
    {
        public string  OldPrice { get; set; }
        public string  NewPrice { get; set; }
        public string  UpdateBy { get; set; }
        public string  DateChanged { get; set; }
        public string  PriceChanged { get; set; }
    }
}
