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
        public List<Quote> GetQuotes(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["Quotes"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            Func<DataTable, List<Quote>> transform = transformQuoteData;
            return getRecords<Quote>(cmdText, transform, sqlParams);
        }

        public List<Quote> GetActiveQuotes(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["ActiveQuotes"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            Func<DataTable, List<Quote>> transform = transformQuoteData;
            return getRecords<Quote>(cmdText, transform, sqlParams);
        }

        public int CreateQuoteDetail(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["CreateQuoteDetail"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return InsertRecord(cmdText, sqlParams);
        }

        public int CreateQuote(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["CreateQuote"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return InsertRecord(cmdText, sqlParams);
        }

        public int CreateSalesQuote(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["CreateSalesQuote"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return InsertRecord(cmdText, sqlParams);
        }

        public List<QuoteDetail> getQuoteDetailsByQuoteID(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["getQuoteDetailsByQuoteID"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<QuoteDetail>(cmdText, sqlParams);
        }

        public Quote getQuoteByQuoteID(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["getQuoteByQuoteID"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<Quote>(cmdText, sqlParams).FirstOrDefault();
        }

        public Quote getQuoteByQuoteHashID(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["getQuoteByQuoteHashID"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<Quote>(cmdText, sqlParams).FirstOrDefault();
        }
      

        public List<QuoteSearchResults> QuoteSearch(JObject sqlParams, string OpenOrClosed = null,string RentalOrSales=null)
        {
            string cmdText = ConfigurationManager.AppSettings["QuoteSearch"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            if (OpenOrClosed != null) cmd.Parameters.AddWithValue("@OpenOrClosed", OpenOrClosed);
            if (OpenOrClosed != null) cmd.Parameters.AddWithValue("@RentalOrSales", RentalOrSales);
            return getRecords<QuoteSearchResults>(cmdText,sqlParams); //no data back with sqlparams
        }

        public List<Quote> transformQuoteData(DataTable data)
        {
            var quotes = data.AsEnumerable().GroupBy(r => Int32.Parse(r["QuoteNumber"].ToString()))
                                              .Select(rec => new Quote
                                              {
                                                  quoteID = rec.Select(row => (int)row["quoteID"]).First(),
                                                  QuoteType = rec.Select(row => row["quoteType"].ToString()).First(),
                                                  account = rec.Select(row => row["companyName"].ToString()).First(),
                                                  jobSite = rec.Select(row => row["jobSite"].ToString()).First(),
                                                  division = rec.Select(row => row["division"].ToString()).First(),
                                                  startDate = (rec.Select(row => row["estimatedStartDate"]).First()) == DBNull.Value ? null : rec.Select(row => (DateTime?)row["estimatedStartDate"]).First(),
                                                  created = rec.Select(row => (DateTime)row["EnterDateTime"]).First(),
                                                  quoteNumber = rec.Key,
                                                  MinimumTerm =  (rec.Select(row => row["MinimumTerm"]).First()) == DBNull.Value ? null : rec.Select(row => (int?)row["MinimumTerm"]).First(),
                                                  MinimumTermUOM = rec.Select(row => row["MinimumTermUOM"].ToString()).FirstOrDefault(),
                                                  items = rec.Select(qd => new QuoteItem
                                                  {
                                                      make = qd["make"].ToString(),
                                                      model = qd["model"].ToString(),
                                                      weeklyRate = qd["weeklyrate"] == DBNull.Value ? Decimal.Zero : (decimal?)qd["weeklyrate"],
                                                      monthlyRate = qd["monthlyrate"] == DBNull.Value ? Decimal.Zero : (decimal?)qd["monthlyrate"],
                                                      dailyRate = qd["dailyrate"] == DBNull.Value ? Decimal.Zero : (decimal?)qd["dailyrate"],
                                                      quantity = (int)qd["quantity"]
                                                  }).ToList(),
                                              }).ToList();
            return quotes;
        }

        public Boolean SendQuoteEmailByDB(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["QuoteSendEmailbyDB"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecord(cmdText, sqlParams);
        }

        public QuoteDetailMoreFields getQuoteDetailExtensionByQuoteDetailID(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["QuoteDetailExtensionByQuoteDetailID"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<QuoteDetailMoreFields> (cmdText,sqlParams).FirstOrDefault();
        }
        public Boolean CreateQuoteDetailExtension(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["CreateQuoteDetailExtension"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecord(cmdText, sqlParams);
        }
        public int GetQuoteNumberByQuoteID(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["GetQuoteNumberByQuoteID"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return getRecords<int>(cmdText, sqlParams).FirstOrDefault();
        }


        public int CreateNewEmailPreference(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["CreateEmailPreference"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return InsertRecord(cmdText, sqlParams);
        }

        public IEnumerable<QuoteEmailPreference> GetEmailPreferenceInfo(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["GetEmailPreferenceInfo"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<QuoteEmailPreference>(cmdText, sqlParams);
        }
        public Boolean DeactiveEmailPreference(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["DeactiveEmailPreference"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecord(cmdText, sqlParams);
        }
        public Boolean EditEmailPreference(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["EditEmailPreference"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecord(cmdText, sqlParams);
        }
        public Boolean MakeDefaultEmailPreference(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["MakeDefaultmailPreference"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecord(cmdText, sqlParams);
        }

        public string GetTermAndCondition(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["GetTermAndCondition"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<string>(cmdText, sqlParams).FirstOrDefault();
        }

        public List<QuoteListCustomerPortal> GetListQuoteForCustomer(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["GetListofQuoteforCustomer"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<QuoteListCustomerPortal>(cmdText, sqlParams);
        }

        public bool CheckQuoteIDMatchCustomerID(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["CheckQuoteIDMatchCustomerID"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return InsertRecord(cmdText, sqlParams) == 1;
        }

        public List<ShipmentQuotesInventory> getShipmentQuotesInventoryByShipmentQuotesIDForCustomer(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["getShipmentQuotesInventoryByShipmentQuotesIDForCustomer"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<ShipmentQuotesInventory>(cmdText, sqlParams);
        }

        public ShipmentQuotes getShipmentQuotesByQuoteIDForCustomer(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["getShipmentQuotesByQuoteIDForCustomer"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<ShipmentQuotes>(cmdText, sqlParams).FirstOrDefault();
        }

        public CategoryType getCategroyTypeByProductCategoryID(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["getCategroyTypeByProductCategoryID"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<CategoryType>(cmdText, sqlParams).FirstOrDefault();
        }

        public bool customerAcceptQuote(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["CustomerAcceptQuote"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecord(cmdText, sqlParams);
        }
    }
}