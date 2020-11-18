using System;

namespace API.Models
{
    public class ServiceSpecific
    {
        public int ID { get; set; }
        public int workOrderNum { get; set; }
        public string workOrderStatus { get; set; }
        public string workOrderCategory { get; set; }
        public int contractDtlID { get; set; }
        public string division { get; set; }
        public string divisionLocationName { get; set; }
        public string assignedDivision { get; set; }
        public string rentalStatus { get; set; }
        public string serviceStatusIn { get; set; }
        public string serviceStatusOut { get; set; }
        public string machArrNum { get; set; }
        public string engArrNum { get; set; }
        public string tranArrNum { get; set; }
        public string engSerialNum { get; set; }
        public string tranSerialNum { get; set; }
        public string ownerType { get; set; }
        public string ownedBy { get; set; }
        public string serviceLocationType { get; set; }
        public string requestedByName { get; set; }
        public string contractorCompanyName { get; set; }
        public string contractorContact { get; set; }
        public string contractorPhone { get; set; }
        public string customerCompanyName { get; set; }
        public string customerContact { get; set; }
        public string customerPhone { get; set; }
        public string fieldServiceLocation { get; set; }
        public string serviceManager { get; set; }
        public string comment { get; set; }
        public bool createCheckOut { get; set; }
        public string shopChargeType { get; set; }
        public string billTo { get; set; }
        public string serviceKind { get; set; }
        public string taxScheduleID { get; set; }
        public string salesManager { get; set; }
        public bool canBeBilled { get; set; }
        public DateTime createdDate { get; set; }
    }
}