using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class Dash
    {
        public string ProductCategoryName { get; set; }
        public int InBoundAquisition { get; set; }
        public int InBoundTransit { get; set; }
        public int InYard { get; set; }
        public int OffSiteStorage { get; set; }
        public int OnJobSite { get; set; }
        public int OutBoundTransit { get; set; }
        public int PendingOutboundTransit { get; set; }
        public int Unknown { get; set; }
        public int LocationStatusTotal { get; set; }
        public string ManufacturerName { get; set; }
        public string ModelNum { get; set; }
        public string DivisionShortName { get; set; }
    }
}