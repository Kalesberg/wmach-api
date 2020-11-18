using API.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace API.Data
{
    public partial class DAL
    {
        public int GetContactIDByUsername(string username)
        {
            var cmdText = ConfigurationManager.AppSettings["ContactIDByUsername"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            cmd.Parameters.AddWithValue("@Username", username);
            var data = getRecords<int>(cmdText);
            return data.Any() ? data.First() : 0;
        }

        public List<Contact> getContactByAccountManagerID(JObject json)
        {
            var cmdText = ConfigurationManager.AppSettings["GetContactByAccountManager"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<Contact>(cmdText, json);
        }

        public IEnumerable<Contact> getContactsFromEmail(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["ContactByEmail"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            var data = getRecords<Contact>(cmdText, sqlParams);
            getContactAddress(data);
            return data;
        }

        public ContactRelationship getContactRelationshipByContactRelationshipID(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["GetContactRelationshipByContactRelationshipID"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<ContactRelationship>(cmdText, sqlParams).FirstOrDefault();
        }

        public Contact getContactByContactID(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["GetContactByContactID"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<Contact>(cmdText, sqlParams).FirstOrDefault();
        }

        public IEnumerable<Contact> getMyClients(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["MyClients"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<Contact>(cmdText, sqlParams);
        }

        public IEnumerable<Contact> getContactByName(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["ContactByName"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<Contact>(cmdText, sqlParams);
        }

        public IEnumerable<Contact> getAccountManagerContacts()
        {
            string cmdText = ConfigurationManager.AppSettings["ContactAccountManagers"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<Contact>(cmdText);
        }

        public IEnumerable<Contact> getUserContacts(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["UserContacts"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<Contact>(cmdText, sqlParams);
        }

        public bool updateContactDetails(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["ContactUpdate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecord(cmdText, sqlParams);
        }

        public void getContactAddress(IEnumerable<Contact> contacts)
        {
            if (sqlConn.State == ConnectionState.Closed) sqlConn.Open();
            foreach (var contact in contacts)
            {
                Address addr = new Address();
                cmd.Connection = sqlConn;
                cmd.CommandText = "[m2].[GetAddressByContactID]";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ContactID", contact.ContactId);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    addr.street = rdr.GetString(0);
                    addr.city = rdr.GetString(1);
                    addr.state = rdr.GetString(2);
                    addr.zip = rdr.GetString(3);
                }
                rdr.Close();
                rdr.Dispose();
                cmd.Parameters.Clear();
                contact.ContactAddress = addr;
            }

            if (sqlConn.State != ConnectionState.Closed)
                sqlConn.Close();
        }

        ///<summary>
        /// Gets the active customer list (default for My Customers in Mobile)
        ///</summary>
        public List<Customer> getCustomers(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["CustomerSearch"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<Customer>(cmdText, sqlParams);
        }

        public List<CustomerDetails> getCustomerInfoByContactID(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["GetCustomerInfoByContactID"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<CustomerDetails>(cmdText, sqlParams);
        }

        public bool updateCustomerInfo(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["UpdateCustomerInfo"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecord(cmdText, sqlParams);
        }

        public bool updateCustomerInfoForContactID(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["UpdateCustomerInfoForContactID"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecord(cmdText, sqlParams);
        }

        public List<ContactAddress> getCustomerAddresses(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["GetCustomerAddresses"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<ContactAddress>(cmdText, sqlParams);
        }

        public bool updateCustomerAddresses(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["UpdateCustomerAddresses"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecord(cmdText, sqlParams);
        }

        public List<string> getCustomerEmails(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["GetCustomerEmails"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<string>(cmdText, sqlParams);
        }

        public bool updateCustomerEmails(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["UpdateCustomerEmails"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecord(cmdText, sqlParams);
        }

        public List<string> getCustomerPhones(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["GetCustomerPhones"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<string>(cmdText, sqlParams);
        }

        public bool updateCustomerPhones(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["UpdateCustomerPhones"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecord(cmdText, sqlParams);
        }

        public IEnumerable<EmailGroup> getEmailGroups(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["EmailGroupsByContactID"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<EmailGroup>(cmdText, sqlParams);
        }

        public List<Salesperson> getSalespeople()
        {
            string cmdText = ConfigurationManager.AppSettings["GetSalespeople"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<Salesperson>(cmdText);
        }

        public List<Salesperson> getCoordinator()
        {
            string cmdText = ConfigurationManager.AppSettings["GetCoordinator"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<Salesperson>(cmdText);
        }

        public List<Salesperson> getSalesManager()
        {
            string cmdText = ConfigurationManager.AppSettings["GetSalesManager"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<Salesperson>(cmdText);
        }

        public IEnumerable<BusinessCodeHierarchy> getBusinessCodeCategories()
        {
            string cmdText = ConfigurationManager.AppSettings["BusinessCodeCategories"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            Func<DataTable, List<BusinessCodeHierarchy>> businessCodeTransform = getBusinessCodesStructured;
            return getRecords<BusinessCodeHierarchy>(cmdText, businessCodeTransform);
        }

        private List<BusinessCodeHierarchy> getBusinessCodesStructured(DataTable data)
        {
            var bizCodes = data.AsEnumerable().GroupBy(row => row["parentName"].ToString()).Select(r => new BusinessCodeHierarchy()
            {
                parent = r.Key,
                children = r.Select(d => d["childName"].ToString()).ToList()
            }).ToList();
            foreach (var bizCode in bizCodes)
            {
                if (bizCode.children.Count == 1 && String.IsNullOrWhiteSpace(bizCode.children[0]))
                {
                    bizCode.children = new List<string>();
                }
            }

            return bizCodes;
        }

        public int CreateContact(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["CreateContact"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return InsertRecord(cmdText, sqlParams);
        }
        public int CreateContactRelationship(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["CreateContactRelationship"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return InsertRecord(cmdText, sqlParams);
        }

        public int getContactIDByCompanyName(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["GetContactIDByCompanyName"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return getRecords<int>(cmdText, sqlParams).FirstOrDefault();
        }

        public int CreateCompanyCategory(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["CreateCompanyCategory"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return InsertRecord(cmdText, sqlParams);
        }
        public int CreateBCCategory(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["CreateBCCategory"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return InsertRecord(cmdText, sqlParams);
        }

        public bool UpdatePortalWithLoginDate(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["UpdatePortalLoginDate"];
            if (string.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecord(cmdText, sqlParams);
        }
    }
}