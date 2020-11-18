using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models;
using Newtonsoft.Json.Linq;

namespace API.Data
{
    //this will provide all the necessary detail for the mobile front end to render
    public class OpportunityDetail : IBuildParams<OpportunityDetail>
    {
        public OpportunityMobile Opportunity;
        public Quote Quote;
        public Contact AccountManager;
        public Contact RentalCoordinator;
        public Contact SalesManager;
        public DivisionDetail DivisionDetail;
        public Contact Company;
        public Address Address;

        public void Build(JObject sqlParams)
        {
            Opportunity = DAL.GetInstance().getOpportunitySelect(sqlParams);
            if (Opportunity != null && Opportunity.Active)
            {
                if (Opportunity.SalesManagerID != 0)
                    GetContactInfo(Opportunity.SalesManagerID, ref SalesManager);
                if (Opportunity.RentalCoordinatorID != 0)
                    GetContactInfo(Opportunity.RentalCoordinatorID, ref RentalCoordinator);
                if (Opportunity.AccountManagerID != 0)
                    GetContactInfo(Opportunity.AccountManagerID, ref AccountManager);
                if (Opportunity.QuoteID != 0)
                {
                    GetQuoteDetail(Opportunity.QuoteID);
                }
                getOpportunityItem(sqlParams);

                foreach (OpportunityItemMobile oi in Opportunity.Equipments)
                {
                    oi.Attachments = GetOpportunityAttachmentOfItems(oi.OpportunityItemID);
                }
                GetDivisionDetailInfo(Opportunity.DivisionID);
            }
            else
                Opportunity = null;
            
        }

        public OpportunityDetail GetResult()
        {
            return this;
        }

        private void GetContactInfo(int contactID, ref Contact contact)
        {
            var json = new JObject { { "ContactID", contactID } };
            contact = DAL.GetInstance().getContactByContactID(json);
        }

        private void GetQuoteDetail(int quoteid)
        {
            var sqlParams = new JObject { { "QuoteID", quoteid } };
            Quote quoted = new Quote();
            Quote = DAL.GetInstance().getQuoteByQuoteID(sqlParams);
            var json = new JObject { { "ContactRelationshipID", Quote.ContactRelationshipID } };
            ContactRelationship ContactRelationship = DAL.GetInstance().getContactRelationshipByContactRelationshipID(json);
            var json1 = new JObject { { "ContactID", ContactRelationship.ParentContactID } };
            Company = DAL.GetInstance().getContactByContactID(json1);
            Address = DAL.GetInstance().getAddressByContactID(json1).FirstOrDefault();
        }

        private void getOpportunityItem(JObject sqlParam)
        {
            Opportunity.Equipments = DAL.GetInstance().getOpportunityItemSelect(sqlParam);

        }

        private List<OpportunityAttachmentsOfItem> GetOpportunityAttachmentOfItems(int OpportunityItemID)
        {
            var json = new JObject { { "OpportunityItemID", OpportunityItemID } };
            return DAL.GetInstance().getOpportunityAttachmentsOfItemSelect(json);
        }

        private void GetDivisionDetailInfo(int DivisionID)
        {
            var json = new JObject { { "DivisionID", DivisionID } };
            DivisionDetail = DAL.GetInstance().getDivisionDetailByDivisionID(json);
        }

    }
}