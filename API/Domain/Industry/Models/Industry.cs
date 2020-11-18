using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.SessionState;

namespace API.Models
{
    public class IndustryGet
    {
        public int IndustryID { get; set; }
        public bool Active { get; set; }
        public string IndustryShortName { get; set; }
        public string IndustryDescription { get; set; }
    }
}