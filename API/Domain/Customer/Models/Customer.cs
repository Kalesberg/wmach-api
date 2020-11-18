using System;
using System.Collections.Generic;

namespace API.Models
{
    public class Customer
    {
        public int contactId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string companyName { get; set; }
        public int numOpenContracts { get; set; }
        public int numMachinesOnRent { get; set; }
        public string creditDesignation { get; set; }
        public string gpClass { get; set; }
        public string accountManager { get; set; }
        public bool hasCPAccess { get; set; }
    }

    public class SalesmanInfo
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Direct { get; set; }
        public string TollFreeNumber { get; set; }

    }

    public class CustomerDetails
    {
        public int contactId { get; set; }
        public string company { get; set; }
        public string contactName { get; set; }
        public string cellNum { get; set; }
        public string assignedRep { get; set; }
        public string assignedRepPhone { get; set; }
        public string assignedRepEmail { get; set; }
        public string assignedRepDivision { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public int assignedRepID { get; set; }
        public List<ContactAddress> address { get; set; }
        public List<string> businessPhones { get; set; }
        public List<string> emailAddresses { get; set; }
        public List<CustomerContractQuoteSummary> contractsAndQuotes { get; set; }
    }

    public class CustomerContractQuoteSummary
    {
        public string Type { get; set; }
        public int QuoteOrContractID { get; set; }
        public int QuoteOrContractNum { get; set; }
        public DateTime? QuoteContractDate { get; set; }
        public string SummaryString { get; set; }
        public string JobLocation { get; set; }
    }


    public class Invoice
    {
        public int InvoiceNum { get; set; }
        public DateTime INVODATE { get; set; }
        public DateTime DUEDATE { get; set; }
        public string DocType { get; set; }
        public string InvoiceType { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal BalanceDue { get; set; }
        public string CompanyName { get; set; }
        public ContactAddress BillingAddress { get; set; }
        public string Division { get; set; }
        public string PONum { get; set; }
        public int ContractNum { get; set; }
        public string InvoiceStatus { get; set; }
        public ContactAddress Jobsite { get; set; }
        public Contact AccountManager { get; set; }
        public Contact RentalCoordinator { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public int AccountManagerID { get; set; }
        public ContactAddress DivisionAddress { get; set; }

    }

    public class InvoiceDetail
    {
        public string InvoiceNumber { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal InvoiceAmount { get; set; }
        public string ItemDescription { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal ItemTotalPrice { get; set; }
        public string ItemComment1 { get; set; }
        public string ItemComment2 { get; set; }
        public string ItemComment3 { get; set; }
        public string ItemComment4 { get; set; }
        public string ItemDetailDecription { get; set; }
    }

    public class QuoteRequest
    {
        public int CustomerQuoteRequestID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Company { get; set; }
        public string Message { get; set; }
        public List<QuoteRequestEquipment> Machines { get; set; }
    }

    public class QuoteRequestEquipment
    {
        public int CustomerQuoteRequestEquipmentID { get; set; }
        public int CustomerQuoteRequestID { get; set; }
        public int ID { get; set; }
        public decimal? Price { get; set; }

    }
    ///<summary>
    ///Contact Insurance Model
    ///</summary>
    public class ContactInsuranceAndCreditStatus
    {
        public DateTime? InsuranceExpirationDate { get; set; }
        public string InsuranceStatus { get; set; }
        public DateTime? CreditExpirationDate { get; set; }
        public string CreditStatus { get; set; }
    }
}