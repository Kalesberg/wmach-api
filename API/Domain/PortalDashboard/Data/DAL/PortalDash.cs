
using API.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace API.Data
{
    public partial class DAL
    {
        /// <summary>
        /// Retrive the list of portal Dashboard Metrics from DB
        /// </summary>
        /// <param name="jobject"></param>
        /// <returns></returns>
        public PortalMetrics GetPortalMetrics(JObject jobject)
        {
            var sqlParams = new PortalMetrics();

            string cmdText = ConfigurationManager.AppSettings["PortalDashMetrics"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            sqlParams.Metrics = getRecords<Metrics>(cmdText, jobject).FirstOrDefault();
            sqlParams.Metrics.Agreements = getRecords<Agreements>(cmdText, jobject).FirstOrDefault();
            sqlParams.Metrics.Quotes = getRecords<Quotes>(cmdText, jobject).FirstOrDefault();
            return sqlParams;
        }

        /// <summary>
        /// Retrieve the list of all the lastest quotes created in last 7 days
        /// </summary>
        /// <param name="jObject"></param>
        /// <returns></returns>
        public List<LatestQuotes> GetLatestQuotes(JObject jObject)
        {
            string cmdText = ConfigurationManager.AppSettings["LatestQuotes"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<LatestQuotes>(cmdText, jObject);
        }

        /// <summary>
        /// Rettrieve list of portal access requests
        /// </summary>
        /// <param name="jObject"></param>
        /// <returns></returns>
        public List<PortalAccessRequest> GetPortalAccessRequests(JObject jObject)
        {
            string cmdText = ConfigurationManager.AppSettings["GetPortalAccessRequest"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<PortalAccessRequest>(cmdText, jObject);
        }

        /// <summary>
        /// Retriveee   List of transport request
        /// </summary>
        /// <param name="jObject"></param>
        /// <returns></returns>
        public List<TransportationRequests> GetTransportRequest(JObject jObject)
        {
            string cmdText = ConfigurationManager.AppSettings["GetTransportRequest"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<TransportationRequests>(cmdText, jObject);
        }

        /// <summary>
        /// Get Outstanding customer invoices
        /// </summary>
        /// <param name="jObject"></param>
        /// <returns></returns>
        public List<OverdueInvoices> GetOutInvoice(JObject jObject)
        {
            string cmdText = ConfigurationManager.AppSettings["GetInvoice"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<OverdueInvoices>(cmdText, jObject);
        }

        /// <summary>
        /// Get list of sequence tracking
        /// </summary>
        /// <param name="jObject"></param>
        /// <returns>A datatable</returns>
        public List<SequenceTracking> GetSequenceTrackingReport(JObject jObject)
        {
            string cmdText = ConfigurationManager.AppSettings["GetReportTrackingSequence"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<SequenceTracking>(cmdText, jObject);
        }

        /// <summary>
        /// Get a list of sequence tracking
        /// </summary>
        /// <param name="limit"></param>
        /// <returns>A specifued limit set</returns>
        public List<SequenceTracking> GetSequenceTrackingReport(int limit)
        {
            string cmdText = ConfigurationManager.AppSettings["GetReportTrackingSequence"];
            JObject obj = new JObject { { "limit", limit } };
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<SequenceTracking>(cmdText, obj);
        }

        /// <summary>
        /// Get a list of sequence tracking
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns>A specifued limit set</returns>
        public List<SequenceTracking> GetSequenceTrackingReport(DateTime dateFrom, DateTime dateTo)
        {
            string cmdText = ConfigurationManager.AppSettings["GetReportTrackingSequence"];
            JObject obj = new JObject { { "dateFrom", dateFrom }, { "dateTo", dateTo } };
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<SequenceTracking>(cmdText, obj);
        }

        public List<PendinContractDetails> GetAllPendingContracts(int offset, int limit, string status, string contractNum, string Division)
        {
            string cmdText = ConfigurationManager.AppSettings["GetPendinContracts"];
            JObject jObject = new JObject { { "offset", offset }, { "row", limit }, { "status", status }, { "contractnum", contractNum }, { "Division", Division } };
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<PendinContractDetails>(cmdText, jObject);
        }

        public List<QuoteDetailsPortal> GetAllQuotes(int offset, int limit, string status, string quotenum, string Division)
        {
            string cmdText = ConfigurationManager.AppSettings["GetAllQuotes"];
            JObject jObject = new JObject { { "offset", offset }, { "row", limit }, { "status", status }, { "Quotenum", quotenum }, { "Division", Division } };
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<QuoteDetailsPortal>(cmdText, jObject);
        }
    }
}