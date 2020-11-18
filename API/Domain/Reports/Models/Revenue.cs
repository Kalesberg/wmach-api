using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    //RETURNS REVENUE GROUPBED BY DIVISION
    public class DivisionRevenue
    {
        public string Division { get; set; }
        public decimal Revenue { get; set; }
    }

    //RETURNS REVENUE GROUPED BY SALESPERSON
    public class SalesPersonRevenue
    {
        public string SalesPerson { get; set; }
        public decimal Revenue { get; set; }
    }

    //REUSABLE CLASS FOR TIME SERIES FINANCIAL DATA
    public class TimeSeries
    {
        public string Entity { get; set; }
        public List<RevenueBucket> Revenue { get; set; }
    }

    //REUSABLE CLASS FOR HOLDING A FINANCIAL FIGURE
    public class RevenueBucket
    {
        public DateTime Date { get; set; }
        public Decimal Value { get; set; }

        public RevenueBucket(DateTime date, Decimal value)
        {
            this.Date = date;
            this.Value = value;
        }
    }

    public enum ReportingEntity
    {
        Division,
        Salesperson
    }

    public enum RevenueType
    {
        Rental,
        Sales,
        Total
    }
}