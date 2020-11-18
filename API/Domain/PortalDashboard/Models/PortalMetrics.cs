using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class PortalMetrics
    {
        public Metrics Metrics { get; set; }
    }

    public class Metrics
    {
        public int Active { get; set; }
        public int New { get; set; }
        public int Waiting { get; set; }
        public Agreements Agreements { get; set; }
        public Quotes Quotes { get; set; }
    }

    public class Agreements
    {
        public int OpenContract{ get; set; }
        public int PendingContract { get; set; }
        public int SignedContract { get; set; }
    }

    public class Quotes
    {
        public int OpenQuotes { get; set; }
        public int NewQuotes { get; set; }
        public int AprovedQuotes { get; set; }
    }

    public class LatestQuotes
    {
        public int QuoteNumber { get; set; }
        public int QuoteID { get; set; }
        public string Customer { get; set; }
        public DateTime CreatedDate { get; set; }
        public int MachineQty { get; set; }
        public string CreatedBy { get; set; }
        public bool Transportation { get; set; }
        public bool PM { get; set; }
    }

    public class PortalAccessRequest
    {
        public DateTime RequestDate { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Industry { get; set; }
        public string Country { get; set; }
    }

    public class TransportationRequests
    {
        public int QuoteNo { get; set; }
        public int QuoteID { get; set; }
        public string Customer { get; set; }
        public int MachineQty { get; set; }
    }

    public class OverdueInvoices
    {
        public string CustomerID { get; set; }
        public string Customer { get; set; }
        public decimal Total { get; set; }
        public int DaysOverdue { get; set; }
    }

    public class SequenceTracking
    {
        public int AssetID { get; set; }
        public string SerialNum { get; set; }
        public int AssetSequence { get; set; }
        public string  AssetOwner { get; set; }
        public DateTime? SequenceAcquisitionDate { get; set; }
        public DateTime? SequenceSoldDate { get; set; }
        public decimal SequenceAquisitionCost { get; set; }
        public DateTime? AuditDate { get; set; }
    }

    /// <summary>
    /// Pending Contract Model
    /// </summary>
    public class PendinContractDetails
    {
        public int ContractID { get; set; }
        public int ContractNum { get; set; }
        public string ContractFormat { get; set; }
        public string SalesPerson { get; set; }
        public string SalesPersonEmail { get; set; }
        public string SalesPersonPhone { get; set; }
        public string JobSite { get; set; }
        public string Coordinator { get; set; }
        public string CoordinatorPhone { get; set; }
        public string CoordinatorEmail { get; set; }
        public string ServiceManager { get; set; }
        public string ServiceManagerPhone { get; set; }
        public string ServiceManagerEmail { get; set; }
        public string Division { get; set; }
        public string CustomerName { get; set; }
        public string CustomerContact { get; set; }
        public string ContractStatus { get; set; }
        public DateTime? EstimatedStartDate { get; set; }
        [Display(Name = "No. of Machine")]
        public int ReadOnlyNumberOfMachines { get; set; }
        [Display(Name = "No. of Attachments")]
        public int ReadOnlyNumberOfAttachments { get; set; }
    }

    public class QuoteDetailsPortal
    {
        public int QuoteID { get; set; }
        public int QuoteNumber { get; set; }
        public string QuoteType { get; set; }
        public string MinTerm { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string Division { get; set; }
        public DateTime? QuoteDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? EstStartDate { get; set; }
        public string Jobsite { get; set; }
        public int Machines { get; set; }
        public string SalesRep { get; set; }
        public string SalesRepEmail { get; set; }
        public string SalesRepPhone { get; set; }
        public string AccountManager { get; set; }
        public string Coordinator { get; set; }
        public string CoordinatorPhone { get; set; }
        public string CoordinatorEmail { get; set; }
        public decimal RPOPrice { get; set; }
        public decimal RPOTerm { get; set; }
    }
}
