using API.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Data.Builders
{
    #region Divisions

    public class RentalRevenueByDivisions : IBuildParams<List<DivisionRevenue>>
    {
        private List<DivisionRevenue> Divisions = new List<DivisionRevenue>();
        public void Build(JObject sqlParams)
        {
            Divisions = DAL.GetInstance().getRevenueByDivision(sqlParams, RevenueType.Rental);
        }

        public List<DivisionRevenue> GetResult()
        {
            return Divisions;
        }
    }

    public class SalesRevenueByDivisions : IBuildParams<List<DivisionRevenue>>
    {
        private List<DivisionRevenue> Divisions = new List<DivisionRevenue>();
        public void Build(JObject sqlParams)
        {
            Divisions = DAL.GetInstance().getRevenueByDivision(sqlParams, RevenueType.Sales);
        }

        public List<DivisionRevenue> GetResult()
        {
            return Divisions;
        }
    }

    public class TotalRevenueByDivisions : IBuildParams<List<DivisionRevenue>>
    {
        private List<DivisionRevenue> Divisions = new List<DivisionRevenue>();
        public void Build(JObject sqlParams)
        {
            Divisions = DAL.GetInstance().getRevenueByDivision(sqlParams, RevenueType.Total);
        }

        public List<DivisionRevenue> GetResult()
        {
            return Divisions;
        }
    }

    public class RentalRevenueBySingleDivision : IBuildParams<TimeSeries>
    {
        private TimeSeries Division = new TimeSeries();
        public void Build(JObject sqlParams)
        {
            Division = DAL.GetInstance().getMonthlyRevenueByDivision(sqlParams, RevenueType.Rental);
        }

        public TimeSeries GetResult()
        {
            return Division;
        }
    }

    public class SalesRevenueBySingleDivision : IBuildParams<TimeSeries>
    {
        private TimeSeries Division = new TimeSeries();
        public void Build(JObject sqlParams)
        {
            Division = DAL.GetInstance().getMonthlyRevenueByDivision(sqlParams, RevenueType.Sales);
        }

        public TimeSeries GetResult()
        {
            return Division;
        }
    }

    public class TotalRevenueBySingleDivision : IBuildParams<TimeSeries>
    {
        private TimeSeries Division = new TimeSeries();
        public void Build(JObject sqlParams)
        {
            Division = DAL.GetInstance().getMonthlyRevenueByDivision(sqlParams, RevenueType.Total);
        }

        public TimeSeries GetResult()
        {
            return Division;
        }
    }

    #endregion

    #region Salespeople

    public class RentalRevenueBySingleSalesperson : IBuildParams<TimeSeries>
    {
        private TimeSeries Salesperson = new TimeSeries();
        public void Build(JObject sqlParams)
        {
            Salesperson = DAL.GetInstance().getMonthlyRevenueBySalesperson(sqlParams, RevenueType.Rental);
        }

        public TimeSeries GetResult()
        {
            return Salesperson;
        }
    }

    public class SalesRevenueBySingleSalesperson : IBuildParams<TimeSeries>
    {
        private TimeSeries Salesperson = new TimeSeries();
        public void Build(JObject sqlParams)
        {
            Salesperson = DAL.GetInstance().getMonthlyRevenueBySalesperson(sqlParams, RevenueType.Sales);
        }

        public TimeSeries GetResult()
        {
            return Salesperson;
        }
    }

    public class TotalRevenueBySingleSalesperson : IBuildParams<TimeSeries>
    {
        private TimeSeries Salesperson = new TimeSeries();
        public void Build(JObject sqlParams)
        {
            Salesperson = DAL.GetInstance().getMonthlyRevenueBySalesperson(sqlParams, RevenueType.Total);
        }

        public TimeSeries GetResult()
        {
            return Salesperson;
        }
    }

    public class RentalRevenueBySalesperson : IBuildParams<List<SalesPersonRevenue>>
    {
        private List<SalesPersonRevenue> SalesPeople = new List<SalesPersonRevenue>();
        public void Build(JObject sqlParams)
        {
            SalesPeople = DAL.GetInstance().getRevenueBySalesPerson(sqlParams, RevenueType.Rental);
        }

        public List<SalesPersonRevenue> GetResult()
        {
            return SalesPeople;
        }
    }

    public class SalesRevenueBySalesperson : IBuildParams<List<SalesPersonRevenue>>
    {
        private List<SalesPersonRevenue> SalesPeople = new List<SalesPersonRevenue>();
        public void Build(JObject sqlParams)
        {
            SalesPeople = DAL.GetInstance().getRevenueBySalesPerson(sqlParams, RevenueType.Sales);
        }

        public List<SalesPersonRevenue> GetResult()
        {
            return SalesPeople;
        }
    }

    public class TotalRevenueBySalesperson : IBuildParams<List<SalesPersonRevenue>>
    {
        private List<SalesPersonRevenue> SalesPeople = new List<SalesPersonRevenue>();
        public void Build(JObject sqlParams)
        {
            SalesPeople = DAL.GetInstance().getRevenueBySalesPerson(sqlParams, RevenueType.Total);
        }

        public List<SalesPersonRevenue> GetResult()
        {
            return SalesPeople;
        }
    }

    #endregion
}