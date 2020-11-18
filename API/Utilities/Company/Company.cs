using API.Data;
using API.Models;
using API.Services.HubspotService.Company.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace API.Utilities.Company
{
    public class Company
    {
        public static JObject CreateCompany(string json, string obj = null, string type = null)
        {
            if (type == "Rental" || type == "Sales")
            {
                Quote newQuote = JsonConvert.DeserializeObject<Quote>(json);

                return IsCompanyCreated(newQuote);
            }
            CompanyDTO companyDTO = JsonConvert.DeserializeObject<CompanyDTO>(json);
            ContactDTO contactDTO = JsonConvert.DeserializeObject<ContactDTO>(obj);

            return CreateCompanyAndContact(companyDTO, contactDTO);
        }

        private static JObject CreateCompanyAndContact(CompanyDTO companyDTO, ContactDTO contactDTO)
        {
            var db = DAL.GetInstance();
            string firstName = string.Empty;
            string lastName = string.Empty;
            string email = string.Empty;
            int accountManagerID = 0;

            //check company existence
            JObject sqlParamsForCheckCompany = new JObject{
                     {"CompanyName", companyDTO.properties.name.value}
                 };
            var companyContactID = db.getContactIDByCompanyName(sqlParamsForCheckCompany);

            //
            if (companyDTO.properties.hubspot_owner_id != null)
            {
                JObject jObject = new JObject
            {
                {"email", companyDTO.properties.hubspot_owner_id.sourceId }
            };
                accountManagerID = db.GetCustomerIDByEmail(jObject);
            }

            var companyExist = companyContactID != 0 ? true : false;
            string user = companyDTO.properties.hubspot_owner_id == null ? "Hubspot" : companyDTO.properties.hubspot_owner_id.sourceId.Substring(0,
                     companyDTO.properties.hubspot_owner_id.sourceId.LastIndexOf("@", StringComparison.Ordinal));
            if (companyContactID == 0)
            {
                JObject sqlParamsForCompanyContact = new JObject{
                    {"ContactType", "BusinessUnit"},
                     {"IsParent", 1},
                     {"FirstName", ""},
                     {"LastName",""},
                     {"BusinessPhone", companyDTO.properties.phone == null ? "" : companyDTO.properties.phone.value},
                     {"Email", ""},
                     {"MobilePhone", ""},
                     {"EnterDateTime", DateTime.Now.ToLocalTime()},
                     {"EditDateTime", DateTime.Now.ToLocalTime()},
                     {"EditUserStr", "WWM\\" + user},
                     {"EnterUserStr", "WWM\\" + user},
                     {"CompanyName", companyDTO.properties.name.value},
                     {"AccountManagerID", accountManagerID } //using chris as salesman, remove later

                    };
                companyContactID = db.CreateContact(sqlParamsForCompanyContact);
                if (companyContactID == 0) return null;
            }


            //create contact category for new company
            if (contactDTO.contacts.Count > 0)
            {
                JObject sqlParamsForCompanyCategory = new JObject{
                            {"ContactID", companyContactID},
                            {"ContactCategoryType", "Prospect"},
                            {"EnterUserStr", "WWM\\" + user}
                            };
                int contactCategoryID = db.CreateCompanyCategory(sqlParamsForCompanyCategory);
            }

            //create businees code category for new company
            if (contactDTO.contacts.Count > 0)
            {
                JObject sqlParamsForBCCategory = new JObject{
                            {"ContactID", companyContactID},
                            {"BusinessCodeName",  "Environmental"},
                            {"EnterUserStr", "WWM\\" + user}
                            };
                int contactCategoryID = db.CreateBCCategory(sqlParamsForBCCategory);
            }

            foreach (var item in contactDTO.contacts)
            {
                foreach (var name in item.properties)
                {
                    if (name.name == "firstname")
                    {
                        firstName = name.value;
                    }
                    else if (name.name == "lastname")
                    {
                        lastName = name.value;
                    }
                }
                foreach (var ident in item.identities)
                {
                    foreach (var prop in ident.identity)
                    {
                        if (prop.type.ToLower() == "email")
                        {
                            email = prop.value;
                        }
                    }
                }
                JObject sqlParamsForPersonContact = new JObject{
                    {"ContactType", "Person"},
                     {"IsParent",0},
                     {"FirstName", firstName},
                     {"LastName",lastName},
                     {"BusinessPhone", ""},
                     {"Email", email},
                     {"MobilePhone", ""},
                     {"EnterDateTime", DateTime.Now.ToLocalTime()},
                     {"EditDateTime", DateTime.Now.ToLocalTime()},
                     {"EditUserStr", "WWM\\" + user},
                     {"EnterUserStr", "WWM\\" + user},
                     {"CompanyName", companyDTO.properties.name.value},
                      {"AccountManagerID", accountManagerID}

                 };
                var personContactID = db.CreateContact(sqlParamsForPersonContact);
                if (personContactID == 0) return null;
                JObject sqlParamsForContactRelationship = new JObject{
                    {"ChildContactID", personContactID},
                     {"ParentContactID",companyContactID},
                     {"EnterDateTime", DateTime.Now.ToLocalTime()},
                     {"EditDateTime", DateTime.Now.ToLocalTime()},
                     {"EditUserStr", "WWM\\" + user},
                     {"EnterUserStr", "WWM\\" + user},

                 };
                var contactRelationshipID = db.CreateContactRelationship(sqlParamsForContactRelationship);
                //if (contactRelationshipID != 0)
                //{
                //    newQuote.ContactRelationshipID = personContactID;
                //}
                //else return null;
            }

            var obj = JsonConvert.SerializeObject(companyDTO);

            return JsonConvert.DeserializeObject<JObject>(obj);

        }



        private static JObject IsCompanyCreated(Quote newQuote)
        {
            var db = DAL.GetInstance();
            //check existing or new customer
#pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
            if (newQuote.ContactRelationshipID == null || newQuote.ContactRelationshipID == 0)  //if new customer create company contact, person contact, contactrelationship, if address create, address , contactaddressrelaship
#pragma warning restore CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
            {
                //check company existence
                JObject sqlParamsForCheckCompany = new JObject{
                     {"CompanyName", newQuote.Contact.CompanyName}
                 };
                var companyContactID = db.getContactIDByCompanyName(sqlParamsForCheckCompany);
                var companyExist = companyContactID != 0 ? true : false;
                if (companyContactID == 0)
                {
                    JObject sqlParamsForCompanyContact = new JObject{
                    {"ContactType", "BusinessUnit"},
                     {"IsParent", 1},
                     {"FirstName", ""},
                     {"LastName",""},
                     {"BusinessPhone", newQuote.Contact.BusinessPhone},
                     {"Email", ""},
                     {"MobilePhone", ""},
                     {"EnterDateTime", DateTime.Now.ToLocalTime()},
                     {"EditDateTime", DateTime.Now.ToLocalTime()},
                     {"EditUserStr", "WWM\\"+ newQuote.EnterUserStr},
                     {"EnterUserStr", "WWM\\"+ newQuote.EnterUserStr},
                     {"CompanyName", newQuote.Contact.CompanyName},
                     {"AccountManagerID", newQuote.AccountManagerID}

                    };
                    companyContactID = db.CreateContact(sqlParamsForCompanyContact);
                    if (companyContactID == 0) return null;

                    //create contact category for new company
                    if (newQuote.Contact != null && newQuote.Contact.CompanyCategory != null && newQuote.Contact.CompanyCategory.Length > 0)
                    {
                        foreach (string cate in newQuote.Contact.CompanyCategory)
                        {
                            JObject sqlParamsForCompanyCategory = new JObject{
                            {"ContactID", companyContactID},
                            {"ContactCategoryType", cate},
                            {"EnterUserStr", "WWM\\"+ newQuote.EnterUserStr}
                            };
                            int contactCategoryID = db.CreateCompanyCategory(sqlParamsForCompanyCategory);
                        }
                    }
                    //create businees code category for new company
                    if (newQuote.Contact != null && newQuote.Contact.BCCategory != null && newQuote.Contact.BCCategory.Length > 0)
                    {
                        foreach (string cate in newQuote.Contact.BCCategory)
                        {
                            JObject sqlParamsForBCCategory = new JObject{
                            {"ContactID", companyContactID},
                            {"BusinessCodeName", cate},
                            {"EnterUserStr", "WWM\\"+ newQuote.EnterUserStr}
                            };
                            int contactCategoryID = db.CreateBCCategory(sqlParamsForBCCategory);
                        }
                    }

                }

                JObject sqlParamsForPersonContact = new JObject{
                    {"ContactType", "Person"},
                     {"IsParent",0},
                     {"FirstName", newQuote.Contact.FirstName},
                     {"LastName",newQuote.Contact.LastName},
                     {"BusinessPhone", newQuote.Contact.BusinessPhone},
                     {"Email", newQuote.Contact.Email},
                     {"MobilePhone", newQuote.Contact.MobilePhone},
                     {"EnterDateTime", DateTime.Now.ToLocalTime()},
                     {"EditDateTime", DateTime.Now.ToLocalTime()},
                     {"EditUserStr", "WWM\\"+ newQuote.EnterUserStr},
                     {"EnterUserStr", "WWM\\"+ newQuote.EnterUserStr},
                     {"CompanyName", newQuote.Contact.CompanyName},
                      {"AccountManagerID", newQuote.AccountManagerID}

                 };
                var personContactID = db.CreateContact(sqlParamsForPersonContact);
                if (personContactID == 0) return null;
                JObject sqlParamsForContactRelationship = new JObject{
                    {"ChildContactID", personContactID},
                     {"ParentContactID",companyContactID},
                     {"EnterDateTime", DateTime.Now.ToLocalTime()},
                     {"EditDateTime", DateTime.Now.ToLocalTime()},
                     {"EditUserStr", "WWM\\"+ newQuote.EnterUserStr},
                     {"EnterUserStr", "WWM\\"+ newQuote.EnterUserStr},

                 };
                var contactRelationshipID = db.CreateContactRelationship(sqlParamsForContactRelationship);
                if (contactRelationshipID != 0)
                {
                    newQuote.ContactRelationshipID = personContactID;
                }
                else return null;

                //create address and contactaddressrelationship
                if (!companyExist)  //exist company don't need create address
                {
                    if (newQuote.Contact.ContactAddr.Street1 != "" && newQuote.Contact.ContactAddr.City != "" && newQuote.Contact.ContactAddr.CountryName != "")
                    {
                        JObject sqlParamsForAddress = new JObject{
                     {"Address1", newQuote.Contact.ContactAddr.Street1},
                     {"Address2",newQuote.Contact.ContactAddr.Street2},
                     {"City", newQuote.Contact.ContactAddr.City},
                     {"StateCode",newQuote.Contact.ContactAddr.State},
                     {"PostalCode", newQuote.Contact.ContactAddr.PostalCode},
                     {"CountryName", newQuote.Contact.ContactAddr.CountryName},
                     {"EnterDateTime", DateTime.Now.ToLocalTime()},
                     {"EditDateTime", DateTime.Now.ToLocalTime()},
                     {"EditUserStr", "WWM\\"+ newQuote.EnterUserStr},
                     {"EnterUserStr", "WWM\\"+ newQuote.EnterUserStr},

                    };
                        var addressID = db.CreateAddress(sqlParamsForAddress);
                        if (addressID != 0)
                        {
                            JObject sqlParamsForContactAddressRelationshipCompany = new JObject{
                         {"ContactID", companyContactID},
                         {"AddressID", addressID},
                         {"EnterDateTime", DateTime.Now.ToLocalTime()},
                         {"EditDateTime", DateTime.Now.ToLocalTime()},
                         {"EditUserStr", "WWM\\"+ newQuote.EnterUserStr},
                         {"EnterUserStr", "WWM\\"+ newQuote.EnterUserStr},

                        };
                            var ContactAddressRelationshipCompany = db.CreateContactAddressRelationship(sqlParamsForContactAddressRelationshipCompany);

                            JObject sqlParamsForContactAddressRelationshipPerson = new JObject{
                         {"ContactID", personContactID},
                         {"AddressID",addressID},
                         {"EnterDateTime", DateTime.Now.ToLocalTime()},
                         {"EditDateTime", DateTime.Now.ToLocalTime()},
                         {"EditUserStr", "WWM\\"+ newQuote.EnterUserStr},
                         {"EnterUserStr", "WWM\\"+ newQuote.EnterUserStr},

                        };
                            var ContactAddressRelationshipPerson = db.CreateContactAddressRelationship(sqlParamsForContactAddressRelationshipPerson);
                        }//contactaddressrelationship end

                    }//address end
                }



            }//contactrelationship end

            var obj = JsonConvert.SerializeObject(newQuote);

            return JsonConvert.DeserializeObject<JObject>(obj);
        }
    }
}