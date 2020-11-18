using API.Data;
using API.Models;
using API.Utilities;
using API.Utilities.Auth;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Managers
{
    public static class OpportunityManager
    {
        public static int Create(OpportunityMobile oppo)
        {
            var db = DAL.GetInstance();
            int opportunityID = 0;
            if(oppo.QuoteID == 0)
            {
                JObject sqlParamsForOpportunity = new JObject{
                         {"OpportunityType", oppo.OpportunityType},
                         {"Customer",oppo.Customer},
                         {"Remarks", oppo.Remarks},
                         {"Division", oppo.Division},
                         {"AccoutManagerID", oppo.AccountManagerID},
                         {"RentalCoordinatorID", oppo.RentalCoordinatorID},
                         {"SalesManagerID", oppo.SalesManagerID},
                         {"QuoteID", oppo.QuoteID},
                         {"EnterUserStr", "WWM\\"+ oppo.EnterUserStr}
                  
                        };
                opportunityID = db.CreateNewOpportunity(sqlParamsForOpportunity);
            }
            else
            {
                JObject json = new JObject{{"QuoteID", oppo.QuoteID}};
                Quote quote = db.getQuoteByQuoteID(json);
                JObject sqlParamsForOpportunity = new JObject{
                         {"OpportunityType", oppo.OpportunityType},
                         {"Customer",oppo.Customer},
                         {"Remarks", oppo.Remarks},
                         {"Division", oppo.Division},
                         {"AccoutManagerID", quote.AccountManagerID},
                         {"RentalCoordinatorID", quote.CoordinatorID},
                         {"SalesManagerID", quote.SalesManager},
                         {"QuoteID", oppo.QuoteID},
                         {"EnterUserStr", "WWM\\"+ oppo.EnterUserStr}
                  
                        };
                opportunityID = db.CreateNewOpportunity(sqlParamsForOpportunity);
            }
           

            if (opportunityID != 0)
            {

                foreach (OpportunityItemMobile oi in oppo.Equipments)
                {
                    JObject sqlParamsForOpportunityItems = new JObject{
                         {"OpportunityID", opportunityID},
                         {"Quantity", oi.Quantity},
                         {"Category", oi.Category},
                         {"Manufacturer", oi.Manufacturer},
                         {"ModelNum", oi.ModelNum},
                         {"Reason", oi.Reason},
                         {"QuotedRate", oi.QuotedRate},
                         {"QuotedRateUOM", oi.QuotedRateUOM},
                         {"RentalTerm", oi.RentalTerm},
                         {"RentalTermUOM", oi.RentalTermUOM},
                         {"EnterUserStr", "WWM\\"+ oppo.EnterUserStr}
                     
                        };
                    var opportunityItemID = db.CreateNewOpportunityItem(sqlParamsForOpportunityItems);

                    if (opportunityItemID != 0)
                    {
                        foreach (OpportunityAttachmentsOfItem att in oi.Attachments)
                        {
                            JObject sqlParamsForOpportunityItemsAttachment = new JObject{
                                 {"OpportunityItemID", opportunityItemID},
                                 {"Category",att.Category},
                                 {"Type", att.Type},
                                 {"ModelNum", att.ModelNum},
                                 {"Position", att.Position},
                                 {"EnterUserStr", "WWM\\"+ oppo.EnterUserStr}
                  
                                };

                            var opportunityItemAttachmentID = db.CreateNewOpportunityItemAttchment(sqlParamsForOpportunityItemsAttachment);
                        }
                    }
                }
                
            }
            return opportunityID;
        }

        public static bool Update(OpportunityMobile oppo)
        {
            try
            {
                var db = DAL.GetInstance();
                bool opportunityID = false;
                if (oppo.QuoteID == 0)
                {
                    JObject sqlParamsForOpportunity = new JObject{
                         {"OpportunityType", oppo.OpportunityType},
                         {"Customer",oppo.Customer},
                         {"Remarks", oppo.Remarks},
                         {"Division", oppo.Division},
                         {"AccoutManagerID", oppo.AccountManagerID},
                         {"RentalCoordinatorID", oppo.RentalCoordinatorID},
                         {"SalesManagerID", oppo.SalesManagerID},
                         {"QuoteID", oppo.QuoteID},
                         {"EnterUserStr", "WWM\\"+ oppo.EnterUserStr},
                         {"OpportunityID", oppo.OpportunityID},
                        };
                    opportunityID = db.UpdateOpportunity(sqlParamsForOpportunity);
                }
                else
                {
                    JObject json = new JObject { { "QuoteID", oppo.QuoteID } };
                    Quote quote = db.getQuoteByQuoteID(json);
                    JObject sqlParamsForOpportunity = new JObject{
                         {"OpportunityType", oppo.OpportunityType},
                         {"Customer",oppo.Customer},
                         {"Remarks", oppo.Remarks},
                         {"Division", oppo.Division},
                         {"AccoutManagerID", quote.AccountManagerID},
                         {"RentalCoordinatorID", quote.CoordinatorID},
                         {"SalesManagerID", quote.SalesManager},
                         {"QuoteID", oppo.QuoteID},
                         {"EnterUserStr", "WWM\\"+ oppo.EnterUserStr},
                         {"OpportunityID", oppo.OpportunityID},
                    
                        };
                    opportunityID = db.UpdateOpportunity(sqlParamsForOpportunity);
                }


                if (opportunityID)
                {

                    foreach (OpportunityItemMobile oi in oppo.Equipments)
                    {
                        if(oi.OpportunityItemID ==0)
                        {
                            JObject sqlParamsForOpportunityItems = new JObject{
                             {"OpportunityID", oppo.OpportunityID},
                             {"Quantity", oi.Quantity},
                             {"Category", oi.Category},
                             {"Manufacturer", oi.Manufacturer},
                             {"ModelNum", oi.ModelNum},
                             {"Reason", oi.Reason},
                             {"QuotedRate", oi.QuotedRate},
                             {"QuotedRateUOM", oi.QuotedRateUOM},
                             {"RentalTerm", oi.RentalTerm},
                             {"RentalTermUOM", oi.RentalTermUOM},
                             {"EnterUserStr", "WWM\\"+ oppo.EnterUserStr}
                     
                            };
                            var opportunityItemID = db.CreateNewOpportunityItem(sqlParamsForOpportunityItems);
                            if (opportunityItemID != 0)
                            {
                                foreach (OpportunityAttachmentsOfItem att in oi.Attachments)
                                {
                                    JObject sqlParamsForOpportunityItemsAttachment = new JObject{
                                 {"OpportunityItemID", opportunityItemID},
                                 {"Category",att.Category},
                                 {"Type", att.Type},
                                 {"ModelNum", att.ModelNum},
                                 {"Position", att.Position},
                                 {"EnterUserStr", "WWM\\"+ oppo.EnterUserStr}
                  
                                };

                                    var opportunityItemAttachmentID = db.CreateNewOpportunityItemAttchment(sqlParamsForOpportunityItemsAttachment);
                                }
                            }
                        }
                        else if(!oi.Active)
                        {
                            JObject sqlParamsForOpportunityItems = new JObject{
                             {"OpportunityItemID", oi.OpportunityItemID},
                             {"EnterUserStr", "WWM\\"+ oppo.EnterUserStr},
                            };
                            db.DeactiveOpportunityItem(sqlParamsForOpportunityItems);
                        }
                        else
                        {
                            JObject sqlParamsForOpportunityItems = new JObject{
                             {"OpportunityID", oppo.OpportunityID},
                             {"Quantity", oi.Quantity},
                             {"Category", oi.Category},
                             {"Manufacturer", oi.Manufacturer},
                             {"ModelNum", oi.ModelNum},
                             {"Reason", oi.Reason},
                             {"QuotedRate", oi.QuotedRate},
                             {"QuotedRateUOM", oi.QuotedRateUOM},
                             {"RentalTerm", oi.RentalTerm},
                             {"RentalTermUOM", oi.RentalTermUOM},
                             {"EnterUserStr", "WWM\\"+ oppo.EnterUserStr},
                             {"OpportunityItemID", oi.OpportunityItemID},
                     
                            };
                            var opportunityItemIDFlag = db.UpdateOpportunityItem(sqlParamsForOpportunityItems);

                            if (opportunityItemIDFlag)
                            {
                                foreach (OpportunityAttachmentsOfItem att in oi.Attachments)
                                {
                                    if(att.OpportunityAttachmentsOfItemID ==0)
                                    {
                                        JObject sqlParamsForOpportunityItemsAttachment = new JObject{
                                         {"OpportunityItemID", oi.OpportunityItemID},
                                         {"Category",att.Category},
                                         {"Type", att.Type},
                                         {"ModelNum", att.ModelNum},
                                         {"Position", att.Position},
                                         {"EnterUserStr", "WWM\\"+ oppo.EnterUserStr}
                  
                                        };

                                        var opportunityItemAttachmentID = db.CreateNewOpportunityItemAttchment(sqlParamsForOpportunityItemsAttachment);
                                    }
                                    else if(!att.Active)
                                    {
                                        JObject sqlParamsForOpportunityItemsatt = new JObject{
                                         {"OpportunityAttachmentsOfItemID", att.OpportunityAttachmentsOfItemID},
                                         {"EnterUserStr", "WWM\\"+ oppo.EnterUserStr},
                                        };
                                        db.DeactiveOpportunityAttachemntofItem(sqlParamsForOpportunityItemsatt);
                                    }
                                    else
                                    {
                                        JObject sqlParamsForOpportunityItemsAttachment = new JObject{
                                         {"OpportunityItemID", oi.OpportunityItemID},
                                         {"Category",att.Category},
                                         {"Type", att.Type},
                                         {"ModelNum", att.ModelNum},
                                         {"Position", att.Position},
                                         {"EnterUserStr", "WWM\\"+ oppo.EnterUserStr},
                                         {"OpportunityAttachmentsOfItemID", att.OpportunityAttachmentsOfItemID}
                                        };

                                        var opportunityItemAttachmentID = db.UpdateOpportunityItemAttchment(sqlParamsForOpportunityItemsAttachment);



                                    }
                                 }
                                   
                            }
                        }
                        

                        
                    }

                }
                return true;
            }
            
            catch(Exception e)
            {
                return false;
            }
        }
       
    }
}
