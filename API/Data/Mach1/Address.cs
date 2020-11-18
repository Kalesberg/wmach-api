//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace API.Data.Mach1
{
    using System;
    using System.Collections.Generic;
    
    public partial class Address
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Address()
        {
            this.ContactAddressRelationships = new HashSet<ContactAddressRelationship>();
        }
    
        public int AddressID { get; set; }
        public string AddressType { get; set; }
        public bool MailingAddress { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Address5 { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string PostalCode { get; set; }
        public string County { get; set; }
        public string CountryName { get; set; }
        public bool Active { get; set; }
        public string EnterUserStr { get; set; }
        public System.DateTime EnterDateTime { get; set; }
        public string EditUserStr { get; set; }
        public System.DateTime EditDateTime { get; set; }
        public byte[] TimeStamp { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContactAddressRelationship> ContactAddressRelationships { get; set; }
    }
}
