using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class WebhookDTO
    {
        public long objectId { get; set; }
        public string propertyName { get; set; }
        public string propertyValue { get; set; }
        public string changeSource { get; set; }
        public long eventId { get; set; }
        public int subscriptionId { get; set; }
        public int portalId { get; set; }
        public int appId { get; set; }
        public long occurredAt { get; set; }
        public string subscriptionType { get; set; }
        public int attemptNumber { get; set; }
        public string changeFlag { get; set; }
    }

    public class DealStages
    {
        public string Name { get; set; }
        public string value { get; set; }
    }
}