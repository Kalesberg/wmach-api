using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using API.Models;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.IO;
using System.Dynamic;

namespace API.Data
{
    public enum DB
    {
        Mach1,
        IssueTrack
    }

    public partial class DAL
    {
        private string mach1Conn = ConfigurationManager.ConnectionStrings["mach1"].ConnectionString;
        private string issueTrackConn = ConfigurationManager.ConnectionStrings["issuetrack"].ConnectionString;
        private SqlConnection sqlConn;
        private SqlTransaction sqltran;
        private SqlCommand cmd;

        public static DAL GetInstance(DB database = DB.Mach1)
        {
            return new DAL(database);
        }

        public DAL(DB database)
        {
            var conn = database == DB.Mach1 ? mach1Conn : issueTrackConn;
            sqlConn = new SqlConnection(conn);
            cmd = new SqlCommand();
        }


        private int InsertRecord(string cmdText, JObject sqlParams = null)
        {
            if (sqlParams != null) addSqlParams(sqlParams);

            var returnVal = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
            returnVal.Direction = ParameterDirection.ReturnValue;

            try
            {
                if (sqlConn.State == ConnectionState.Closed) sqlConn.Open();

                cmd.Connection = sqlConn;
                cmd.CommandText = cmdText;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (sqlConn.State != ConnectionState.Closed)
                    sqlConn.Close();

                cmd.Parameters.Clear();
            }

            return (int)returnVal.Value;
        }

        private bool InsertData(string cmdText, JObject sqlParams = null)
        {
            if (sqlParams != null) addSqlParams(sqlParams);
            try
            {
                if (sqlConn.State == ConnectionState.Closed) sqlConn.Open();
                cmd.Connection = sqlConn;
                cmd.CommandText = cmdText;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                return false;
            }
            finally
            {
                if (sqlConn.State != ConnectionState.Closed)
                    sqlConn.Close();

                cmd.Parameters.Clear();
            }

            return true;
        }

        #region UPDATE METHODS

        private bool UpdateRecord(string cmdText, JObject sqlParams = null)
        {
            if (sqlParams != null) addSqlParams(sqlParams);

            try
            {
                if (sqlConn.State == ConnectionState.Closed) sqlConn.Open();

                cmd.Connection = sqlConn;
                cmd.CommandText = cmdText;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (sqlConn.State != ConnectionState.Closed)
                    sqlConn.Close();

                cmd.Parameters.Clear();
            }

            return true;
        }

        #endregion

        #region READ METHODS

        //PRIMARY DB CALL METHODS
        private List<T> getRecords<T>(string cmdText, JObject sqlParams = null)
        {
            DataTable data = new DataTable("data");
            if (sqlParams != null) addSqlParams(sqlParams);
            try
            {
                if (sqlConn.State == ConnectionState.Closed) sqlConn.Open();
                cmd.Connection = sqlConn;
                cmd.CommandText = cmdText;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 60000;
                var adapter = new SqlDataAdapter(cmd);
                adapter.Fill(data);
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                if (sqlConn.State != ConnectionState.Closed)
                    sqlConn.Close();

                cmd.Parameters.Clear();
            }
            return data.ConvertToList<T>();
        }

        //FOR DATA THAT REQUIRES TRANSFORM LOGIC. TAKES CALLBACK TO TRANSFORM DATA.
        private List<T> getRecords<T>(string cmdText, Func<DataTable, List<T>> transform, JObject sqlParams = null)
        {
            DataTable data = new DataTable("data");
            if (sqlParams != null) addSqlParams(sqlParams);
            try
            {
                if (sqlConn.State == ConnectionState.Closed) sqlConn.Open();
                cmd.Connection = sqlConn;
                cmd.CommandText = cmdText;
                cmd.CommandType = CommandType.StoredProcedure;
                var adapter = new SqlDataAdapter(cmd);
                adapter.Fill(data);
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                if (sqlConn.State != ConnectionState.Closed)
                    sqlConn.Close();

                cmd.Parameters.Clear();
            }

            //return the result of the transform
            return transform(data);
        }

