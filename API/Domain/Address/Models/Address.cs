using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class Address
    {
        public string street {  get; set;}
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
    }

    public class ContactAddress
    {
        public int AddressID { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string CountryName { get; set; }
    }
}