using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class Contract
    {
        public int ContractNum { get; set; }
        public decimal? CRC { get; set; }
        public string CompanyStateCode { get; set; }
        public string CompanyCity { get; set; }
        public string CompanyStreet { get; set; }
        public string AccountManagerFirstName { get; set; }
        public string AccountManagerLastName { get; set; }
        public string RentalCoordinatorFirstName { get; set; }
        public string RentalCoordinatorLastName { get; set; }
        public string ContractOwner { get; set; }
        public string CustomerReferenceNumber { get; set; }
        public string RentingCompany { get; set; }
        public string CustomerName { get; set; }
        public string CustomerBusinessPhone { get; set; }
        public string CustomerMobilePhone { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerStateCode { get; set; }
        public string CustomerCity { get; set; }
        public string CustomerStreet { get; set; }
        public decimal ContractRentalRate { get; set; }
        public string ContractMinRentalTerm { get; set; }
        public string ContractRentalPeriod { get; set; }
        public decimal ContractRPOPrice { get; set; }
        public int ContractRentalRateCollectedToDate { get; set; }
        public int StartHours { get; set; }
        public decimal RentCollectedToDate { get; set; }
        public DateTime? ContractStartBillDate { get; set; }
        public DateTime? ContractActualStartDate { get; set; }
        public DateTime? ContractEstimatedEndDate { get; set; }
        public DateTime? ContractEstimatedStartDate { get; set; }
    }

    public class ContractView
    {
        public int ContractID { get; set; }
        public int ContractNum { get; set; }
        public string ContractType { get; set; }
        public string ContractFormat { get; set; }
        public int CustomerContactID { get; set; }
        public int SalesmanContactID { get; set; }
        public int RentalCoordinatorID { get; set; }
        public int ServiceManagerID { get; set; }
        public int WWMDivisionID { get; set; }
        public string ContractStatus { get; set; }
        public int CustomerMainOfficeAddressID { get; set; }
        public int CustomerBillingAddressID { get; set; }
        public string BillingContact { get; set; }
        public string BillingPhone { get; set; }
        public string BillingFax { get; set; }
        public string BillingEmail { get; set; }
        public string JobSiteContact { get; set; }
        public int JobSiteAddressID { get; set; }
        public string JobSitePhone { get; set; }
        public string JobSiteMobile { get; set; }
        public string JobSiteFax { get; set; }
        public string FobCity { get; set; }
        public string FobState { get; set; }
        public string SpecialProvisions { get; set; }
        public DateTime? ContractExecutedDate { get; set; }
        public string RentalPeriodStartOption { get; set; }
        public string RentalPeriodStopOption { get; set; }
        public DateTime? EstimatedStartDate { get; set; }
        public DateTime? EstimatedShipDate { get; set; }
        public string ReferenceNumberCustomer { get; set; }
        public string TransportationResponsibility { get; set; }
        public string TransportToArrangedBy { get; set; }
        public string TransportToBilledTo { get; set; }
        public string TransportFromArrangedBy { get; set; }
        public string TransportFromBilledTo { get; set; }
        public string CancelledPerson { get; set; }
        public string CancelledReason { get; set; }
        public DateTime? ReadOnlyStartDateOfFirstItem { get; set; }
        public int ReadOnlyNumberOfMachines { get; set; }
        public int ReadOnlyNumberOfAttachments { get; set; }
        public string EnterUserStr { get; set; }
        public DateTime EnterDateTime { get; set; }
        public string EditUserStr { get; set; }
        public DateTime EditDateTime { get; set; }
        public string JobSiteEmail { get; set; }
    }


    public class ContractDetails
    {
        public int ContractDtlID { get; set; }
        public int ContractID { get; set; }
        public int EquipmentID { get; set; }
        public int StartHours { get; set; }
        public int StopHours { get; set; }
        public decimal RentalRate { get; set; }
        public int MinFeetPerMonth { get; set; }
        public int OverageHourLimit { get; set; }
        public decimal OverageHourFee { get; set; }
        public int OveragePercentageHourFee { get; set; }
        public decimal InsuranceValue { get; set; }
        public decimal RentalPurchaseOptionPrice { get; set; }
        public decimal TransportationRate { get; set; }
        public string TransportationRateType { get; set; }
        public string RentalType { get; set; }
        public int RentalPeriod { get; set; }
        public string RentalPeriodTimeSpan { get; set; }
        public DateTime EstimatedStartDate { get; set; }
        public DateTime ActualStartDate { get; set; }
        public DateTime EstimatedEndDate { get; set; }
        public DateTime ActualEndDate { get; set; }
        public string SwapReason { get; set; }
        public string SwapReasonAdditionalInfo { get; set; }
        public string SwapTransportationToSite { get; set; }
        public string SwapTransportationFromSite { get; set; }
        public bool SwapIsOriginalEquipment { get; set; }
        public int SwappedWithID { get; set; }
        public DateTime SwappedDate { get; set; }
        public string EnterUserStr { get; set; }
        public DateTime EnterDateTime { get; set; }
        public string EditUserStr { get; set; }
        public DateTime EditDateTime { get; set; }
        public Equipment Equipment { get; set; }
        public List<Equipment> Attachments { get; set; }
        //public List<ContractDetailAttachment> ContractDetailAttachments { get; set; }
        public IEnumerable<Service> Services { get; set; }
        public IEnumerable<Transportation> Transportations { get; set; }
        public Model ModelDetail { get; set; }
    }
    public class ContractDetailAttachment
    {
        public int ContractDtlAttachmentID { get; set; }
        public int ContractDtlID { get; set; }
        public int AttachmentID { get; set; }
        public string AttachmentPosition { get; set; }
            
    }

    public class ContractSignature
    {
        public string LoginEmail { get; set; }
        public string Name { get; set; }
        public int ContractID { get; set; }
        public int CompanyID { get; set; }
        public string JobTitle { get; set; }
        public string Image { get; set; }
        public string ImageType { get; set; }
        public string SignatureImage { get; set; }
        public string IP { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string RegionCode { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string TimeZone { get; set; }
        public int Latitude { get; set; }
        public int Longitude { get; set; }
        public int MetroCode { get; set; }
        public DateTime Date { get; set; }
    }
    public class ContractListCustomerPortal
    {
        public int ContractID { get; set; }
        public int ContractNum { get; set; }
        public string ContractType { get; set; }
        public string ContractStatus { get; set; }
        public string DivisionName { get; set; }
        public DateTime ContractExecutedDate { get; set; }
        public DateTime EstimatedStartDate { get; set; }
        public int MachineTotal { get; set; }
        public string Contactperson { get; set; }
        public string Jobsite { get; set; }
        public string Salesperson { get; set; }
        public string SalespersonEmail { get; set; }
        public string SalespersonPhone { get; set; }
        public string Coordinator { get; set; }
        public string CoordinatorEmail { get; set; }
        public string CoordinatorPhone { get; set; }
        public string ServiceManager { get; set; }
        public string ServiceManagerEmail { get; set; }
        public string ServiceManagerPhone { get; set; }
        public int SignStatus { get; set; }
    }


    public class JobInfoSheet
    {
     public int   ContractNum{ get; set; }
     public int ContractID { get; set; }
      public string  JobSheetStatus{ get; set; }
     public string   CustomerType{ get; set; }
     public DateTime   EstRentalStartDate{ get; set; }
     public string   ProjectSite{ get; set; }
     public string JobSiteContactName { get; set; }
     public string   JobSiteContactPhone{ get; set; }
     public Address JobSiteAddress { get; set; }
     public string   JobSitePhone{ get; set; }
     public string   JobSiteFax{ get; set; }
     public string   JobSiteEmail{ get; set; }
      public string  PropertyOnwerContactName{ get; set; }
     public string   PropertyOnwerContactPhone{ get; set; }
     public Address PropertyOnwerAddress { get; set; }
     public string   GeneralContractorContactName{ get; set; }
     public string   GeneralContractorContactPhone{ get; set; }
     public Address GeneralContractorAddress { get; set; }
     public string   TypeOfConstruction{ get; set; }
     public string   NameOfAgent{ get; set; }
     public string   AgentPhone{ get; set; }
     public string   Surety{ get; set; }
     public string   Amount{ get; set; }
     public string   BondNum{ get; set; }
     public string   UtahSRCNum{ get; set; }
     public string   Comment{ get; set; }
    }

}