        private T getRecords<T>(string cmdText, Func<DataTable, T> transform, JObject sqlParams = null)
        {
            DataTable data = new DataTable("data");
            if (sqlParams != null) addSqlParams(sqlParams);
            try
            {
                if (sqlConn.State == ConnectionState.Closed) sqlConn.Open();
                cmd.Connection = sqlConn;
                cmd.CommandText = cmdText;
                cmd.CommandType = CommandType.StoredProcedure;
                var adapter = new SqlDataAdapter(cmd);
                adapter.Fill(data);
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                if (sqlConn.State != ConnectionState.Closed)
                    sqlConn.Close();

                cmd.Parameters.Clear();
            }

            //return the result of the transform
            return transform(data);
        }

        private DataTable getRecords(string cmdText, JObject sqlParams = null)
        {
            DataTable data = new DataTable("data");
            if (sqlParams != null) addSqlParams(sqlParams);
            try
            {
                if (sqlConn.State == ConnectionState.Closed) sqlConn.Open();
                cmd.Connection = sqlConn;
                cmd.CommandText = cmdText;
                cmd.CommandType = CommandType.StoredProcedure;
                var adapter = new SqlDataAdapter(cmd);
                adapter.Fill(data);
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                if (sqlConn.State != ConnectionState.Closed)
                    sqlConn.Close();

                cmd.Parameters.Clear();
            }

            //return the result of the transform
            return data;
        }

        private bool ExecuteBooleanScalar(string cmdText, object arg)
        {
            var val = new Object();

            try
            {
                if (sqlConn.State == ConnectionState.Closed) sqlConn.Open();

                cmd.Connection = sqlConn;
                cmd.CommandText = String.Format("SELECT {0}('{1}')", cmdText, arg);
                cmd.CommandType = CommandType.Text;
                val = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (sqlConn.State != ConnectionState.Closed)
                    sqlConn.Close();

                cmd.Parameters.Clear();
            }

            return (Boolean)val;
        }

        private int ExecuteIntegerScalar(string cmdText, object arg)
        {
            var val = new Object();

            try
            {
                if (sqlConn.State == ConnectionState.Closed) sqlConn.Open();

                cmd.Connection = sqlConn;
                cmd.CommandText = String.Format("SELECT {0}('{1}')", cmdText, arg);
                cmd.CommandType = CommandType.Text;
                val = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                if (sqlConn.State != ConnectionState.Closed)
                    sqlConn.Close();

                cmd.Parameters.Clear();
            }

            return (int)val;
        }

       

        private DataSet getDataSetEquipmentAdmin(string cmdText, JObject sqlParams)
        {
            DataTable data = new DataTable("Equipment_Search");
            if (sqlParams != null) addSqlParamsEquipmentAdmin(sqlParams);
            try
            {
                if (sqlConn.State == ConnectionState.Closed) sqlConn.Open();
                cmd.Connection = sqlConn;
                cmd.CommandText = cmdText;
                cmd.CommandType = CommandType.StoredProcedure;
                var adapter = new SqlDataAdapter(cmd);
                adapter.Fill(data);
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                if (sqlConn.State != ConnectionState.Closed)
                    sqlConn.Close();

                cmd.Parameters.Clear();
            }
            DataSet ds = new DataSet();
            ds.Tables.Add(data);
            return ds;
        }

        #endregion

        #region SQL PARAM HELPER METHODS

