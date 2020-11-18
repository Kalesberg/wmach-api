using API.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace API.Data
{
    public class CustomerDetail : IBuildParams<List<CustomerDetails>>
    {
        public List<CustomerDetails> _customer;
        public void Build(JObject sqlParams)
        {
            if (sqlParams.Count > 1)
            {
                //We're doing an update, not just a fetch.  We've sent more than just a contact ID.

            }
            _customer = DAL.GetInstance().getCustomerInfoByContactID(sqlParams);
            //For each we need to get addresses for that customer
            if (_customer != null && _customer.Count > 0)
            {
                foreach (CustomerDetails details in _customer)
                {
                    GetAddresses(details.contactId);
                    GetEmails(details.contactId);
                    GetPhoneNumbers(details.contactId);
                    GetContractsAndHistorySummary(details.contactId);
                    GetSalespersonDivision(details.assignedRepID);
                }
            }
        }

        public List<CustomerDetails> GetResult()
        {
            return _customer;
        }

        private void GetAddresses(int contactId)
        {
            var json = new JObject { { "ContactID", contactId } };
            _customer.Find((val) => (val.contactId == contactId)).address = DAL.GetInstance().getCustomerAddresses(json).ToList();
        }

        private void GetEmails(int contactId)
        {
            var json = new JObject { { "ContactID", contactId } };
            _customer.First().emailAddresses = DAL.GetInstance().getCustomerEmails(json).ToList();
        }

        private void GetPhoneNumbers(int contactId)
        {
            var json = new JObject { { "ContactID", contactId } };
            _customer.First().businessPhones = DAL.GetInstance().getCustomerPhones(json).ToList();
        }

        private void GetContractsAndHistorySummary(int contactId)
        {
            var json = new JObject { { "ContactID", contactId } };
            _customer.First().contractsAndQuotes = DAL.GetInstance().getContractQuoteSummary(json).ToList();
        }

        private void GetSalespersonDivision(int contactId)
        {
            var json = new JObject { { "ContactID", contactId } };
            _customer.First().assignedRepDivision = DAL.GetInstance().getDivisionBySalesPersonContactID(json).ToList().FirstOrDefault();
        }
    }

}