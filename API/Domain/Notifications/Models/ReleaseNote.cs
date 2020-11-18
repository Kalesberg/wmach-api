using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class Releases
    {
        public string ReleaseVersion { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<ReleaseNote> ReleaseNote { get; set; }
    }

    public class ReleaseNote
    {
        public string ReleaseVersion { get; set; }
        public string ChangeType { get; set; }
        public List<string> Changes { get; set; }
    }
 }