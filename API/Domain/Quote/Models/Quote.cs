using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace API.Models
{
    public class Quote
    {
        public int quoteID { get; set; }
        public string QuoteType { get; set; }
        public string QuoteDetailType { get; set; }
        public string account { get; set; }
        public string division { get; set; }
        public string jobSite { get; set; }
        public int quoteNumber { get; set; }
        public int AccountManagerID { get; set; }
        public int CoordinatorID { get; set; }
        public int SalesManager { get; set; }
        public int DivisionID { get; set; }
        public List<QuoteItem> items { get; set; }
        public DateTime created { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? EstimatedStartDate { get; set; }
        public int ContactRelationshipID { get; set; }
        public string Terms { get; set; }
        public string EnterUserStr { get; set; }
        public string FOB { get; set;}
        public string QuoteStatus { get; set; }
        public decimal? Freight { get; set; }
        public DateTime QuoteDate { get; set; }
        public DateTime ExpirationDate { get;set; }
        public Boolean ShowWeight { get; set; }
	    public Boolean ShowQuantity { get; set; }
	    public Boolean ShowPicture { get; set; }
	    public Boolean ShowSerialNumber { get; set; }
	    public Boolean ShowTotal { get; set; }
        public Boolean ShowFreight { get; set; }
        public Boolean ShowFootRate { get; set; }
        public Boolean ShowPhotoLink { get; set; }
        public Boolean ShowCell { get; set; }
        public Boolean IncludeCurrentLocation { get; set; }
        public Boolean IncludeComponents { get; set; }
        public Boolean IncludeMarketingBlurb { get; set; }
        public Boolean IncludeMachineSpecifications { get; set; }
        public Boolean ShowMonthlyRate { get; set; }
        public Boolean ShowWeeklyRate { get; set; }
        public Boolean ShowDailyRate { get; set; }
        public Boolean ShowOvertimeRate { get; set; }
        public string Format { get; set; }
        public int? MinimumTerm { get; set; }
        public string MinimumTermUOM { get; set; }
        public IEnumerable<Equipment> equipments { get; set; }
        public IEnumerable<QuoteDetail> quoteDetails { get; set; }
        public Contact Contact { get; set; }
        public string CurrencyRegion { get; set; }
        public string CurrencyType { get; set; }
        public decimal TaxAmount { get; set; }
        public string TermsAndConditions { get; set; }
        public int? RPORate { get; set; }
        public int? RPOOptionTerms { get; set; }
        public string HashID { get; set; }
        public int JobSiteAddressID { get; set; }
        public bool IsAccepted { get; set; }
        public long DealID { get; set; }
    }

    public class QuoteItem
    {
        public string make { get; set; }
        public string model { get; set; }
        public decimal? weeklyRate { get; set; }
        public decimal? monthlyRate { get; set; }
        public decimal? dailyRate { get; set; }
        public int quantity { get; set; }
    }

    //quotedetails is the QuoteItem above. i didn't notice it first. we can easily remove QuoteItem class by changing a little.  
    public class QuoteDetail
    {
        public int QuoteID { get; set; }
        public int QuoteDetailID { get; set; }
        public int  EquipmentID{ get; set; }
        public int Quantity { get; set; }
        public int ProductCategoryID { get; set; }
        public string ProductType { get; set; }
        public string CategoryName { get; set; }
        public string Model { get; set; }
        public string ManufacturerName { get; set; }
        public decimal  MonthlyRate { get; set; }
        public decimal WeeklyRate { get; set; }
        public decimal FootRate { get; set; }
        public int MinFeet { get; set; }
        public string Description { get; set; }
        public decimal Freight { get; set; }
        public decimal OvertimeHourlyRate { get; set; }
        public string SerialNumber { get; set; }
        public decimal UnitPrice { get; set; }
        public QuoteDetailMoreFields QuoteDetailMoreFields { get; set; }
        public Model ModelDetail { get; set; }
        public Equipment Equipment { get; set; }
    }

    public class QuoteSearchResults
    {
        public int QuoteID { get; set; }
        public string Customer { get; set; }
        public int MachineTotal { get; set; }
        public string Saleperson { get; set; }
        public DateTime DateCreated { get; set; }
        public string Reason { get; set; }
        public string Contactperson { get; set; }
        public int QuoteNumber { get; set; }
    }

    public class QuoteDetailMoreFields
    {
         public int QuoteDetailID  { get; set; }
            public  int MinQuantity  { get; set; }
            public int MaxQuantity{ get; set; }
            public  int Discount  { get; set; }
            public string  Category { get; set; }
            public string Make{ get; set; }
            public string Model { get; set; }
            public int YearFrom { get; set; }
            public int YearTo { get; set; }
            public string FrontAttachment{ get; set; }
            public string RearAttachment { get; set; }
            public string TertiaryAttachment { get; set; }
            public string FrontType { get; set; }
            public string RearType { get; set; }
            public string TertiaryType { get; set; }
            public decimal TertiaryRate { get; set; }
            public string Notes { get; set; }
            public string FitsOnCategory  { get; set; }
            public string FitsOnMake { get; set; }
            public string FitsOnModel { get; set; }
            public string AttachmentCategory { get; set; }
            public string AttachmentType { get; set; }
            public string MaximumHours { get; set; }
            public string MaximumHoursUOM { get; set; }
            public int OverageHours { get; set; }
            public decimal OptionPrice { get; set; }
            public string FrontModel { get; set; }
            public string RearModel { get; set; }
            public string TertiaryModel { get; set; }
            public string AttachmentModel { get; set; }
            public int InventoryMasterID { get; set; }
            public string Specs { get; set; }
            public string Files { get; set; }
    }

    public class QuoteEmailPreference
    {
        public int EmailPreferenceID { get; set; }
        public int ContactID { get; set; }
        public string PreferenceType { get; set; }
        public string PreferenceName { get; set; }
        public string ContentData { get; set; }
        public Boolean Active { get; set; }
        public Boolean Default { get; set; }
        public string EnterUserStr { get; set; }
        public DateTime EnterDateTime { get; set; }
        public string EditUserStr { get; set; }
        public DateTime EditDateTime { get; set; }
    }

    public class QuoteListCustomerPortal
    {
        public int QuoteID { get; set; }
        public int QuoteNumber { get; set; }
        public string QuoteStatus { get; set; }
        public string QuoteType { get; set; }
        public string QuoteDetailType { get; set; }
        public string Contactperson { get; set; }
        public string DivisionName { get; set; }
        public DateTime QuoteDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime EstimateStartDate { get; set; }
        public string Location { get; set; }
        public int MachineTotal { get; set; }
        public string Salesperson { get; set; }
        public string SalespersonEmail { get; set; }
        public string SalespersonPhone { get; set; }
        public string AccountManager { get; set; }
        public string AccountManagerEmail { get; set; }
        public string AccountManagerPhone { get; set; }
        public string Coordinator { get; set; }
        public string CoordinatorEmail { get; set; }
        public string CoordinatorPhone { get; set; }
        public string ServiceManager { get; set; }
        public string ServiceManagerEmail { get; set; }
        public string ServiceManagerPhone { get; set; }
        public int MinimumTerm  { get; set; }
        public string MinimumTermUOM  { get; set; }
        public int RPORate { get; set; }
        public int RPOOptionTerms { get; set; }
        public bool IsAccepted { get; set; }
        public string AcceptedBy { get; set; }
        public DateTime DateTimeAccepted { get; set; }
        public decimal SalesPriceTotal { get; set; }
        public string FirstEquipmentDescription { get; set; }
        
    }

    public class CategoryType
    {
        public string ProductType { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
    }
}