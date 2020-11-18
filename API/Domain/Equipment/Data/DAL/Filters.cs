using API.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace API.Data
{
    public partial class DAL
    {
        public Dictionary<string, Dictionary<string, List<string>>> getAllFilters()
        {
            string cmdText = ConfigurationManager.AppSettings["AllFilters"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getFilters(cmdText);
        }

        private Dictionary<string, Dictionary<string, List<string>>> getFilters(string cmdText)
        {
            var filters = new Dictionary<string, Dictionary<string, List<string>>>();
            if (sqlConn.State == ConnectionState.Closed) sqlConn.Open();
            cmd.Connection = sqlConn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = cmdText;

            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var key = rdr.GetString(0);
                    var val = rdr.GetString(1);
                    var type = rdr.GetString(2);

                    if (filters.ContainsKey(key))
                    {
                        if (filters[key].ContainsKey(type))
                        {
                            filters[key][type].Add(val);
                        }
                        else
                        {
                            var subFilters = new List<string>();
                            subFilters.Add(val);
                            filters[key].Add(type, subFilters);
                        }
                    }
                    else
                    {
                        var filterGroup = new Dictionary<string, List<string>>();
                        var filterType = new List<string>();
                        filterType.Add(val);
                        filterGroup.Add(type, filterType);
                        filters.Add(key, filterGroup);
                    }
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                if (sqlConn.State != ConnectionState.Closed)
                    sqlConn.Close();

                cmd.Parameters.Clear();
            }

            return filters;
        }
    }
}