        //TAKE JOBJECT AND TURN INTO SQLPARAMCOLLECTION
        private void addSqlParams(JObject sqlParams)
        {
            DataSet data = new DataSet("params");
            foreach (var token in sqlParams)
            {
                DataTable table = null;
                switch (token.Value.Type)
                {
                    case JTokenType.Array:
                        table = getDataTableParam(token);
                        if(table.Rows != null && table.Rows.Count > 0)
                            data.Tables.Add(table);
                        break;
                    case JTokenType.Integer:
                        cmd.Parameters.AddWithValue("@" + token.Key, long.Parse(token.Value.ToString()));
                        break;
                    case JTokenType.String:
                        cmd.Parameters.AddWithValue("@" + token.Key, token.Value.ToString());
                        break;
                    case JTokenType.Boolean:
                        cmd.Parameters.AddWithValue("@" + token.Key, Boolean.Parse(token.Value.ToString()));
                        break;
                    case JTokenType.Date:
                        cmd.Parameters.AddWithValue("@" + token.Key, DateTime.Parse(token.Value.ToString()));
                        break;
                    case JTokenType.Object:
                        foreach (var element in token.Value)
                        {
                            table = getDataTableParam(element);
                            data.Tables.Add(table);
                        }
                        break;
                    case JTokenType.Float:
                        cmd.Parameters.AddWithValue("@" + token.Key, float.Parse(token.Value.ToString()));
                        break; 
                }
            }

            //ADD DATATABLES INTO PARAM COLLECTION
            foreach (DataTable table in data.Tables)
                cmd.Parameters.AddWithValue("@" + table.TableName, table);
        }

        private void addSqlParams(JObject sqlParams, SqlCommand cmd)
        {
            DataSet data = new DataSet("params");
            foreach (var token in sqlParams)
            {
                DataTable table = null;
                switch (token.Value.Type)
                {
                    case JTokenType.Array:
                        table = getDataTableParam(token);
                        if (table.Rows != null && table.Rows.Count > 0)
                            data.Tables.Add(table);
                        break;
                    case JTokenType.Integer:
                        cmd.Parameters.AddWithValue("@" + token.Key, Int32.Parse(token.Value.ToString()));
                        break;
                    case JTokenType.String:
                        cmd.Parameters.AddWithValue("@" + token.Key, token.Value.ToString());
                        break;
                    case JTokenType.Boolean:
                        cmd.Parameters.AddWithValue("@" + token.Key, Boolean.Parse(token.Value.ToString()));
                        break;
                    case JTokenType.Date:
                        cmd.Parameters.AddWithValue("@" + token.Key, DateTime.Parse(token.Value.ToString()));
                        break;
                    case JTokenType.Object:
                        foreach (var element in token.Value)
                        {
                            table = getDataTableParam(element);
                            data.Tables.Add(table);
                        }
                        break;
                    case JTokenType.Float:
                        cmd.Parameters.AddWithValue("@" + token.Key, float.Parse(token.Value.ToString()));
                        break;
                }
            }

            //ADD DATATABLES INTO PARAM COLLECTION
            foreach (DataTable table in data.Tables)
                cmd.Parameters.AddWithValue("@" + table.TableName, table);
        }

        private void addSqlParams(Equipment _equipment)
        {
            if (_equipment == null)
                return;

        }

