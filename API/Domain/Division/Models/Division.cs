using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class Division
    {
        public string parent { get; set; }
        public List<string> children { get; set; }
    }
    public class DivisionDetail
    {
        public string DivisionShortName { get; set; }
        public string DivisionName { get; set; }
        public string DivisionImageFile { get; set; }
        public string DivisionImageFileBlackWhite { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string PostalCode { get; set; }
        public string CountryName { get; set; }
        public string BusinessPhone { get; set; }
        public string BusinessFax { get; set; }
        public string URL { get; set; }

    }
}