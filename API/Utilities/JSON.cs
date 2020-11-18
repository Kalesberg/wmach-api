using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Utilities
{
    public static class JSON
    {
        public static JObject Parse(dynamic value, string propName)
        {
            var jObj = new JObject();
            jObj.Add(propName, value);
            return jObj;
        }
    }
}