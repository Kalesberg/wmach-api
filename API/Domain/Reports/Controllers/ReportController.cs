using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API.Data;
using System.Net.Http.Headers;
using System.Data;
using System.IO;
using System.Web.Http.Cors;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using API.Utilities.Auth;
using API.Utilities;
using API.Models;

namespace API.Controllers
{
    [JWTAuthorization]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ReportController : ApiController
    {
        //PROVIDES ENDPOINT FOR ALL REPORTS WITH JSON PAYLOAD
        [HttpGet]
        [Route("api/report/{reportName}/{startDate}/{endDate}")]
        public HttpResponseMessage GetRentalRevenue(string reportName, string startDate, string endDate)
        {
            var db = DAL.GetInstance();
            var tokens = new JObject();
            tokens.Add("startDate", startDate);
            tokens.Add("endDate", endDate);
            var data = db.getReport(reportName, tokens);
            if (data == null) return Request.CreateResponse(HttpStatusCode.NoContent);
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

        ///<summary>
        ///Returns excel sheet of all contacts created last month
        ///</summary>
        [HttpGet]
        [Route("api/report/createdcontacts/")]
        public HttpResponseMessage GetLastMonthsCreatedContacts()
        {
            var db = DAL.GetInstance();
            var myContacts = db.getReportContactsCreated();
            if (myContacts == null) return Request.CreateResponse(HttpStatusCode.NoContent);
            HttpResponseMessage response;
            response = Request.CreateResponse(HttpStatusCode.OK);
            MediaTypeHeaderValue mediaType = new MediaTypeHeaderValue("application/octet-stream");
            Dictionary<int, string> ColumnFormats = new Dictionary<int, string>();
            ColumnFormats.Add(1, "yyyy-mm-dd hh:mm");
            response.Content = new StreamContent(ExcelGeneration.GetExcelSheet(myContacts, "Contacts", true, true, ColumnFormats));
            response.Content.Headers.ContentType = mediaType;
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = "ContactsCreated-"+DateTime.Now.AddMonths(-1).Month+"//"+DateTime.Now.AddMonths(-1).Year+".xls";
            return response;

        }


        ///<summary>
        ///Returns excel sheet of all contacts created last month
        ///</summary>
        [HttpGet]
        [Route("api/report/servicehistory/")]
        public HttpResponseMessage GetServiceHistoryReport()
        {
            var db = DAL.GetInstance();
            var data = db.getServiceHistoryReport();
            if (data == null) return Request.CreateResponse(HttpStatusCode.NoContent);
            HttpResponseMessage response;
            response = Request.CreateResponse(HttpStatusCode.OK);
            MediaTypeHeaderValue mediaType = new MediaTypeHeaderValue("application/octet-stream");
            response.Content = new StreamContent(ExcelGeneration.GetExcelSheet(data, "SvcHistory", true, true));
            response.Content.Headers.ContentType = mediaType;
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = String.Format("ServiceHistory{0}_{1}.xls", DateTime.Now.Month, DateTime.Now.Year);
            return response;
        }

        ///<summary>
        ///Returns excel sheet of all contacts created last month
        ///</summary>
        [HttpGet]
        [Route("api/report/workorders/")]
        public HttpResponseMessage GetWorkOrderReport()
        {
            var db = DAL.GetInstance();
            var data = db.getWorkOrderReport();
            if (data == null) return Request.CreateResponse(HttpStatusCode.NoContent);
            HttpResponseMessage response;
            response = Request.CreateResponse(HttpStatusCode.OK);
            MediaTypeHeaderValue mediaType = new MediaTypeHeaderValue("application/octet-stream");
            response.Content = new StreamContent(ExcelGeneration.GetExcelSheet(data, "WorkOrders", true, true));
            response.Content.Headers.ContentType = mediaType;
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = String.Format("WorkOrders{0}_{1}.xls", DateTime.Now.Month, DateTime.Now.Year);
            return response;
        }

        ///<summary>
        ///Returns excel sheet of all contacts created last month
        ///</summary>
        [HttpGet]
        [Route("api/report/cnbv/")]
        public HttpResponseMessage GetCNBVReport()
        {
            var db = DAL.GetInstance();
            var data = db.getCurrentCNBV();
            if (data == null) return Request.CreateResponse(HttpStatusCode.NoContent);
            HttpResponseMessage response;
            response = Request.CreateResponse(HttpStatusCode.OK);
            MediaTypeHeaderValue mediaType = new MediaTypeHeaderValue("application/octet-stream");
            response.Content = new StreamContent(ExcelGeneration.GetExcelSheet(data, "CNBV", true, true));
            response.Content.Headers.ContentType = mediaType;
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = String.Format("CNBV_{0}_{1}.xlsx", DateTime.Now.Month, DateTime.Now.Year);
            return response;
        }

        ///<summary>
        ///Returns excel sheet of all contacts created last month
        ///</summary>
        [HttpGet]
        [Route("api/report/availablereports/")]
        public HttpResponseMessage GetAvailableReports()
        {
            var db = DAL.GetInstance();
            var tokens = new JObject();
            var data = db.getAvailableReports();
            if (data == null) return Request.CreateResponse(HttpStatusCode.NoContent);
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// Returns aggregated lost opportunity reasons by division and length of duration in days from the current day
        /// <summary>
        /// <returns></returns>
        /// </summary>
        [HttpPost]
        [Route("api/report/opportunities/reasons")]
        public HttpResponseMessage GetLostOpportunityDataByDivision([FromBody] JObject json)
        {
            var db = DAL.GetInstance();
            var data = db.getLostOpportunityMetricsByReason(json);
            var opp = new OpportunityMetrics();
            opp.Duration = (int)json["Duration"];
            opp.Division = json["Division"].Select(token => token.ToString()).ToList();
            opp.Metrics = data;
            return Request.CreateResponse(HttpStatusCode.OK, opp);
        }

        ///<summary>
        /// Returns aggregated lost opportunity models by division, length of duration in days from current day, and a given reason
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/report/opportunities/models")]
        public HttpResponseMessage GetLostOpportunityDataByModel([FromBody] JObject json)
        {
            try
            {
                var db = DAL.GetInstance();
                var data = db.getLostOpportunityMetricsByModel(json);
                var opp = new OpportunityMetrics();
                opp.Duration = (int)json["Duration"];
                opp.Division = json["Division"].Select(token => token.ToString()).ToList();
                opp.Metrics = data;
                return Request.CreateResponse(HttpStatusCode.OK, opp);
            }
            catch(Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        ///<summary>
        ///Returns excel sheet of all Services
        ///</summary>
        [HttpGet]
        [Route("api/report/servicereport/")]
        public HttpResponseMessage GetServiceReport()
        {
            var tokensServiceData = new JObject();
            tokensServiceData.Add("@ContractNum", null);
            tokensServiceData.Add("@DivisionLocationID", null);
            tokensServiceData.Add("@RequestedByID", null);
            tokensServiceData.Add("@AssignedDivisionID", null);
            tokensServiceData.Add("@WorkOrderNum", null);
            tokensServiceData.Add("@WorkOrderStatus", null);
            tokensServiceData.Add("@NeedByDateStart", null);
            tokensServiceData.Add("@NeedByDateEnd", null);
            tokensServiceData.Add("@EndDateStart", null);
            tokensServiceData.Add("@EndDateEnd", null);
            tokensServiceData.Add("@SerialNum", null);
            tokensServiceData.Add("@UnitNumber", null);
            tokensServiceData.Add("@PropertyTag", null);
            tokensServiceData.Add("@ModelNum", null);
            tokensServiceData.Add("@CustomerName", null);
            tokensServiceData.Add("@ContractorName", null);
            tokensServiceData.Add("@ServiceManagerID", null);
            tokensServiceData.Add("@ServiceLocType", null);
            tokensServiceData.Add("@SalesManagerID", null);
            tokensServiceData.Add("@ServiceKind", "WorkOrder");

            var tokenServiceHistory = new JObject();
            tokenServiceHistory.Add("@EquipmentID", null);
            tokenServiceHistory.Add("@IncludeCancelledWO", "1");
            tokenServiceHistory.Add("@ServiceKind", "WorkOrder");
            
            var db = DAL.GetInstance();
            var ServiceSearch = db.getServiceReport(tokensServiceData);
            var ServiceEquipmentHistoryByWO = db.getServiceEquipmentHistoryByWO(tokenServiceHistory);
            if (ServiceSearch == null && ServiceEquipmentHistoryByWO == null) return Request.CreateResponse(HttpStatusCode.NoContent);

            HttpResponseMessage response;
            response = Request.CreateResponse(HttpStatusCode.OK);
            MediaTypeHeaderValue mediaType = new MediaTypeHeaderValue("application/octet-stream");

            //Setup Sheet 1 Formats
            Dictionary<int, string> ColumnFormatsServiceData = new Dictionary<int, string>();
            ColumnFormatsServiceData.Add(6, "yyyy-mm-dd hh:mm");
            ColumnFormatsServiceData.Add(10, "yyyy-mm-dd hh:mm");
            ColumnFormatsServiceData.Add(11, "yyyy-mm-dd hh:mm");
            ColumnFormatsServiceData.Add(12, "yyyy-mm-dd hh:mm");
            ColumnFormatsServiceData.Add(42, "yyyy-mm-dd hh:mm");
            ColumnFormatsServiceData.Add(43, "yyyy-mm-dd hh:mm");

            Dictionary<int, string> ColumnFormatsServiceHistory = new Dictionary<int, string>();
            ColumnFormatsServiceHistory.Add(1, "$#,##0");
            ColumnFormatsServiceHistory.Add(2, "$#,##0");
            ColumnFormatsServiceHistory.Add(4, "$#,##0");
            ColumnFormatsServiceHistory.Add(5, "$#,##0");
            ColumnFormatsServiceHistory.Add(7, "$#,##0");
            ColumnFormatsServiceHistory.Add(8, "$#,##0");
            ColumnFormatsServiceHistory.Add(9, "$#,##0");
            ColumnFormatsServiceHistory.Add(33, "$#,##0");
            ColumnFormatsServiceHistory.Add(34, "$#,##0");
            ColumnFormatsServiceHistory.Add(19, "yyyy-mm-dd hh:mm");
            ColumnFormatsServiceHistory.Add(20, "yyyy-mm-dd hh:mm");
            ColumnFormatsServiceHistory.Add(21, "yyyy-mm-dd hh:mm");
            ColumnFormatsServiceHistory.Add(22, "yyyy-mm-dd hh:mm");
            ColumnFormatsServiceHistory.Add(26, "yyyy-mm-dd hh:mm");

            //Compile Dictionary of the Sheet Formats
            Dictionary<string, Dictionary<int, string>> ColumnFormats = new Dictionary<string, Dictionary<int, string>>();
            ColumnFormats.Add("ServiceData", ColumnFormatsServiceData);
            ColumnFormats.Add("ServiceHistory", ColumnFormatsServiceHistory);

            //Compile Dictionary of the Data
            Dictionary<string, DataTable> DataTables = new Dictionary<string, DataTable>();
            DataTables.Add("ServiceData", ServiceSearch);
            DataTables.Add("ServiceHistory", ServiceEquipmentHistoryByWO);
            //Compile the Worksheet Names
            string[] Worksheets = { "ServiceData", "ServiceHistory" };

            response.Content = new StreamContent(ExcelGeneration.GetExcelWorkbook(DataTables, Worksheets, true, true, ColumnFormats));
            response.Content.Headers.ContentType = mediaType;
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = "WorkOrders-" + DateTime.Now.Month + "//" + DateTime.Now.Day + "//" + DateTime.Now.Year + ".xls";
            return response;

        }

        ///<summary>
        ///Returns excel sheet of all contacts created last month
        ///</summary>
        [HttpGet]
        [Route("api/report/fixedasset/")]
        public HttpResponseMessage GetFixedAssetReport()
        {
            try
            {
                var db = DAL.GetInstance();
                var Assets = db.getReportFixedAsset();
                if (Assets == null) return Request.CreateResponse(HttpStatusCode.NoContent);
                HttpResponseMessage response;
                response = Request.CreateResponse(HttpStatusCode.OK);
                MediaTypeHeaderValue mediaType = new MediaTypeHeaderValue("application/octet-stream");
                Dictionary<int, string> ColumnFormats = new Dictionary<int, string>();
                ColumnFormats.Add(22, "yyyy-mm-dd hh:mm");
                ColumnFormats.Add(23, "yyyy-mm-dd hh:mm");
                ColumnFormats.Add(37, "yyyy-mm-dd hh:mm");
                ColumnFormats.Add(38, "yyyy-mm-dd hh:mm");
                ColumnFormats.Add(39, "yyyy-mm-dd hh:mm");
                response.Content = new StreamContent(ExcelGeneration.GetExcelSheet(Assets, "FixedAssets", true, true, ColumnFormats));
                response.Content.Headers.ContentType = mediaType;
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = "FixedAssets-" + DateTime.Now.AddMonths(-1).Month + "//" + DateTime.Now.AddMonths(-1).Year + ".xls";
                return response;
            }
            catch(Exception Ex)
            {
                HttpResponseMessage response;
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, Ex.Message);
                return response;
            }

        }

        [HttpGet]
        [Route("api/report/equipment/metrics")]
        public HttpResponseMessage GetAdminGridMetrics()
        {
            var db = DAL.GetInstance();
            var data = db.getEquipmentAdminMetrics();
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

        [HttpPost]
        [Route("api/report/equipment/metrics")]
        public HttpResponseMessage GetAdminGridMetrics([FromBody] JObject json)
        {
            var db = DAL.GetInstance();
            var data = db.getEquipmentAdminMetrics(json);
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

        [HttpPost]
        [Route("api/report/equipment/projections")]
        public HttpResponseMessage SaveAdminGridMetrics([FromBody] JObject json)
        {
            var db = DAL.GetInstance();
            var created = db.saveAdminMetricProjections(json);
            return created ? Request.CreateResponse(HttpStatusCode.Created, created) : Request.CreateResponse(HttpStatusCode.BadRequest, created);
        }
    }
}