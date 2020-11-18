using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    //Lost Rentals and Lost Sales
    public class OpportunityItem
    {
        public int quantity { get; set; }
        public string category { get; set; }
        public string manufacturer { get; set; }
        public string model { get; set; }
        public string reason { get; set; }
    }

    public class Opportunity
    {
        public string opportunityType { get; set; }
        public string customer { get; set; }
        public string jobLocation { get; set; }
        public string remarks { get; set; }
        public List<OpportunityItem> equipment { get; set; }
    }

    public class OpportunityMetrics
    {
        public List<string> Division { get; set; }
        public int Duration { get; set; }
        public List<OpportunityMetricAgg> Metrics { get; set; }
    }

    public class OpportunityMetricAgg
    {
        public string Aggregate { get; set; }
        public int Count { get; set; }
        public int Total { get; set; }
    }

    public class OpportunityLostReasons
    {
        public string opportunityType { get; set; }
        public List<OpportunityLostReason> reasons { get; set; }
    }

    public class OpportunityLostReason
    {
        public int ID { get; set; }
        public string reason { get; set; }
    }

    public enum OpportunityType
    {
        Rental,
        Sale
    }

    public class OpportunityLost
    {
        public int OpportunityItemID { get; set; }
        public int OpportunityID { get; set; }
        public string OpportunityType { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; }
        public string Manufacturer { get; set; }
        public string ModelNum { get; set; }
        public DateTime EnterDateTime { get; set; }
        public string EnterUserStr { get; set; }
        public string Customer { get; set; }
        public string Remarks { get; set; }
        public string DivisionShortName { get; set; }
        public string Reason { get; set; }
        public string AccountManager { get; set; }
        public string RentalCoordinator { get; set; }
        public int QuoteID { get; set; }
        public string SalesManager { get; set; }
        public decimal QuotedRate { get; set; }
        public string QuotedRateUOM { get; set; }
        public int RentalTerm { get; set; }
        public string RentalTermUOM { get; set; }
        
    }

    public class OpportunityMobile
    {
        public int OpportunityID { get; set; }
        public string OpportunityType { get; set; }
        public string Customer { get; set; }
        public string JobLocation { get; set; }
        public string Remarks { get; set; }
        public int DivisionID { get; set; }
        public int AccountManagerID { get; set; }
        public int RentalCoordinatorID { get; set; }
        public int SalesManagerID { get; set; }
        public int QuoteID { get; set; }
        public List<OpportunityItemMobile>  Equipments { get; set; }
        public string EnterUserStr { get; set; }
        public DateTime EnterDateTime { get; set; }
        public bool Active { get; set; }
        public string Division { get; set; }
    }

    public class OpportunityItemMobile
    {
        public int OpportunityItemID { get; set; }
        public int OpportunityID { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; }
        public string Manufacturer { get; set; }
        public string ModelNum { get; set; }
        public string Reason { get; set; }
        public decimal QuotedRate { get; set; }
        public string QuotedRateUOM { get; set; }
        public int RentalTerm { get; set; }
        public string RentalTermUOM { get; set; }
        public bool Active { get; set; }
        public string EnterUserStr { get; set; }
        public DateTime EnterDateTime { get; set; }
        public List<OpportunityAttachmentsOfItem> Attachments { get; set; }
    }

    public class OpportunityAttachmentsOfItem
    {
        public int OpportunityAttachmentsOfItemID { get; set; }
        public int OpportunityItemID { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string ModelNum { get; set; }
        public string Position { get; set; }
        public bool Active { get; set; }
        public string EnterUserStr { get; set; }
        public DateTime EnterDateTime { get; set; }
        
    }

}