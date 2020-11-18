using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models;
using Newtonsoft.Json.Linq;
using System.Data;
using API.Models;


namespace API.Data
{
    public class DashBuilder : IBuildParams<IEnumerable<Dash>>
    {
        public IEnumerable<Dash> _dashes;
        public void Build(JObject sqlParams = null)
        {
            _dashes = DAL.GetInstance(DB.Mach1).GetDashes(sqlParams);
        }

        public IEnumerable<Dash> GetResult()
        {
            return _dashes;
        }


    }
}