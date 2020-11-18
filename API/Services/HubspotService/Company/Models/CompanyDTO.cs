using Newtonsoft.Json;
using System.Collections.Generic;

namespace API.Services.HubspotService.Company.Models
{
    public class CompanyDTO
    {
        public long companyId { get; set; }
        public bool isDeleted { get; set; }
        public Properties properties { get; set; }

    }

    public class Properties
    {
        public Name name { get; set; }
        public Country country { get; set; }
        public City city { get; set; }
        public State state { get; set; }
        public Zip zip { get; set; }
        public Address address { get; set; }
        public Phone phone { get; set; }
        public Domain domain { get; set; }
        public HubspotOwner hubspot_owner_id { get; set; }
        public Industry industry { get; set; }
    }

    public class Industry
    {
        public string value { get; set; }
    }

    public class HubspotOwner
    {
        //public string value { get; set; }
        public string sourceId { get; set; }
    }

    public class Country
    {
        public string value { get; set; }
    }
    public class City
    {
        public string value { get; set; }
    }
    public class State
    {
        public string value { get; set; }
    }
    public class Zip
    {
        public string value { get; set; }
    }
    public class Address
    {
        public string value { get; set; }
    }
    public class Phone
    {
        public string value { get; set; }
    }
    public class Domain
    {
        public string value { get; set; }
    }
    public class Name
    {
        public string value { get; set; }
    }

    public class ContactDTO
    {
        public List<Contact> contacts { get; set; }
        public int vidOffset { get; set; }
        public bool hasMore { get; set; }
    }

    public class Contact
    {
        public List<Identities> identities { get; set; }
        public List<Props> properties { get; set; }
    }

    public class Props
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Identities
    {
        public List<Identity> identity { get; set; }
    }

    public class Identity
    {
        public string value { get; set; }
        public string type { get; set; }
    }

    public class Deal
    {
        public long dealId { get; set; }
        public Associations associations { get; set; }
        public DealProperties properties { get; set; }
    }
    public class Associations
    {
        public List<int> associatedVids { get; set; }
        public List<long> associatedCompanyIds { get; set; }
        public List<long> associatedDealIds { get; set; }
        public List<long> associatedTicketIds { get; set; }

    }

    public class DealProperties
    {
        public Props dealname { get; set; }
        public Props coordinator { get; set; }
        public Props sales_manager { get; set; }
        public Props division { get; set; }
        public Props hubspot_owner_id { get; set; }
        public Props amount { get; set; }
        public Props est_start_date { get; set; }
        public Props min_rental_term_period { get; set; }
        public Props min_rental_term { get; set; }
        public Props dealstage { get; set; }
        public Props job_site { get; set; }
        public Props quote_date { get; set; }
        public Props term_and_conditions { get; set; }
    }

    public class Dealname
    {
        public string value { get; set; }
        public string type { get; set; }
    }

    public class LineItem
    {
        public long objectId { get; set; }
        public LineItemProperties properties { get; set; }
    }
    public class LineItemProperties
    {
        public Props quantity { get; set; }
        public Props price { get; set; }
        public Props name { get; set; }
        public Props hs_product_id { get; set; }
    }

    public class LineItemID
    {
        public List<long> results { get; set; }
    }

    public class ContactDetail
    {
        public ContactProperties properties { get; set; }
        [JsonProperty("associated-company")]
        public AssociatedCompany associatedcompany { get; set; }
    }

    public class ContactProperties
    {
        public Props firstname { get; set; }
        public Props lastname { get; set; }
        public Props associatedcompanyid { get; set; }

    }

    public class AssociatedCompany
    {
        [JsonProperty("company-id")]
        public long companyid { get; set; }
        public AssociatedCompanyProperties properties { get; set; }
    }
    public class AssociatedCompanyProperties
    {
        public Props name { get; set; }
        public Country country { get; set; }
        public City city { get; set; }
        public State state { get; set; }
        public Zip zip { get; set; }
        public Address address { get; set; }
        public Phone phone { get; set; }
        public Domain domain { get; set; }
        public HubspotOwner hubspot_owner_id { get; set; }
        public Industry industry { get; set; }
    }

    public class Owner
    {
        public int  portalId { get; set; }
        public int  ownerId { get; set; }
        public string  firstName { get; set; }
        public string  lastName { get; set; }
        public string  email { get; set; }
        public string  type { get; set; }
    }

}