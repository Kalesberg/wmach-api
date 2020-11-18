using API.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace API.Data
{
    public partial class DAL
    {
        public bool saveAdminMetricProjections(JObject json = null)
        {
            string cmdText = ConfigurationManager.AppSettings["SaveAdminGridProjections"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return InsertData(cmdText, json);
        }

        public List<EquipmentMetrics> getEquipmentAdminMetrics(JObject json = null)
        {
            string cmdText = ConfigurationManager.AppSettings["AdminGridMetrics"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            Func<DataTable, List<EquipmentMetrics>> metricTransform = transformEquipmentMetricData;
            return getRecords<EquipmentMetrics>(cmdText, metricTransform, json);
        }

        public DataTable getReportContactsCreated()
        {
            string cmdText = ConfigurationManager.AppSettings["ContactsCreatedLastMonth"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords(cmdText);
        }

        public DataTable getServiceHistoryReport()
        {
            string cmdText = ConfigurationManager.AppSettings["GetServiceHistoryReport"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords(cmdText);
        }

        public DataTable getWorkOrderReport()
        {
            string cmdText = ConfigurationManager.AppSettings["Service_EquipmentHistoryByWO"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords(cmdText);
        }

        public DataTable getCurrentCNBV()
        {
            string cmdText = ConfigurationManager.AppSettings["CNBV"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords(cmdText);
        }

        public DataTable getReportFixedAsset()
        {
            string cmdText = ConfigurationManager.AppSettings["FixedAssetReport"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords(cmdText);
        }

        public DataTable getAvailableReports()
        {
            string cmdText = ConfigurationManager.AppSettings["AvailableReports"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords(cmdText);
        }

        public DataTable getReport(string reportName, JObject sqlParams)
        {
            throw new NotImplementedException();
        }

        public DataTable getTotalRentalRevenue(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["GetRentalRevenue"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords(cmdText, sqlParams);
        }

        public DataTable getAllTotalRentalRevenue()
        {
            string cmdText = ConfigurationManager.AppSettings["GetTotalRentalRevenue"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords(cmdText);
        }

        public List<SalesPersonRevenue> getRevenueBySalesPerson(JObject sqlParams, RevenueType revenueType)
        {
            string cmdText = ConfigurationManager.AppSettings["GetTotalRevenueBySalesPerson"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            cmd.Parameters.AddWithValue("@RevenueType", Enum.GetName(typeof(RevenueType), revenueType));
            return getRecords<SalesPersonRevenue>(cmdText, sqlParams);
        }

        public List<DivisionRevenue> getRevenueByDivision(JObject sqlParams, RevenueType revenueType)
        {
            string cmdText = ConfigurationManager.AppSettings["GetTotalRevenueByDivision"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            cmd.Parameters.AddWithValue("@RevenueType", Enum.GetName(typeof(RevenueType), revenueType));
            return getRecords<DivisionRevenue>(cmdText, sqlParams);
        }

        public TimeSeries getMonthlyRevenueBySalesperson(JObject sqlParams, RevenueType revenueType)
        {
            string cmdText = ConfigurationManager.AppSettings["GetMonthlyRevenueBySalesperson"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            cmd.Parameters.AddWithValue("@RevenueType", Enum.GetName(typeof(RevenueType), revenueType));
            Func<DataTable, TimeSeries> tsTransform = timeSeriesTransform;
            return getRecords<TimeSeries>(cmdText, tsTransform, sqlParams);
        }    

        public TimeSeries getMonthlyRevenueByDivision(JObject sqlParams, RevenueType revenueType)
        {
            string cmdText = ConfigurationManager.AppSettings["GetMonthlyRevenueByDivision"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            cmd.Parameters.AddWithValue("@RevenueType", Enum.GetName(typeof(RevenueType), revenueType));
            Func<DataTable, TimeSeries> tsTransform = timeSeriesTransform;
            return getRecords<TimeSeries>(cmdText, tsTransform, sqlParams);
        }

        public TimeSeries timeSeriesTransform(DataTable data)
        {
            if (data.Rows.Count == 0) return new TimeSeries();
            var ts = data.AsEnumerable().GroupBy(row => row["Entity"].ToString()).Select(r => new TimeSeries()
            {
                Entity = r.Key,
                Revenue = r.Select(d => new RevenueBucket(DateTime.Parse(d["Month"].ToString()), 
                                                          Decimal.Parse(d["Revenue"].ToString()))).ToList()
            });

            return ts.First();
        }

        private List<EquipmentMetrics> transformEquipmentMetricData(DataTable data)
        {
            var metrics = data.AsEnumerable().Select(r => new EquipmentMetrics
            {
                EquipmentID = Int32.Parse(r["EquipmentID"].ToString()),
                MonthlyRentalRate = Decimal.Parse(r["MonthlyRentalRate"].ToString()),
                Actual = new EquipmentMetric
                {
                    RollingAnnualAvgRentalRate = Decimal.Parse(r["RollingAnnualAvgRentalRate_ACTUAL"].ToString()),
                    RollingAnnualServiceCost = Decimal.Parse(r["RollingAnnualServiceCost_ACTUAL"].ToString()),
                    Utilization = Decimal.Parse(r["Utilization_ACTUAL"].ToString()),
                    CRC = Decimal.Parse(r["CRC_ACTUAL"].ToString()),
                    ROC = Decimal.Parse(r["ROC_ACTUAL"].ToString()),
                    ROV = Decimal.Parse(r["ROV_ACTUAL"].ToString())
                },
                Projected = new EquipmentMetric
                {
                    RollingAnnualAvgRentalRate = Decimal.Parse(r["RollingAnnualAvgRentalRate_PROJ"].ToString()),
                    RollingAnnualServiceCost = Decimal.Parse(r["RollingAnnualServiceCost_PROJ"].ToString()),
                    Utilization = Decimal.Parse(r["Utilization_PROJ"].ToString()),
                    CRC = Decimal.Parse(r["CRC_PROJ"].ToString()),
                    ROC = Decimal.Parse(r["ROC_PROJ"].ToString()),
                    ROV = Decimal.Parse(r["ROV_PROJ"].ToString())
                }
            }).ToList();

            return metrics;
        }

        private string getParamNameFromEnum(RevenueType rev)
        {
            return "@" + rev;
        }
    }
}