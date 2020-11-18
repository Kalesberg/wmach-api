using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models;
using Newtonsoft.Json.Linq;

namespace API.Data
{
    public class TransportBuilder : IBuildParams<Transport>
    {
        private Transport _transport = null;
        public void Build(JObject sqlParams)
        {
            BuildTransport(sqlParams);
        }

        public Transport GetResult()
        {
            return _transport;
        }

        private void BuildTransport(JObject sqlParams)
        {
            var db = DAL.GetInstance();
            _transport = db.getTransportByEquipmentId(sqlParams);
        }
    }
}