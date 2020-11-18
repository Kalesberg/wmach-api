using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class EquipmentLegacy
    {
        int EquipmentID { get; set; }
        int MachineAttachedToID { get; set; }
        string AttachmentPosition { get; set; }
        string RentalStatus { get; set; }
        int? AssignedDivisionID { get; set; }
        int? CurrentLocationID { get; set; }
        string SerialNum { get; set; }
        string SerialNumStripped { get; set; }
        string PropertyTag { get; set; }
        string PropertyTagStripped { get; set; }
        int? SupplierID { get; set; }
        int? InventoryMasterID { get; set; }
        int? PictureCount { get; set; }
        string BarCode { get; set; }
        decimal? AdditionalCapCost { get; set; }
        string MarketingDescription { get; set; }
        int? ConditionID { get; set; }
        int? AppraiserID { get; set; }
        bool? ForRent { get; set; }
        bool? ForSale { get; set; }
        bool? Rerented { get; set; }
        bool? PublicViewable { get; set; }
        bool? PublicPriceViewable { get; set; }
        decimal? Price { get; set; }
        decimal? MinPrice { get; set; }
        decimal? MonthlyRentalRate { get; set; }
        decimal? OrderlyLiquidationValue { get; set; }
        decimal? InsuranceValue { get; set; }
        decimal? RentalPurchaseOptionPrice { get; set; }
        decimal? PurchasePrice { get; set; }
        int? Hours { get; set; }
        int? ManufacturedYear { get; set; }
        DateTime? DateAcquired { get; set; }
        DateTime? DateSold { get; set; }
        bool? IsTemplate { get; set; }
        bool? Active { get; set; }
        bool? Deletable { get; set; }
        string EnterUserStr { get; set; }
        DateTime? EnterDateTime { get; set; }
        string EditUserStr { get; set; }
        DateTime? EditDateTime { get; set; }
        //TimeStamp Timestamp
        string OwnerType { get; set; }
        string ProjectNumber { get; set; }
        int? OriginalHour { get; set; }
        DateTime? RentDateAvailable { get; set; }
        DateTime? SaleDateAvailable { get; set; }
        string LocationStatus { get; set; }
        string PackageNumber { get; set; }
        int? EnterpriseBuyerID { get; set; }
        decimal? AcqAttachmentPrice { get; set; }
        decimal? AcqShippingCost { get; set; }
        bool? AddToFixedAsset { get; set; }
        DateTime? DateSentFixedAsset { get; set; }
        int? OwnedByID { get; set; }
        DateTime? LastAppraisalDate { get; set; }
        decimal? MinMonthlyRentalRate { get; set; }
        DateTime? WarrantyExpDate { get; set; }
        int? WarrantyExpClick { get; set; }
        string WarrantyServicer { get; set; }
        bool? PurchasedNew { get; set; }
        string OriginalPONumber { get; set; }
        bool? DepreciateAsset { get; set; }
        decimal? SoldPrice { get; set; }
        int? SoldToID { get; set; }
        string SoldInvoiceNumber { get; set; }
        decimal? BrokerPrice { get; set; }
        bool? PublicViewSerialNum { get; set; }
        decimal? EquipmentSize { get; set; }
        string EquipmentSizeUnit { get; set; }
        string AttachmentDefaultPosition { get; set; }
        decimal? Weight { get; set; }
        decimal? Height { get; set; }
        decimal? Length { get; set; }
        decimal? Width { get; set; }
        int? LocationContactRelationshipID { get; set; }
        string EquipmentDescriptor { get; set; }
        string ServiceStatus { get; set; }
        string UnitNumber { get; set; }
        string SMMPlate { get; set; }
        string SMMTag { get; set; }
        int? SalesmanContactID { get; set; }
        DateTime? SmmTabExpiresOn { get; set; }
        DateTime? SmmPlateExpiresOn { get; set; }
        string LicensingNotes { get; set; }
        string SmmTabLastOne { get; set; }
        string SmmPlateLastOne { get; set; }
        string EquipmentStatusNotes { get; set; }
        string SizeRangeType { get; set; }
        string CustomerName { get; set; }
        string CustomerPhone { get; set; }
        string EngArrNum { get; set; }
        string TranArrNum { get; set; }
        string MachArrNum { get; set; }
        string EngSerialNum { get; set; }
        string TranSerialNum { get; set; }
        bool? HasHours { get; set; }
        int? Generation { get; set; }
        int? AssetOwner { get; set; }
        int? AssetLife { get; set; }
        bool? HasMiles { get; set; }
        int? Miles { get; set; }
        bool? ExcludeFromForecast { get; set; }
        decimal? AuctionValue { get; set; }
        string RecordStatus { get; set; }
    }
}