using API.Services.HubspotService.Company.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Services.HubspotService.Deals.Models
{
    public class DealUpdateJson
    {
        public List<Props> properties { get; set; }
        public AssociationsContact associations { get; set; }
    }
    public class AssociationsContact
    {
        public List<long> associatedCompanyIds { get; set; }
        public List<long> associatedVids { get; set; }

    }
}