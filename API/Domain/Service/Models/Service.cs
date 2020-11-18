using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class Service
    {
        public int ID { get; set; }
        public int workOrderNum { get; set; }
        public string workOrderStatus { get; set; }
        public string workOrderCategory { get; set; }
        public int contractDtlID { get; set; }
        public int divisionID { get; set; }
        public int divisionLocationID { get; set; }
        public int assignedDivisionID { get; set; }
        public string rentalStatus { get; set; }
        public string serviceStatusIn { get; set; }
        public string serviceStatusOut { get; set; }
        public string machArrNum { get; set; }
        public string engArrNum { get; set; }
        public string tranArrNum { get; set; }
        public string engSerialNum { get; set; }
        public string tranSerialNum { get; set; }
        public string ownerType { get; set; }
        public int ownedByID { get; set; }
        public string serviceLocationType { get; set; }
        public int requestedByID { get; set; }
        public int contractorID { get; set; }
        public string contractorContact { get; set; }
        public string contractorPhone { get; set; }
        public int customerID { get; set; }
        public string customerContact { get; set; }
        public string customerPhone { get; set; }
        public string fieldServiceLocation { get; set; }
        public int serviceManagerID { get; set; }
        public string comment { get; set; }
        public bool createCheckOut { get; set; }
        public string shopChargeType { get; set; }
        public string billTo { get; set; }
        public string serviceKind { get; set; }
        public string taxScheduleID { get; set; }
        public int salesManagerID { get; set; }
        public bool canBeBilled { get; set; }
        public DateTime createdDate { get; set; }
    }

    public class Checkout
    {
        public string CompanyName { get; set; }
        public int ContractID { get; set; }
        public string SerialNum { get; set; }
        public int InspectionHours { get; set; }
        public int ServiceManagerID { get; set; }
        public string Make { get; set; }
        public string ModelNum { get; set; }
        public string ProductType { get; set; }
        public int ManufacturedYear { get; set; }
        public string ServiceStatus { get; set; }
        public string DivisionShortName { get; set; }
        public string JobsiteCity { get; set; }
        public string JobsiteState { get; set; }
        public DateTime EstimatedStartDate { get; set; }
        public string ServiceManagerName { get; set; }
        public string Email { get; set; }
        public string BusinessPhone { get; set; }

    }
}