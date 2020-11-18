using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace API.Services.HubspotService.Deals.Models
{
    public class DealResponse
    { 
        public List<DealDTO> deals { get; set; }
        public bool hasMore { get; set; }
        public string offset { get; set; }
    }
    public class DealDTO
    {
        public long dealId { get; set; }
        public Property properties { get; set; }
    }

    public class Property
    {
        public DealName dealname { get; set; }
        public DealStage dealstage { get; set; }
        public DealOwner hubspot_owner_id { get; set; }
    }
    public class DealName
    {
        public string sourceId { get; set; }
        public string value { get; set; }
    }
    public class DealStage
    {
        public string value { get; set; }
    }
    public class DealOwner
    {
        public string value { get; set; }
    }

    public class CompanyResponse
    {
        public List<CompanyHubspotDTO> companies { get; set; }
        [JsonProperty(PropertyName = "has-more")]
        public bool hasMore { get; set; }
        public string offset { get; set; }
    }
    public class CompanyHubspotDTO
    {
        public long companyId { get; set; }
        public CompanyProperty properties { get; set; }
    }

    public class CompanyProperty
    {
        public CompanyValue name { get; set; }
        [JsonProperty(PropertyName = "ckms_site_id__c")]
        public CompanyValue dodge_site_ID { get; set; }
     
    }
    public class CompanyValue
    {
        public string value { get; set; }
    }

    public class CompanyHubspotUpdate
    {
       public long objectId { get; set; }
        public List<HubspotPropertyName> properties { get; set; }

    }

    public class HubspotPropertyName
    {
        public string name { get; set; }
        public string value { get; set; }
    }


}