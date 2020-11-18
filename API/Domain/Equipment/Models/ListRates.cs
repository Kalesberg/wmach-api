using System;
using System.Collections.Generic;
using System.Web.SessionState;
using System.Data;

namespace API.Models
{
    public class ListRates
    {
        public int? EquipmentID{ get; set; }
        public decimal? MonthlyRentalRate { get; set; }
        public decimal? ListRateCAN { get; set; }
        public decimal? ListRateCE { get; set; }
        public decimal? ListRateLA { get; set; }
        public decimal? ListRateAUD { get; set; }
        public decimal? ListRateINT { get; set; }
        public decimal? MinMonthlyRentalRate { get; set; }
        public decimal? MinMonthlyListRateCAN { get; set; }
        public decimal? MinMonthlyListRateCE { get; set; }
        public decimal? MinMonthlyListRateLA { get; set; }
        public decimal? MinMonthlyListRateAUD { get; set; }
        public decimal? MinMonthlyListRateINT { get; set; }
        public decimal? RentalPurchaseOptionPrice { get; set; }
        public decimal? RentalPurchaseOptionPriceCAN { get; set; }
        public decimal? RentalPurchaseOptionPriceCE { get; set; }
        public decimal? RentalPurchaseOptionPriceLA { get; set; }
        public decimal? RentalPurchaseOptionPriceAUD { get; set; }
        public decimal? RentalPurchaseOptionPriceINT { get; set; }
        public DataTable ListRateHistory { get; set; }
        public DateTime ListRateChangeDate { get; set; }
        public DateTime ListRateChangeDateCAN { get; set; }
        public DateTime ListRateChangeDateCE { get; set; }
        public DateTime ListRateChangeDateLA { get; set; }
        public DateTime ListRateChangeDateAUD { get; set; }
        public DateTime ListRateChangeDateINT { get; set; }
        public string UserName { get; set; }

    }
}
