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
        public IEnumerable<Division> getDivisionsHierarchy(JObject sqlParams = null, string EquipmentType = null)
        {
            string cmdText = ConfigurationManager.AppSettings["DivisionHierarchy"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            Func<DataTable, List<Division>> divisionTransform = getDivisionsStructured;
            return getRecords<Division>(cmdText, divisionTransform, sqlParams);
        }

        public IEnumerable<string> getDivisions(JObject sqlParams = null, string EquipmentType = null)
        {
            string cmdText = ConfigurationManager.AppSettings["DivisionFilters"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<string>(cmdText, sqlParams);
        }

        public List<string> GetTaxCodes(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["GpTaxSchedules"];
            if (string.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<string>(cmdText, sqlParams);
        }

        private List<Division> getDivisionsStructured(DataTable data)
        {
            var divisions = data.AsEnumerable().GroupBy(row => row["parentName"].ToString()).Select(r => new Division()
            {
                parent = r.Key,
                children = r.Select(d => d["childName"].ToString()).ToList()
            }).ToList();

            foreach (var division in divisions)
            {
                if (division.children.Count == 1 && String.IsNullOrWhiteSpace(division.children[0]))
                {
                    division.children = new List<String>();
                }
            }

            return divisions;
        }
        public IEnumerable<string> getSalesContactIDByDivision(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["DivisionSaleContact"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<string>(cmdText, sqlParams);
        }

        public IEnumerable<string> getDivisionBySalesPersonContactID(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["DivisionBySalesPerson"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<string>(cmdText, sqlParams);
        }

        public DivisionDetail getDivisionDetailByDivisionID(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["DivisionDetailByDivisionID"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<DivisionDetail>(cmdText, sqlParams).FirstOrDefault();
        }
    }
}