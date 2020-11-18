using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class MemberShipUser
    {

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MyAccountPassword { get; set; }

        public bool AccesstoPortal { get; set; }

        public bool ResetPwd { get; set; }

        public string Role { get; set; }

        public int CustomerID { get; set; }

        public string CompanyName { get; set; }

        public string TimeStampEncrypted { get; set; }

    }
}
