using API.Models;
using API.Utilities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Xml.Linq;

namespace API.Data
{
    public partial class DAL
    {

        public bool CreateNewCustomerPortalAccount(JObject sqlParams)
        {
            var PasswordObj = new API.Utilities.Password.Password();
            var secretKey = ConfigurationManager.AppSettings["CustomerPortalAuth"];
            string TimeStampLink = string.Empty;
            var CustomerPortalObj = new API.Utilities.Auth.CustomerPortalAuth();
            var TimeStampEncrypted = PasswordObj.Encrypt(DateTime.Now.ToString(), secretKey);
            TimeStampLink = CustomerPortalObj.FormatLink(TimeStampEncrypted);
            sqlParams.Add("TimeStampEncrypted", TimeStampEncrypted);
            sqlParams.Add("TimeStampLink", TimeStampLink);
            string cmdText = ConfigurationManager.AppSettings["CreateNewCustomerPortalAccount"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return InsertData(cmdText, sqlParams);
        }
        public int CheckifCustomerPortalAccountExist(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["CheckifCustomerPortalAccountExist"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return InsertRecord(cmdText, sqlParams);
        }

        public async Task SendPasswordResetEmail(string TimeStampLink, string Email)
        {
            string link = string.Empty;

            //Get Environment

            string Env = ConfigurationManager.ConnectionStrings["mach1"].ConnectionString;

            //Build the link by Environnement


            if (Env.Contains("galsql01"))
            {
                link = "https://staging.customers.wwmach.com/login/" + TimeStampLink;
                Email = "testsupports@wwmach.com";
            }

            else if (Env.Contains("localhost"))
            {
                link = "https://staging.customers.wwmach.com/login/" + TimeStampLink;
                Email = "testsupports@wwmach.com";
            }
            else
            {
                link = "https://customers.wwmach.com/login/" + TimeStampLink;
            }

            string CurrentYear = DateTime.Now.Year.ToString();
            string body = string.Empty;
            //using streamreader for reading my htmltemplate   
            using (StreamReader reader = new StreamReader(HostingEnvironment.MapPath(("~/Templates/HTML/NewCustomerPortalAccount.html"))))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{link}", link);
            body = body.Replace("{CurrentYear}", CurrentYear);
            //Send Email

            await new Email("Customer Portal", "DoNotReply@wwmach.com", Email,
                           "Customer Portal: New account", body).Send();
        }

        public List<string> GetListInvoiceXMLForCustomer(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["GetListofInvoiceXMLforCustomer"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<string>(cmdText, sqlParams);
        }

        public List<Invoice> GetListOfInvoice(string list)
        {
            XDocument doc = new XDocument();
            doc = XDocument.Parse(list);
            List<Invoice> BillList = new List<Invoice>();

            IEnumerable<XElement> lx = from inv in doc.Descendants("eConnect")
                                       orderby inv.Attribute("DOCDATE").Value descending
                                       select inv;

            foreach (var ele in lx)
            {
                string invoicet = "";

                if (ele.Element("Receivables").Element("SalesOrderHeaderHistory").Element("UserDefined").Element("USRTAB03").Value.ToString().StartsWith("Rental"))
                    invoicet = "Rental";
                else if (ele.Element("Receivables").Element("SalesOrderHeaderHistory").Element("UserDefined").Element("USRTAB03").Value.ToString().StartsWith("Shipment"))
                    invoicet = "Shipment";
                else if (ele.Element("Receivables").Element("SalesOrderHeaderHistory").Element("UserDefined").Element("USRTAB03").Value.ToString().StartsWith("Service"))
                    invoicet = "Service";

                var bill = new Invoice
                {
                    InvoiceNum = (int)ele.Element("Receivables").Element("DOCNUMBR"),
                    DUEDATE = (DateTime)ele.Element("Receivables").Element("DUEDATE"),
                    INVODATE = (DateTime)ele.Element("Receivables").Element("SalesOrderHeaderHistory").Element("INVODATE"),
                    DocType = (string)ele.Element("Receivables").Element("SalesOrderHeaderHistory").Element("DOCID"),
                    InvoiceType = invoicet,
                    SubTotal = (decimal)ele.Element("Receivables").Element("SalesOrderHeaderHistory").Element("SUBTOTAL"),
                    TaxAmount = (decimal)ele.Element("Receivables").Element("TAXAMNT"),
                    BalanceDue = (decimal)ele.Element("Receivables").Element("CURTRXAM"),
                    InvoiceAmount = (decimal)ele.Element("Receivables").Element("SalesOrderHeaderHistory").Element("DOCAMNT"),
                    CompanyName = (string)ele.Element("Receivables").Element("SalesOrderHeaderHistory").Element("CUSTNAME"),
                    BillingAddress = new ContactAddress
                    {
                        Street1 = (string)ele.Element("Receivables").Element("SalesOrderHeaderHistory").Element("Address").Element("ADDRESS1"),
                        Street2 = (string)ele.Element("Receivables").Element("SalesOrderHeaderHistory").Element("Address").Element("ADDRESS2"),
                        City = (string)ele.Element("Receivables").Element("SalesOrderHeaderHistory").Element("Address").Element("CITY"),
                        State = (string)ele.Element("Receivables").Element("SalesOrderHeaderHistory").Element("Address").Element("STATE"),
                        PostalCode = (string)ele.Element("Receivables").Element("SalesOrderHeaderHistory").Element("Address").Element("ZIP"),
                        CountryName = (string)ele.Element("Receivables").Element("SalesOrderHeaderHistory").Element("Address").Element("COUNTRY"),
                    },
                    Division = (string)ele.Element("Receivables").Element("SalesOrderHeaderHistory").Element("LOCNCODE"),
                    PONum = (string)ele.Element("Receivables").Element("SalesOrderHeaderHistory").Element("CSTPONBR"),
                    ContractNum = ((string)ele.Element("Receivables").Element("SalesOrderHeaderHistory").Element("UserDefined").Element("USRDEF05")) != "" ? (int)ele.Element("Receivables").Element("SalesOrderHeaderHistory").Element("UserDefined").Element("USRDEF05") : 0,
                    InvoiceStatus = ((decimal)ele.Element("Receivables").Element("CURTRXAM")) > 0 ? "Pending" : "Paid",
                    AccountManagerID = (int)ele.Element("Receivables").Element("SLPRSNID")

                };
                var sqlParamsAccount = new JObject { { "ContactID", bill.AccountManagerID } };
                bill.AccountManager = DAL.GetInstance().getContactByContactID(sqlParamsAccount);
                if (bill.ContractNum != 0)
                {
                    var sqlParams = new JObject { { "ContractNum", bill.ContractNum } };
                    ContractView contra = DAL.GetInstance().getContractByContractNum(sqlParams);
                    var sqlParamsRent = new JObject { { "ContactID", contra.RentalCoordinatorID } };
                    bill.RentalCoordinator = DAL.GetInstance().getContactByContactID(sqlParamsRent);
                    var json = new JObject { { "AddressID", contra.JobSiteAddressID } };
                    bill.Jobsite = DAL.GetInstance().getAddressByAddressID(json);
                    var jsondivision = new JObject { { "DivisionID", contra.WWMDivisionID } };
                    bill.DivisionAddress = DAL.GetInstance().getAddressByDivisionID(jsondivision);
                }


                BillList.Add(bill);

            }
            return BillList;
        }

        public List<SalesmanInfo> GetListofSalesmanforCustomer(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["GetListofSalesmanforCustomer"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<SalesmanInfo>(cmdText, sqlParams);
        }

        public List<InvoiceDetail> GetInvoiceDetailForCustomer(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["GetInvoiceDetailForCustomer"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<InvoiceDetail>(cmdText, sqlParams);
        }

        public int CheckEmailWhenRequestAccess(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["CheckEmailWhenRequestAccess"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return getRecords<int>(cmdText, sqlParams).FirstOrDefault();
        }

        public int GetCustomerIDByEmail(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["GetCustomerIDByEmail"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return getRecords<int>(cmdText, sqlParams).FirstOrDefault();
        }

        /// <summary>
        /// Creates an access request object
        /// </summary>
        /// <param name="jObject"></param>
        /// <returns></returns>
        public int CreatePortalAccessRequestObject(JObject jObject)
        {
            string cmdText = ConfigurationManager.AppSettings["PortalAccessRequestObject"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return getRecords<int>(cmdText, jObject).FirstOrDefault();
        }

        public int createCustomerQuoteRequest(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["CustomerQuoteRequestCreate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return InsertRecord(cmdText, sqlParams);
        }
        public int createCustomerQuoteRequestEquipment(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["CustomerQuoteRequestEquipmentCreate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return InsertRecord(cmdText, sqlParams);
        }
    }
}