        private void addSqlParamsEquipmentAdmin(JObject sqlParams)
        {
            DataSet data = new DataSet("params");
            foreach (var token in sqlParams)
            {
                DataTable table = null;
                switch (token.Value.Type)
                {
                    case JTokenType.Array:
                        if (token.Key.Equals("SearchTerms"))
                        {
                            foreach (var element in token.Value)
                            {
                                table = getDataTableAdminSearchParam(element.Value<string>());
                                data.Tables.Add(table);
                            }
                        }
                        else if (token.Key.Equals("RentalStatus"))
                        {

                        }
                        else
                        {
                            table = getDataTableParam(token);
                            data.Tables.Add(table);
                        }
                        break;
                    case JTokenType.Integer:
                        cmd.Parameters.AddWithValue("@" + token.Key, Int32.Parse(token.Value.ToString()));
                        break;
                    case JTokenType.String:
                        cmd.Parameters.AddWithValue("@" + token.Key, token.Value.ToString());
                        break;
                    case JTokenType.Object:
                        foreach (var element in token.Value)
                        {
                            table = getDataTableParam(element);
                            data.Tables.Add(table);
                        }
                        break;
                }
            }

            //ADD DATATABLES INTO PARAM COLLECTION
            foreach (DataTable table in data.Tables)
            {
                if (table.TableName != null && table.TableName.Equals("Locations"))
                {
                    table.TableName = "Location";
                    if (table.Rows != null && table.Rows.Count > 0 & table.Rows[0].ItemArray != null && table.Rows[0].ItemArray[0].ToString().Trim().Length > 0)
                        cmd.Parameters.AddWithValue("@" + table.TableName, table.Rows[0].ItemArray[0].ToString());
                }
                else if (table.TableName != null && table.TableName.Equals("Makes"))
                {
                    table.TableName = "ManufacturerID";
                    if (table.Rows != null && table.Rows.Count > 0 & table.Rows[0].ItemArray != null && table.Rows[0].ItemArray[0].ToString().Trim().Length > 0)
                        cmd.Parameters.AddWithValue("@" + table.TableName, table.Rows[0].ItemArray[0].ToString());
                }
                else if (table.TableName != null && table.TableName.Equals("Models"))
                {
                    table.TableName = "ModelNum";
                    if (table.Rows != null && table.Rows.Count > 0 & table.Rows[0].ItemArray != null && table.Rows[0].ItemArray[0].ToString().Trim().Length > 0)
                        cmd.Parameters.AddWithValue("@" + table.TableName, table.Rows[0].ItemArray[0].ToString());
                }
                else if (table.TableName != null && table.TableName.Equals("Divisions"))
                {
                    table.TableName = "DivisionID";
                    if (table.Rows != null && table.Rows.Count > 0 & table.Rows[0].ItemArray != null && table.Rows[0].ItemArray[0].ToString().Trim().Length > 0)
                        cmd.Parameters.AddWithValue("@" + table.TableName, table.Rows[0].ItemArray[0].ToString());
                }
                else if (table.TableName != null && table.TableName.Equals("Categories"))
                {
                    table.TableName = "ProductCategoryID";
                    if (table.Rows != null && table.Rows.Count > 0 & table.Rows[0].ItemArray != null && table.Rows[0].ItemArray[0].ToString().Trim().Length > 0)
                        cmd.Parameters.AddWithValue("@" + table.TableName, table.Rows[0].ItemArray[0].ToString());
                }
                else
                    if (table.Rows != null && table.Rows.Count > 0 & table.Rows[0].ItemArray != null && table.Rows[0].ItemArray[0].ToString().Trim().Length > 0)
                        cmd.Parameters.AddWithValue("@" + table.TableName, table.Rows[0].ItemArray[0].ToString());
            }
        }

        private DataTable getDataTableAdminSearchParam(string element)
        {
            DataTable table = new DataTable(element.Substring(0, element.IndexOf(']')).Trim('[', ']'));
            table.Columns.Add("value");
            DataRow row = table.NewRow();
            row["value"] = element.Substring(element.IndexOf(']') + 1, element.Length - element.IndexOf(']') - 1);
            table.Rows.Add(row);
            return table;
        }

