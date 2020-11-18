using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.SessionState;

namespace API.Models
{
    public class Equipment
    {
        public int EquipmentID { get; set; }
        public string SerialNum { get; set; }
        public bool ForRent { get; set; }
        public bool ForSale { get; set; }
        public bool PublicViewable { get; set; }
        public bool PublicPriceViewable { get; set; }
        public decimal? Price { get; set; }
        public decimal? BrokerPrice { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MonthlyRentalRate { get; set; }
        public decimal? InternationalRentalRate { get; set; }
        public decimal? CNBV { get; set; }
        public decimal? OLV { get; set; }
        public DateTime? CNBVDate { get; set; }
        public DateTime? NBVDate { get; set; }
        public decimal? InsuranceValue { get; set; }
        public string AttachmentPosition { get; set; }
        public string PropertyTag { get; set; }
        public string RentalStatus { get; set; }
        public string GpsFilter { get; set; }
        public string OwnerType { get; set; }
        public string LocationStatus { get; set; }
        public string ServiceStatus { get; set; }
        public int? YearManufactured { get; set; }
        public int? Hours { get; set; }
        public int? Miles { get; set; }
        public DateTime? LastAppraisalDate { get; set; }
        public DateTime? DateAcquired { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? RPOPrice { get; set; }
        public DateTime? DateSold { get; set; }
        public decimal? SoldPrice { get; set; }
        public string ModelNum { get; set; }
        public DateTime? LastCheckInDate { get; set; }
        public DateTime? LastCheckOutDate { get; set; }
        public string ManufacturerName { get; set; }
        public string Category { get; set; }
        public string RentalReservationGroup { get; set; }
        public string Division { get; set; }
        public bool GpsEnabled { get; set; }
        public string GpsProvider { get; set;  }
        public string EquipmentType { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public decimal? PositionLatitude { get; set; }
        public decimal? PositionLongitude { get; set; }
        public DateTime? EquipmentSummaryDate { get; set; }
        public string YardName { get; set; }
        public List<string> Pictures { get; set; }
        public List<Attachment> Attachments { get; set; } 
        public Contract ContractDetails { get; set; }
        public decimal? ActualRentalRate { get; set; }
        public Transport Transportation { get; set; }
        public string AttachmentType { get; set; }
        public string EquipmentDescriptor { get; set; }
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
        public string ContractRentalPeriod { get; set; }
        public decimal? NBV { get; set; }
        public string PrivateDescription { get; set; }
    }

    public class Attachment
    {
        public int EquipmentID { get; set; }
        public string SerialNum { get; set; }
        public string AttachmentType { get; set; }
        public string AttachmentPosition { get; set; }
    }
    public class AttachmentNameAndPosition : Attachment
    {
        public string AttachmentPosition { get; set; }
        public string AttachmentDescrption { get; set; }
    }

    public class EquipmentDetailCustomerPortal
    {
        public List<string> Pictures { get; set; }
        public string Category { get; set; }
        public string ManufacturerName { get; set; }
        public string ModelNum { get; set; }
        public string SerialNum { get; set; }
        public int? YearManufactured { get; set; }
        public int? Hours { get; set; }
        public decimal? ForSalePrice { get; set; }
        public List<AttachmentNameAndPosition> Attachments { get; set; } 
        public int ContractNum { get; set; }
        public int ContractID { get; set; }
        public ContactAddress Jobsite { get; set; }
        public string JobSiteContact { get; set; }
        public string JobSitePhone { get; set; }
        public string JobSiteEmail { get; set; }
        public DateTime LastServiceDate { get; set; }
        public Contact AccountManager { get; set; }
        public Contact RentalCoordinator { get; set; }
        public Contact ServiceManager { get; set; }
        public string ContractType { get; set; }
        public int? RentalPeriod{ get; set; }
        public string RentalPeriodTimeSpan { get; set; }
        public decimal? RentalRate { get; set; }
        public decimal? RentalPurchaseOptionPrice { get; set; }
        public DateTime EstimatedStartDate { get; set; }
        public DateTime ActualStartDate { get; set; }
        public DateTime ActualEndDate { get; set; }
        public string ServiceStatus { get; set; }
        public string RentalStatus { get; set; }
        public string LocationStatus { get; set; }
        [JsonIgnore]
        public int  RentalCoordinatorID { get; set; }
        [JsonIgnore]
        public int ContractDtlID { get; set; }
        [JsonIgnore]
        public string ServiceManagerID { get; set; }
        [JsonIgnore]
        public int JobsiteID { get; set; }
        [JsonIgnore]
        public int AccountManagerID { get; set; }
        [JsonIgnore]
        public int EquipmentID { get; set; }
       
    }


    //CP-193
    public class EquipmentOnRent
    {

        public string ProductCategoryName { get; set; }
        public string ManufacturerName { get; set; }
        public string ModelNum { get; set; }
        public string SerialNum { get; set; }
        public int ManufacturedYear { get; set; }
        public int Hours { get; set; }
        public string Address1 { get; set; }
        public DateTime ActualStartDate { get; set; }
        public int ContractNum { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal OptionPrice { get; set; }
        public string EquipmentGPSEnabled { get; set; }
        public string PoNum { get; set; }
        public int ContractID { get; set; }
        public int MinTerm { get; set; }
        public string MinTermUOM { get; set; }
    }

    public class CheckInOutLast
    {
        public string Type { get; set; }
        public DateTime? InspectionDate { get;set; }
    }
}