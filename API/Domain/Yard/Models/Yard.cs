using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class Yard
    {
        public string YardName { get; set; }
        public List<string> SubYards { get; set; }
    }
}