using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class Transport : ITransport
    {
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public string ResponsibleParty { get; set; }
    }

    interface ITransport
    {
        string FromLocation { get; set; }
        string ToLocation { get; set; }
        string ResponsibleParty { get; set; }
    }

    public class Transportation
    {
        public int ShipmentID { get; set; }
        public int ShipmentInventoryID { get; set; }
        public string InventoryRelationshipType { get; set; }
        public int InventoryID { get; set; }
        public int ParentInventoryID { get; set; }
        public int ContractDtlID { get; set; }
        public string FrontAttachment { get; set; }
        public string RearAttachment { get; set; }
        public string TertiaryAttachment { get; set; }
        public int HoursOut { get; set; }
        public decimal AmountToBillCustomer { get; set; }
        public string ShipmentStatus { get; set; }
        public string ShipmentType { get; set; }
        public string PickupLocation { get; set; }
        public string DestinationLocation { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
    }

    public class ShipmentQuotes
    {
        public int ShipmentQuotesID { get; set; }
        public string ShipmentQuotesStatus { get; set; }
        public DateTime? RequestDate { get; set; }
        public DateTime? RequestedPickupDate { get; set; }
        public DateTime? RequestedDeliveryDate { get; set; }
        public DateTime? EstimatedStartDate { get; set; }
        public string RawInput { get; set; }
        public List<ShipmentQuotesInventory> ShipmentQuotesInventories { get; set; }
        public Boolean RoundTrip { get; set; }
        
    }

    public class ShipmentQuotesInventory
    {
        public int ShipmentQuotesInventoryID { get; set; }
        public int QuoteDetailID { get; set; }
        public decimal Price { get; set; }
    
    }

}