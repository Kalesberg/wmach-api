using API.Data;
using API.Models;
using API.Services.HubspotService.Company.Controller;
using API.Services.HubspotService.Deals.Models;
using API.Utilities.Auth;
using API.Utilities.Company;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace API.Domain.Hubspot.Controllers
{
    /// <summary>
    /// Hubspot deals controller
    /// Get deals from Hubspot 
    /// Update Hubspot deals
    /// </summary>
    [JWTAuthorization]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CompanyHubspotController : ApiController
    {

        private readonly CompanyController companyController = new CompanyController();

        [HttpGet]
        [Route("api/hubspot/companies")]
        public HttpResponseMessage GetAllHubspotCompanies()
        {
            var companies = companyController.GetAllHubspotCompany();
            if (companies != null)
                return Request.CreateResponse(HttpStatusCode.OK, companies);
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        [Route("api/hubspot/companies/update")]
        public HttpResponseMessage HubspotCompaniesUpdate(List<CompanyHubspotUpdate> companies)
        {
            var updateCompany = companyController.UpdateHubspotCompanyInGroup(companies);
            if (updateCompany)
                return Request.CreateResponse(HttpStatusCode.OK);
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}

