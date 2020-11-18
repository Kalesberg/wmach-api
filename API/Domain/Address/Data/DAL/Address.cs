using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using API.Models;
using Newtonsoft.Json.Linq;

namespace API.Data
{
    public partial class DAL
    {
        public List<Address> getAddressByContactID(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["getAddressByContactID"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<Address>(cmdText, sqlParams);
        }

        public int CreateAddress(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["CreateAddress"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return InsertRecord(cmdText, sqlParams);
        }
        
        public int CreateContactAddressRelationship(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["CreateContactAddressRelationship"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return InsertRecord(cmdText, sqlParams);
        }
        public ContactAddress getAddressByAddressID(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["GetAddressByAddressID"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<ContactAddress>(cmdText, sqlParams).FirstOrDefault();
        }
        public ContactAddress getAddressByDivisionID(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["GetAddressByDivisionID"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<ContactAddress>(cmdText, sqlParams).FirstOrDefault();
        }
    }
}