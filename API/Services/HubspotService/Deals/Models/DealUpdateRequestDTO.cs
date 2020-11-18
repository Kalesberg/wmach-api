using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Services.HubspotService.Deals.Models
{
    public class DealUpdateRequestDTO
    {
        public long DealId { get; set; }
        public decimal Amount { get; set; }
        public string DealEquipmentDescription { get; set; }
        public string DealStage { get; set; }
        public string QuoteType { get; set; }
        public string QuoteNumber { get; set; }
        public long Closedate { get; set; }
        public string QuoteUrl { get; set; }
        public decimal MaxExpectedAmount { get; set; }
        public int QuoteID { get; set; }
    }

    public class DealCreateRequestDTO : DealUpdateRequestDTO
    {
        public string contactHubspotEmail { get; set; }
        public string dealname { get; set; }
        public string Pipeline { get; set; }
        public string hubspot_owner_id { get; set; }
        public string hubspot_owner_email { get; set; }
    }

    public class ContactHubspot 
    {
       public long vid { get; set; }
        public PropertyContact properties { get; set; }
    }

    public class PropertyContact
    {
        public HubspotProperty associatedcompanyid
        { get; set; }
    }

        public class HubspotProperty
        {
            public string value { get; set; }
        }

    public class DealUpdateRequestDTOForContract
    {
        public long DealId { get; set; }
        public decimal Amount { get; set; }
        public string DealEquipmentDescription { get; set; }
        public string DealStage { get; set; }
        public string ContractType { get; set; }
        public string ContractNumber { get; set; }
        public long Closedate { get; set; }
        public string ContractUrl { get; set; }
        public decimal MaxExpectedAmount { get; set; }
        public int ContractID { get; set; }
    }


}