using System;
using System.Collections.Generic;
using System.Web.SessionState;

namespace API.Models
{
    public class Model
    {
        public int InventoryMasterID { get; set; }
        public decimal? MonthlyRentalRate { get; set; }
        public decimal? ListRateCAN { get; set; }
        public decimal? ListRateCE { get; set; }
        public decimal? ListRateLA { get; set; }
        public decimal? ListRateAUD { get; set; }
        public decimal? ListRateINT { get; set; }
        public string User { get; set; }
        public decimal? TransportationCost { get; set; }
        public decimal? InsuranceCost { get; set; }
        public decimal? PreventiveMaintenance { get; set; }
        public decimal? PreventiveMaintenanceCost { get; set; }
        public decimal? PreventiveMaintenancePrice { get; set; }
        public bool RequiresPMPricing { get; set; }
        public string SpecSheet { get; set; }
        public decimal? SalePrice { get; set; }
        public bool ForRent { get; set; }
        public string ModelImage { get; set; }
        public string ModelNum { get; set; }
        public string MessageForWebsite { get; set; }
        public bool ShowOnWebsite { get; set; }
        public decimal? Weight { get; set; }
        public string WeightUnits { get; set; }
        public string Height { get; set; }
        public string HeightUnits { get; set; }
        public string Length { get; set; }
        public string LengthUnits { get; set; }
        public string Width { get; set; }
        public string WidthUnits { get; set; }
        public string FrontAttachmentCategory { get; set; }
        public string FrontAttachmentType { get; set; }
        public string FrontAttachmentMake { get; set; }
        public string FrontAttachmentModel { get; set; }
        public string RearAttachmentCategory { get; set; }
        public string RearAttachmentType { get; set; }
        public string RearAttachmentMake { get; set; }
        public string RearAttachmentModel { get; set; }
        public string TertiaryAttachmentCategory { get; set; }
        public string TertiaryAttachmentType { get; set; }
        public string TertiaryAttachmentMake { get; set; }
        public string TertiaryAttachmentModel { get; set; }
        public string FullDescription { get; set; }
        public string MarketingDescription { get; set; }
        public IEnumerable<ModelPhoto> ModelPhotos { get; set; }
        public IEnumerable<ModelSpec> ModelSpecs { get; set; }
        public IEnumerable<ModelFile> ModelFiles { get; set; }
    }
    public class ModelRentalData
    {
        public string ProductType { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string Manufacturer { get; set; }
        public string ModelNum { get; set; }
        public decimal MonthlyRentalRate { get; set; }
        public string MessageForWebsite { get; set; }
        public bool ShowOnWebsite { get; set; }

    }

    public abstract class IGeneralDetail
    {
        public decimal? MonthlyRentalRate { get; set; }
        public decimal? ListRateCAN { get; set; }
        public decimal? ListRateCE { get; set; }
        public decimal? ListRateLA { get; set; }
        public decimal? ListRateAUD { get; set; }
        public decimal? ListRateINT { get; set; }
        public decimal? FootRate { get; set; }
        public decimal? FootRateCAN { get; set; }
        public decimal? FootRateCE { get; set; }
        public decimal? FootRateLA { get; set; }
        public decimal? FootRateAUD { get; set; }
        public decimal? FootRateINT { get; set; }
        public string FullDescription { get; set; }
        public string MarketingDescription { get; set; }
        public decimal? ShippingPerMile25Cost { get; set; }
        public decimal? ShippingPerMile25Price { get; set; }
        public decimal? ShippingPerMile50Cost { get; set; }
        public decimal? ShippingPerMile50Price { get; set; }
        public decimal? ShippingPerMile50AboveCost { get; set; }
        public decimal? ShippingPerMile50AbovePrice { get; set; }
        public decimal? ShippingPerHour4MinimumCost { get; set; }
        public decimal? ShippingPerHour4MinimumPrice { get; set; }
        public decimal? ShippingPerHour4AboveCost { get; set; }
        public decimal? ShippingPerHour4AbovePrice { get; set; }
        public decimal? ShippingFlatRateCost { get; set; }
        public decimal? ShippingFlatRatePrice { get; set; }
        public decimal? PreventiveMaintenance250Cost { get; set; }
        public decimal? PreventiveMaintenance250Price { get; set; }
        public decimal? PreventiveMaintenance500Cost { get; set; }
        public decimal? PreventiveMaintenance500Price { get; set; }
        public decimal? PreventiveMaintenance750Cost { get; set; }
        public decimal? PreventiveMaintenance750Price { get; set; }
        public decimal? PreventiveMaintenance1000Cost { get; set; }
        public decimal? PreventiveMaintenance1000Price { get; set; }
        public decimal? PreventiveMaintenanceCost { get; set; }
        public decimal? PreventiveMaintenancePrice { get; set; }
        public decimal? DamageInsPerHourCost { get; set; }
        public decimal? DamageInsPerHourPrice { get; set; }
        public decimal? DamageInsPerMonthCost { get; set; }
        public decimal? DamageInsPerMonthPrice { get; set; }
    }

