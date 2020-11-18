using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Newtonsoft.Json.Linq;

namespace API.Data.Builders.Equipment
{
    public class EquipmentAdminSimple : IBuildParams<DataSet>
    {
        private DataSet _equipment;
        public void Build(JObject sqlParams)
        {
            _equipment = DAL.GetInstance().getEquipmentAdmin(sqlParams, "machine");
        }

        public DataSet GetResult()
        {
            return _equipment;
        }
    }
}