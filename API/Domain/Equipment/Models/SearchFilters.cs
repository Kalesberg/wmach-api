using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class Search
    {
        public string[] SearchTerms { get; set; }
        public string[] Divisions { get; set; }
        public string[] RentalStatus { get; set; }
        public string[] GpsFilter { get; set; }
        public string[] LocationStatus { get; set; }
        public string[] Locations { get; set; }
        public string[] Makes { get; set; }
        public string[] Models { get; set; }
        public string[] Categories { get; set; }
        public string[] RentalReservationGroup { get; set; }
    }

    public class MachineSearch : Search
    {
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public int? MinYear { get; set; }
        public int? MaxYear { get; set; }
        public bool ForSale { get; set; }
        public string[] ServiceStatus { get; set; }
        public string[] AccountManager { get; set; }
        public string Customer { get; set; }
        public bool ShowPPE { get; set; }
    }

    public class AttachmentSearch : Search
    {
        public string[] AttachmentTypes { get; set; }
        public int MaxPrice { get; set; }
        public int MinYear { get; set; }
        public int MaxYear { get; set; }
        public AttachmentFitsOn fitsOn { get; set; }
    }

    public class AttachmentFitsOn
    {
        public string[] Makes { get; set; }
        public string[] Models { get; set; }
        public string[] Categories { get; set; }
    }

    public class CustomerSearch
    {
        public string SearchString { get; set; }
        public string CompanyName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Division { get; set; }
        public int? AccountManager { get; set; }
        public int? EmailGroupID { get; set; }//This is most likely an int.  I haven't finished all API things for back end on Group Management.
        public string NotesFrom { get; set; }
        public string NotesTo { get; set; }
        public string ContactMasterCategory { get; set; }
        public string ContactBusinessCodeCategory { get; set; }
        public int? ContactVendorClassID { get; set; }
        public bool? HasEmailOnly { get; set; }
        public bool? Active { get; set; }
    }

    public class QuotesSearch
    {
        public string SearchString { get; set; }
        public string Customer { get; set; }
        public string Division { get; set; }
        public string AccountManagerID { get; set; }
        public string Status { get; set; }
        public string Category { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int? MinYear { get; set; }
        public int? MaxYear { get; set; }
    }


    public class EquipmentOnRentSearch
    {
        public string ProductCategoryName { get; set; }
        public string ManufacturerName { get; set; }
        public string ModelNum { get; set; }
        public string SerialNum { get; set; }
        public int ManufacturedYear { get; set; }
        public int ContractNum { get; set; }
        public string Email { get; set; }
    }
}