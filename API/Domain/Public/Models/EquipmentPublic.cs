using System;
using System.Collections.Generic;
using System.Web.SessionState;

namespace API.Models
{
    public class EquipmentPublic
    {
        public int EquipmentID { get; set; }
        public string SerialNum { get; set; }
        public bool ForRent { get; set; }
        public bool ForSale { get; set; }
        public bool PublicPriceViewable { get; set; }
        public decimal? Price { get; set; }
        public decimal? BrokerPrice { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MonthlyRentalRate { get; set; }
        public string RentalStatus { get; set; }
        public string GpsFilter { get; set; }
        public string OwnerType { get; set; }
        public string LocationStatus { get; set; }
        public string ServiceStatus { get; set; }
        public int? YearManufactured { get; set; }
        public int? Hours { get; set; }
        public int? Miles { get; set; }
        public DateTime? DateAcquired { get; set; }
        public DateTime? DateSold { get; set; }
        public string ModelNum { get; set; }
        public string ManufacturerName { get; set; }
        public string Category { get; set; }
        public string Division { get; set; }
        public bool GpsEnabled { get; set; }
        public string GpsProvider { get; set; }
        public string EquipmentType { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public DateTime? EquipmentSummaryDate { get; set; }
        public string YardName { get; set; }
        public List<string> Pictures { get; set; }
        public List<Attachment> Attachments { get; set; }
        public string AttachmentType { get; set; }
        public string MarketingDescription { get; set; }
        public string AttachmentCategory { get; set; }
        public string AttachedToSerialNum { get; set; }
        public string AttachedToModelNum { get; set; }
        public string AttachmentFitsOn { get; set; }
        public string Size { get; set; }
        public decimal? ListRateUS { get; set; }
        public decimal? ListRateCAN { get; set; }
        public decimal? ListRateCE { get; set; }
        public decimal? ListRateLA { get; set; }
        public decimal? ListRateAUD { get; set; }
    }

    public class PublicRentalCategory
    {
        public string Category { get; set; }
        public string ImageUrl { get; set; }
    }
}