        private DataTable getDataTableParam(KeyValuePair<string, JToken> token)
        {
            DataTable table = null;
            table = new DataTable(token.Key);

            if (token.Key.ToLower() == "equipment")
            {
                table.Columns.Add("Quantity");
                table.Columns.Add("Category");
                table.Columns.Add("Manufacturer");
                table.Columns.Add("Model");
                table.Columns.Add("Reason");

                foreach (var val in token.Value.Children())
                {
                    var props = val.Children<JProperty>();
                    DataRow row = table.NewRow();
                    foreach (var prop in props)
                    {
                        switch (prop.Name.ToLower())
                        {
                            case "quantity":
                                row["Quantity"] = prop.Value.ToString();
                                break;
                            case "category":
                                row["Category"] = prop.Value.ToString();
                                break;
                            case "manufacturer":
                                row["Manufacturer"] = prop.Value.ToString();
                                break;
                            case "model":
                                row["Model"] = prop.Value.ToString();
                                break;
                            case "reason":
                                row["Reason"] = prop.Value.ToString();
                                break;
                        }
                    }

                    table.Rows.Add(row);
                }
            }
            else if(token.Key.ToLower() == "address")
            {
                table.Columns.Add("AddressID");
                table.Columns.Add("Street1");
                table.Columns.Add("Street2");
                table.Columns.Add("City");
                table.Columns.Add("State");
                table.Columns.Add("PostalCode");
                table.Columns.Add("CountryName");

                foreach (var val in token.Value.Children())
                {
                    var props = val.Children<JProperty>();
                    DataRow row = table.NewRow();
                    foreach (var prop in props)
                    {
                        switch (prop.Name.ToLower())
                        {
                            case "addressid":
                                row["AddressID"] = prop.Value.ToString();
                                break;
                            case "street1":
                                row["Street1"] = prop.Value.ToString();
                                break;
                            case "street2":
                                row["Street2"] = prop.Value.ToString();
                                break;
                            case "city":
                                row["City"] = prop.Value.ToString();
                                break;
                            case "state":
                                row["State"] = prop.Value.ToString();
                                break;
                            case "postalcode":
                                row["PostalCode"] = prop.Value.ToString();
                                break;
                            case "countryname":
                                row["CountryName"] = prop.Value.ToString();
                                break;
                        }
                    }

                    table.Rows.Add(row);
                }
            }
            else
            {
                //SIMPLE ARRAY
                IEnumerable<string> values = token.Value.Select(val => val.ToString());

                //CONDITIONALLY ADD COLUMNS
                table.Columns.Add("values");
                foreach (var val in values)
                {
                    DataRow row = table.NewRow();
                    row["values"] = val;
                    table.Rows.Add(row);
                }
            }

            return table;
        }

        private DataTable getDataTableParam(JToken prop)
        {
            DataTable table = null;
            var element = prop as JProperty;
            IEnumerable<string> values = element.Value.Select(val => val.ToString());
            table = new DataTable("FitsOn" + element.Name);
            table.Columns.Add("values");
            foreach (var val in values)
            {
                DataRow row = table.NewRow();
                row["values"] = val;
                table.Rows.Add(row);
            }

            return table;
        }

        private int InsertRecordInTrans(string cmdText, JObject sqlParams = null)
        {
            SqlCommand cmd = new SqlCommand();
            if (sqlParams != null) addSqlParams(sqlParams, cmd);

            var returnVal = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
            returnVal.Direction = ParameterDirection.ReturnValue;

            try
            {
                cmd.Connection = sqlConn;
                cmd.CommandText = cmdText;
                cmd.Transaction = sqltran;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }

            return (int)returnVal.Value;
        }
        private bool UpdateRecordInTrans(string cmdText, JObject sqlParams = null)
        {
            SqlCommand cmd = new SqlCommand();
            if (sqlParams != null) addSqlParams(sqlParams, cmd);

            try
            {
                cmd.Connection = sqlConn;
                cmd.CommandText = cmdText;
                cmd.Transaction = sqltran;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }

            return true;
        }
        
        public void BeginSqlTranscation()
        {
            if (sqlConn.State == ConnectionState.Closed) sqlConn.Open();
            sqltran = sqlConn.BeginTransaction();
        }

        public void CommitSqlTranscation()
        {
            sqltran.Commit();
            sqlConn.Close();
            cmd.Parameters.Clear();
        }

        public void RollBackSqlTranscation()
        {
            sqltran.Rollback();
            sqlConn.Close();
            cmd.Parameters.Clear();
        }

        #endregion
    }
}