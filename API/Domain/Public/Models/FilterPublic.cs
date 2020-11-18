using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class FilterPublic
    {
        public int ID { get; set; }
        public string Value { get; set; }
        public bool Visible { get; set; }
    }

    public class Categories : FilterPublic
    {
        public List<Makes> Makes { get; set; }
    }

    public class Makes : FilterPublic
    {
        public List<FilterPublic> Models { get; set; }
    }
}