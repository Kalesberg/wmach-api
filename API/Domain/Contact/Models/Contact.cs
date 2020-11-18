using System.Collections.Generic;

namespace API.Models
{
    public class Contact
    {
        public Contact()
        {
            Groups = new List<string>();
        }
        public int? ContactId { get; set; }
        public string UserName { get; set; }
        public string BusinessPhone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string DivisionImageURI { get; set; }
        public bool IsDeveloper { get; set; }
        public Address ContactAddress { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public string[] CompanyCategory { get; set; }
        public string[] BCCategory { get; set; }
        public ContactAddress ContactAddr { get; set; }
        public List<string> Groups { get; set; }
    }

    public class Salesperson
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Division { get; set; }
    }

    public class BusinessCode
    {
        public string parentName { get; set; }
        public string childName { get; set; }
    }

    public class BusinessCodeHierarchy
    {
        public string parent { get; set; }
        public List<string> children { get; set; }
    }


    public class ContactRelationship
    {
        public int ContactRelationshipID { get; set; }
        public int ChildContactID { get; set; }
        public int ParentContactID { get; set; }
    }
}