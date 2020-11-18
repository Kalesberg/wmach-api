using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class EquipmentMetric
    {
        public decimal RollingAnnualAvgRentalRate { get; set; }
        public decimal RollingAnnualServiceCost { get; set; }
        public decimal Utilization { get; set; }
        public decimal CRC { get; set; }
        public decimal ROC { get; set; }
        public decimal ROV { get; set; }
    }

    public class EquipmentMetrics
    {
        public int EquipmentID { get; set; }
        public decimal MonthlyRentalRate { get; set; }
        public EquipmentMetric Actual { get; set; }
        public EquipmentMetric Projected { get; set; }
    }
}