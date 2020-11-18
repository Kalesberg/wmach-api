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
        public int GetUserLoginDuration(string userName)
        {
            string cmdText = ConfigurationManager.AppSettings["LoginUserDuration"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            cmd.Parameters.AddWithValue("@Username", userName);
            return ExecuteIntegerScalar(cmdText, userName);
        }
    }
}
