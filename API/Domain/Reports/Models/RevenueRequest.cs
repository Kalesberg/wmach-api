using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class RevenueRequest
    {
        public string Division { get; set; }
        public string Customer { get; set; }
        public string SalesPerson { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class EntityRevenueRequest
    {
        public ReportingEntity Entity { get; set; }
        public string Filter { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public struct DateSelector
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class DivisionCompareRequest
    {
        public string Division1 { get; set; }
        public string Division2 { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class SalesPersonCompareRequest
    {
        public string SalesPerson1 { get; set; }
        public string SalesPerson2 { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
