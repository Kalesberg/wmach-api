using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class EmailGroup
    {
        public int EmailGroupID { get; set; }
        public string EmailGroupName { get; set; }
        public int EmailGroupMemberID {get; set;}
        public bool Active {get; set;}
        public int EmailGroupOwnerID {get; set;}
        public string EmailGroupOwnerName {get; set; }
        public DateTime? EditUserStr {get; set;}
        public DateTime? EditDateTime {get; set;}
    }

    public class EmailGroupMember
    {
    }
}