    public class ModelMobileView : IGeneralDetail
    {
        public int InventoryMasterID { get; set; }
        public bool Active { get; set; }
        public string Category { get; set; }
        public int ProductCategoryID { get; set; }
        public string Make { get; set; }
        public int ManufacturerID { get; set; }
        public string Model { get; set; }
        public string RentalReservationGroup { get; set; }
        public decimal? Weight { get; set; }
        public string WeightUnits { get; set; }
        public string Height { get; set; }
        public string HeightUnits { get; set; }
        public string Length { get; set; }
        public string LengthUnits { get; set; }
        public string Width { get; set; }
        public string WidthUnits { get; set; }
        public string FrontAttachmentCategory { get; set; }
        public string FrontAttachmentType { get; set; }
        public string FrontAttachmentMake { get; set; }
        public string FrontAttachmentModel { get; set; }
        public string RearAttachmentCategory { get; set; }
        public string RearAttachmentType { get; set; }
        public string RearAttachmentMake { get; set; }
        public string RearAttachmentModel { get; set; }
        public string TertiaryAttachmentCategory { get; set; }
        public string TertiaryAttachmentType { get; set; }
        public string TertiaryAttachmentMake { get; set; }
        public string TertiaryAttachmentModel { get; set; }
        public List<ModelSpec> ModelSpecs { get; set; }
        public decimal? LauriniCostUSD { get; set; }
        public decimal? LauriniCostEuro { get; set; }
        public int LauriniManufacturingTime { get; set; }
        public string LauriniManufacturingTimeUnit { get; set; }
        public decimal? LauriniPriceUSD { get; set; }
        public decimal? LauriniPriceCanada { get; set; }
        public decimal? LauriniPriceEurope { get; set; }
        public decimal? LauriniPriceAustralia { get; set; }
        public decimal? LauriniPriceLatinAmerica { get; set; }
        public decimal? LauriniPriceOther { get; set; }
        public List<ModelCompetitor> Competitors { get; set; }
        public List<ModelPhoto> Photos { get; set; }
        public List<ModelFile> Files { get; set; }

    }

    public class ModelSpec
    {
        public int ModelsSpecID { get; set; }
        public int InventoryMasterID { get; set; }
        public bool Active { get; set; }
        public string SpecLabel { get; set; }
        public string SpecValue { get; set; }
        public string SpecUnitType { get; set; }
        public string EnterUserStr { get; set; }
        public DateTime EnterDateTime { get; set; }
        public string EditUserStr { get; set; }
        public DateTime EditDateTime { get; set; }
    }

    public abstract class IPhotos
    {
        public bool Active { get; set; }
        public bool IsPrimary { get; set; }
        public string PhotoName { get; set; }
        public string PhotoType { get; set; }
        public string PhotoUrl { get; set; }
        public string EnterUserStr { get; set; }
        public DateTime EnterDateTime { get; set; }
        public string EditUserStr { get; set; }
        public DateTime EditDateTime { get; set; }
    }

    public abstract class IFiles
    {
        public bool Active { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileUrl { get; set; }
        public string EnterUserStr { get; set; }
        public DateTime EnterDateTime { get; set; }
        public string EditUserStr { get; set; }
        public DateTime EditDateTime { get; set; }
    }

    public class ModelPhoto : IPhotos
    {
        public int ModelsPhotosID { get; set; }
        public int InventoryMasterID { get; set; }
    }

    public class ModelFile : IFiles
    {
        public int ModelsFilesID { get; set; }
        public int InventoryMasterID { get; set; }
    }

    public class ModelCompetitor
    {
        public int ModelsCompetitorsID { get; set; }
        public int InventoryMasterID { get; set; }
        public bool Active { get; set; }
        public string CompetitorName { get; set; }
        public decimal? CompetitorUSD { get; set; }
        public decimal? CompetitorEuro { get; set; }
        public string EnterUserStr { get; set; }
        public DateTime EnterDateTime { get; set; }
        public string EditUserStr { get; set; }
        public DateTime EditDateTime { get; set; }
    }


    public class InventoryMaster: ModelMobileView
    {
        public string EnterUserStr { get; set; }
    }

    public class RentalReservationGroupData
    {
        public int RentalReservationGroupID { get; set; }
        public string RentalReservationGroup { get; set; }
        public string EnterUserStr { get; set; }
        public DateTime EnterDateTime { get; set; }
        public string EditUserStr { get; set; }
        public DateTime EditDateTime { get; set; }
        public RentalReservationGroupDetail RentalReservationGroupDetail { get; set; }
    }

