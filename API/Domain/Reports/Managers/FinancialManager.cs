using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models;
using Newtonsoft.Json.Linq;
using API.Data;
using API.Data.Builders;

namespace API.Managers
{
    public  static class FinancialManager
    {
        public static TimeSeries GetRentalRevenue(ReportingEntity entity, JObject sqlParams)
        {
            switch (entity)
            {
                case ReportingEntity.Division:
                    return Builder.Build(new RentalRevenueBySingleDivision(), sqlParams);
                case ReportingEntity.Salesperson:
                    return Builder.Build(new RentalRevenueBySingleSalesperson(), sqlParams);
                default:
                    return new TimeSeries();
            }
        }

        public static TimeSeries GetSalesRevenue(ReportingEntity entity, JObject sqlParams)
        {
            switch (entity)
            {
                case ReportingEntity.Division:
                    return Builder.Build(new SalesRevenueBySingleDivision(), sqlParams);
                case ReportingEntity.Salesperson:
                    return Builder.Build(new SalesRevenueBySingleSalesperson(), sqlParams);
                default:
                    return new TimeSeries();
            }
        }

        public static TimeSeries GetTotalRevenue(ReportingEntity entity, JObject sqlParams)
        {
            switch (entity)
            {
                case ReportingEntity.Division:
                    return Builder.Build(new TotalRevenueBySingleDivision(), sqlParams);
                case ReportingEntity.Salesperson:
                    return Builder.Build(new TotalRevenueBySingleSalesperson(), sqlParams);
                default:
                    return new TimeSeries();
            }
        }
    }
}