    public class RentalReservationGroupDetail : IGeneralDetail
    {
        public int RentalReservationGroupDetailID { get; set; }
        public int RentalReservationGroupID { get; set; }
        public bool Active { get; set; }
        public DateTime InactiveDate { get; set; }
        public string InactiveBy { get; set; }
        public List<RentalReservationPhoto> Photos { get; set; }
        public List<RentalReservationFile> Files { get; set; }
        public string EnterUserStr { get; set; }
        public DateTime EnterDateTime { get; set; }
        public string EditUserStr { get; set; }
        public DateTime EditDateTime { get; set; }
    }

    public class RentalReservationGroupView : RentalReservationGroupDetail
    {
        public string RentalReservationGroup { get; set; }
        public List<string> Models { get; set; }
        public int EquipmentCount { get; set; }
        public int ModelCount { get; set; }
    }

    public class ShowHideItem
    {
        public int ID { get; set; }
        public bool Active { get; set; }
    }

    public class ShowHide
    {
        public List<ShowHideItem> RentalReservationGroup { get; set; }
        public string EnterUserStr { get; set; }
    }

    public class RentalReservationGroupModelList
    {
        
        public int InventoryMasterID { get; set; }
        public string ModelNum { get; set; }
        public int RentalReservationGroupID { get; set; }
        public string RentalReservationGroup { get; set; }
        public int ManufacturerID { get; set; }
        public string ManufacturerName { get; set; }
        public bool RentalReservationActive { get; set; }

    }

    public class NewModelViewGrid
    {
          public int EquipmentCount { get; set; }
        public string DefaultImage { get; set; }
        public int ImageCount { get; set; }
        public string DefaultImageUrl { get; set; }
        public bool Active { get; set; }
    }

    public class CategoryGrid:NewModelViewGrid
    {
        public string Category { get; set; }
        public int  ProductCategoryID { get; set; }
    }
    public class ReservationGroupGrid:NewModelViewGrid
    {
        public int RentalReservationGroupID { get; set; }
        public string    ReservationGroup { get; set; }
         public int    ModelCount { get; set; }
         public decimal? ListRateUS { get; set; }
        public decimal? ListRateCAN { get; set; }
        public decimal? ListRateEU { get; set; }
        public decimal? ListRateLA { get; set; }
        public decimal? ListRateAUS { get; set; }
        public decimal? ListRateINT { get; set; }
    
        public decimal? TPPPrice { get; set; }
        public decimal? TPPCost { get; set; }
    }

    public class ModelGrid : CategoryGrid
    {
        public string ReservationGroup { get; set; }
        public int InventoryMasterID { get; set; }
        public string     Make { get; set; }
        public int ManufacturerID { get; set; }
        public string     Model { get; set; }
        public decimal? ListRateUS { get; set; }
        public decimal? ListRateCAN { get; set; }
        public decimal? ListRateEU { get; set; }
        public decimal? ListRateLA { get; set; }
        public decimal? ListRateAUS { get; set; }
        public decimal? ListRateINT { get; set; }  
        public decimal? TPPPrice { get; set; }
        public decimal? TPPCost { get; set; }
        public decimal? DefaultSalesPrice { get; set; }
        public decimal? DefaultBuildCost { get; set; }
        public int AvailableOptions { get; set; }
    }

    public class  ProductCategory
    {
        public int ProductCategoryID { get; set; }
        public bool Active { get; set; }
        public string ProductCategoryName { get; set; }
        public List<CategoryImages> Images { get; set; }
        public string FullDescription { get; set; }
        public string MarketingDescription { get; set; }
        public string EnterUserStr { get; set; }
        public DateTime EnterDateTime { get; set; }
        public string EditUserStr { get; set; }
        public DateTime EditDateTime { get; set; }
    }

    public class CategoryImages
    {
        public int CategoryImagesID { get; set; }
        public int ProductCategoryID { get; set; }
        public bool Active { get; set; }
        public bool IsPrimary { get; set; }
        public string ImageName { get; set; }
        public string ImageType { get; set; }
        public string ImageUrl { get; set; }
        public string EnterUserStr { get; set; }
        public DateTime EnterDateTime { get; set; }
        public string EditUserStr { get; set; }
        public DateTime EditDateTime { get; set; }
    }

    public class RentalReservationPhoto : IPhotos
    {
        public int RentalReservationPhotosID { get; set; }
        public int RentalReservationID { get; set; }
    }

    public class RentalReservationFile : IFiles
    {
        public int RentalReservationFilesID { get; set; }
        public int RentalReservationID { get; set; }
    }


    public class CateAndManu
    {
        public string ManufacturerName { get; set; }
        public string ProductCategoryName { get; set; }
    }

    public class RentalReservationList
    {
        public string ProductCategoryName { get; set; }
        public string RentalReservationGroup { get; set; }
        public string Note { get; set; }
        public int Quantity { get; set; }
        public string CompanyName { get; set; }
        public DateTime EnterDateTime { get; set; }
        public DateTime RORDateTime { get; set; }
        public string AccountManagerFirstName { get; set; }
        public string AccountManagerLastName { get; set; }
    }
}