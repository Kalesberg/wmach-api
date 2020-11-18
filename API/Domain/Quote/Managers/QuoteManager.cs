using API.Data;
using API.Models;
using API.Services.HubspotService.Deals.Helper;
using API.Services.HubspotService.Deals.Models;
using API.Utilities;
using API.Utilities.Auth;
using API.Utilities.Company;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace API.Managers
{
    public static class QuoteManager
    {
        public static Quote CreateQuote(Quote newQuote)
        {
            //GRAB USER DETAILS
            var db = DAL.GetInstance();

            //Contact Utility
            var json = JsonConvert.SerializeObject(newQuote);
            var contactRec = Company.CreateCompany(json, null, newQuote.QuoteType);
            var quoteJson = JsonConvert.SerializeObject(contactRec);
            Quote quoteModel = JsonConvert.DeserializeObject<Quote>(quoteJson);


            int QuoteID = 0;
            if (contactRec != null && newQuote.QuoteType == "Rental")
            {
                JObject sqlParams = new JObject{
                 {"QuoteStatus", newQuote.QuoteStatus},
                 {"RentalOrSales", newQuote.QuoteType},
                 //{"CompanyName", newQuote.account},
                 //{"CustomerName", newQuote},  those are parameter we realy don't need to create a quote and quotedetails
                 //{"CompanyAddress", newQuote},
                 //{"CustomerEmail", newQuote},
                 //{"BuissnessPhone", newQuote},
                 //{"MobielPhone", newQuote},
                 {"DivisionShortName", newQuote.division},
                 //{"DivisionLocation", newQuote},
                 {"Saleperson","WWM\\"+ newQuote.EnterUserStr},
                 {"TermsAndConditions", newQuote.Terms},
                 {"QuoteEstimatedStartTime", newQuote.startDate},
                 {"QuoteExpireTime", newQuote.ExpirationDate},
                 // {"QuoteEstimatedStartTime", "2009-05-11 00:00:00.000"},
                 //{"QuoteExpireTime", "2009-05-11 00:00:00.000"},
                
                 //{"CustomerNotes", newQuote},
                 {"ShowWeight", newQuote.ShowWeight},
                 {"ShowQuantity", newQuote.ShowQuantity},
                 {"ShowPicture", newQuote.ShowPicture},
                 {"ShowSerialNumber", newQuote.ShowSerialNumber},
                 {"ShowTotal", newQuote.ShowTotal},
                 {"IncludeCurrentLocation", newQuote.IncludeCurrentLocation},
                 {"IncludeComponents", newQuote.IncludeComponents},
                 {"IncludeMarketingBlurb", newQuote.IncludeMarketingBlurb},
                 {"IncludeMachineSpecifications", newQuote.IncludeMachineSpecifications},
                 {"ShowPhotoLink", newQuote.ShowPhotoLink},
                 {"ShowFreight", newQuote.ShowFreight},
                 {"ShowCell", newQuote.ShowCell},
                 {"ShowFootRate", newQuote.ShowFootRate},
                 {"ContactRelationshipID", quoteModel.ContactRelationshipID},
                 {"FOB", newQuote.FOB},
                 {"DealID", newQuote.DealID},
                 //{"ShowInsuranceValue", newQuote.},
                 {"ShowMonthlyRate", newQuote.ShowMonthlyRate},
                 {"ShowWeeklyRate", newQuote.ShowWeeklyRate},
                 {"ShowDailyRate", newQuote.ShowDailyRate},
                 {"ShowOvertimeRate", newQuote.ShowOvertimeRate},
                 {"Jobsite", newQuote.jobSite},
                 {"Format", newQuote.Format},
                 {"MinimumTerm", newQuote.MinimumTerm},
                 {"MinimumTermUOM", newQuote.MinimumTermUOM.ToLower()},
                 {"CoordinatorID",newQuote.CoordinatorID},
                  {"SalesManager",newQuote.SalesManager},
                 {"AccountManagerID",newQuote.AccountManagerID},
                  {"QuoteDetailType",newQuote.QuoteDetailType},
                  {"CurrencyRegion",newQuote.CurrencyRegion},
                  {"CurrencyType",newQuote.CurrencyType},
                   {"RPORate",newQuote.RPORate},
                  {"RPOOptionTerms",newQuote.RPOOptionTerms}

                 };
                QuoteID = db.CreateQuote(sqlParams);
            }
            else if (contactRec != null && newQuote.QuoteType == "Sales")
            {
                if (newQuote.MinimumTermUOM == null)
                    newQuote.MinimumTermUOM = "month";
                JObject sqlParams = new JObject{
                 {"QuoteStatus", newQuote.QuoteStatus},
                 {"RentalOrSales", newQuote.QuoteType},
                 {"DivisionShortName", newQuote.division},
                 {"Saleperson","WWM\\"+ newQuote.EnterUserStr},
                 {"TermsAndConditions", newQuote.Terms},
                  {"QuoteEstimatedStartTime", newQuote.startDate},
                 {"QuoteExpireTime", newQuote.ExpirationDate},
                 //{"ShowWeight", newQuote.ShowWeight},
                 //{"ShowQuantity", newQuote.ShowQuantity},
                 //{"ShowPicture", newQuote.ShowPicture},
                 //{"ShowSerialNumber", newQuote.ShowSerialNumber},
                 //{"ShowTotal", newQuote.ShowTotal},
                 //{"IncludeCurrentLocation", newQuote.IncludeCurrentLocation},
                 //{"IncludeComponents", newQuote.IncludeComponents},
                 //{"IncludeMarketingBlurb", newQuote.IncludeMarketingBlurb},
                 //{"IncludeMachineSpecifications", newQuote.IncludeMachineSpecifications},
                 //{"ShowPhotoLink", newQuote.ShowPhotoLink},
                 //{"ShowFreight", newQuote.ShowFreight},
                 //{"ShowCell", newQuote.ShowCell},
                 {"ContactRelationshipID", quoteModel.ContactRelationshipID},
                 {"FOB", newQuote.FOB},
                 {"MinimumTerm", newQuote.MinimumTerm},
                 {"MinimumTermUOM", newQuote.MinimumTermUOM.ToLower()},
                 {"CoordinatorID",newQuote.CoordinatorID},
                  {"SalesManager",newQuote.SalesManager},
                 {"AccountManagerID",newQuote.AccountManagerID},
                  {"QuoteDetailType",newQuote.QuoteDetailType},
                  {"CurrencyRegion",newQuote.CurrencyRegion},
                  {"CurrencyType",newQuote.CurrencyType},
                   {"RPORate",newQuote.RPORate},
                  {"RPOOptionTerms",newQuote.RPOOptionTerms}

                 };
                if(newQuote.QuoteDetailType == "RPO")
                {
                    sqlParams["RentalOrSales"] = "Rental";
                    sqlParams.Add("Jobsite", newQuote.FOB);
                    sqlParams["FOB"] = "";
                }

                QuoteID = db.CreateSalesQuote(sqlParams);
            }

          
            //after create quote successfully, we will create quotedetails depend on what type: sale or rental quote.They take different params
            if (QuoteID != 0)
            {
                newQuote.quoteID = QuoteID;
                newQuote.created = DateTime.Now;

                if (newQuote.QuoteType == "Sales")
                {
                    foreach (QuoteDetail qd in newQuote.quoteDetails)
                    {
                        string productCategory = qd.QuoteDetailMoreFields.Category != null ? qd.QuoteDetailMoreFields.Category : qd.QuoteDetailMoreFields.AttachmentType;
                            JObject sqlParamsForQuoteDetail = new JObject{
                            {"MonthlyRate", qd.MonthlyRate },
                            {"WeeklyRate", qd.WeeklyRate},

                           {"QuoteID", newQuote.quoteID},
                           {"SerialNumber", qd.SerialNumber},
                           {"Quantity", qd.Quantity},
                           {"Model", qd.Model},
                           {"Category", productCategory},
                           {"ManufacturerName", qd.ManufacturerName},
                           {"YearManufactured", qd.QuoteDetailMoreFields.YearFrom},
                           {"Price", qd.UnitPrice},
                           {"Freight", newQuote.Freight},
                           {"Description", qd.Description}
                      
                         };
                       
                          var quotedetailID =  db.CreateQuoteDetail(sqlParamsForQuoteDetail);
                          if (quotedetailID != 0)
                          {
                              JObject sqlParamsForQuoteDetailMoreFields = new JObject
                             {
                       
                               {"QuoteDetailID",quotedetailID},
                                
                               {"Category", qd.QuoteDetailMoreFields.Category},
                               {"Make", qd.QuoteDetailMoreFields.Make },
                               {"Model", qd.QuoteDetailMoreFields.Model},
                               {"YearFrom", qd.QuoteDetailMoreFields.YearFrom},
                               {"YearTo", qd.QuoteDetailMoreFields.YearTo},
                               {"FrontAttachment", qd.QuoteDetailMoreFields.FrontAttachment},
                               {"RearAttachment", qd.QuoteDetailMoreFields.RearAttachment},
                               {"TertiaryAttachment", qd.QuoteDetailMoreFields.TertiaryAttachment},
                               {"FrontType", qd.QuoteDetailMoreFields.FrontType},
                               {"RearType", qd.QuoteDetailMoreFields.RearType},
                               {"TertiaryType", qd.QuoteDetailMoreFields.TertiaryType},
                               {"TertiaryRate", qd.QuoteDetailMoreFields.TertiaryRate},
                               {"Notes", qd.QuoteDetailMoreFields.Notes},
                               {"EnterUserStr", "WWM\\"+ newQuote.EnterUserStr},
                               {"EnterDateTime", DateTime.Now.ToLocalTime()},
                               {"EditDateTime", DateTime.Now.ToLocalTime()},
                               {"EditUserStr","WWM\\"+ newQuote.EnterUserStr },
                               {"FitsOnCategory", qd.QuoteDetailMoreFields.FitsOnCategory},
                               {"FitsOnMake", qd.QuoteDetailMoreFields.FitsOnMake},
                               {"FitsOnModel", qd.QuoteDetailMoreFields.FitsOnModel},
                               {"AttachmentCategory",qd.QuoteDetailMoreFields.AttachmentCategory },
                               {"AttachmentType",qd.QuoteDetailMoreFields.AttachmentType },
                               {"MaximumHours",qd.QuoteDetailMoreFields.MaximumHours},
                               {"MaximumHoursUOM",qd.QuoteDetailMoreFields.MaximumHoursUOM},
                               {"OverageHours",qd.QuoteDetailMoreFields.OverageHours},
                               {"OptionPrice",qd.QuoteDetailMoreFields.OptionPrice},
                               {"FrontModel",qd.QuoteDetailMoreFields.FrontModel},
                               {"RearModel",qd.QuoteDetailMoreFields.RearModel},
                               {"TertiaryModel",qd.QuoteDetailMoreFields.TertiaryModel},
                               {"AttachmentModel",qd.QuoteDetailMoreFields.AttachmentModel},
                               {"InventoryMasterID",qd.QuoteDetailMoreFields.InventoryMasterID},
                               {"Specs",qd.QuoteDetailMoreFields.Specs},
                               {"Files",qd.QuoteDetailMoreFields.Files},
                                  
                             };
                              var quoteDetailMoreFieldsStatus = db.CreateQuoteDetailExtension(sqlParamsForQuoteDetailMoreFields);
                          }

                    }
                }
                else if (newQuote.QuoteType == "Rental")
                {
                    foreach (QuoteDetail qd in newQuote.quoteDetails)
                    {
                        string productCategory = qd.QuoteDetailMoreFields.Category != null?qd.QuoteDetailMoreFields.Category: qd.QuoteDetailMoreFields.AttachmentType;
                        string productCatelog = qd.QuoteDetailMoreFields.Category != null ? "" : qd.QuoteDetailMoreFields.AttachmentCategory;
                      

                         JObject sqlParamsForQuoteDetail = new JObject
                         {
                               {"QuoteID", newQuote.quoteID},
                               {"SerialNumber", qd.SerialNumber},
                               {"Quantity", qd.Quantity},
                               {"MonthlyRate", qd.MonthlyRate },
                               {"WeeklyRate", qd.WeeklyRate},
                               {"FootRate", qd.FootRate},
                               {"MinFeet", qd.MinFeet},
                               {"Model", qd.Model},
                               {"OvertimeHourlyRate", qd.OvertimeHourlyRate},
                               {"Description", qd.Description},     
                               {"ManufacturerName", qd.ManufacturerName},
                               {"Freight", newQuote.Freight},
                               {"ProductCategory",productCategory},
                               {"ProductCatelog",productCatelog}
                         };
                            var quotedetailID = db.CreateQuoteDetail(sqlParamsForQuoteDetail);

                            if (quotedetailID!=0)
                        {
                            JObject sqlParamsForQuoteDetailMoreFields = new JObject
                             {
                       
                               {"QuoteDetailID",quotedetailID},
                                
                               {"Category", qd.QuoteDetailMoreFields.Category},
                               {"Make", qd.QuoteDetailMoreFields.Make },
                               {"Model", qd.QuoteDetailMoreFields.Model},
                               {"YearFrom", qd.QuoteDetailMoreFields.YearFrom},
                               {"YearTo", qd.QuoteDetailMoreFields.YearTo},
                               {"FrontAttachment", qd.QuoteDetailMoreFields.FrontAttachment},
                               {"RearAttachment", qd.QuoteDetailMoreFields.RearAttachment},
                               {"TertiaryAttachment", qd.QuoteDetailMoreFields.TertiaryAttachment},
                               {"FrontType", qd.QuoteDetailMoreFields.FrontType},
                               {"RearType", qd.QuoteDetailMoreFields.RearType},
                               {"TertiaryType", qd.QuoteDetailMoreFields.TertiaryType},
                               {"TertiaryRate", qd.QuoteDetailMoreFields.TertiaryRate},
                               {"Notes", qd.QuoteDetailMoreFields.Notes},
                               {"EnterUserStr", "WWM\\"+ newQuote.EnterUserStr},
                               {"EnterDateTime", DateTime.Now.ToLocalTime()},
                               {"EditDateTime", DateTime.Now.ToLocalTime()},
                               {"EditUserStr","WWM\\"+ newQuote.EnterUserStr },
                               {"FitsOnCategory", qd.QuoteDetailMoreFields.FitsOnCategory},
                               {"FitsOnMake", qd.QuoteDetailMoreFields.FitsOnMake},
                               {"FitsOnModel", qd.QuoteDetailMoreFields.FitsOnModel},
                               {"AttachmentCategory",qd.QuoteDetailMoreFields.AttachmentCategory },
                               {"AttachmentType",qd.QuoteDetailMoreFields.AttachmentType },
                               {"MaximumHours",qd.QuoteDetailMoreFields.MaximumHours},
                               {"MaximumHoursUOM",qd.QuoteDetailMoreFields.MaximumHoursUOM},
                               {"OverageHours",qd.QuoteDetailMoreFields.OverageHours},
                               {"OptionPrice",qd.QuoteDetailMoreFields.OptionPrice},
                               {"FrontModel",qd.QuoteDetailMoreFields.FrontModel},
                               {"RearModel",qd.QuoteDetailMoreFields.RearModel},
                               {"TertiaryModel",qd.QuoteDetailMoreFields.TertiaryModel},
                               {"AttachmentModel",qd.QuoteDetailMoreFields.AttachmentModel},
                                  
                             };
                            var quoteDetailMoreFieldsStatus = db.CreateQuoteDetailExtension(sqlParamsForQuoteDetailMoreFields);
                        }
                     }

                }

                //create customer portal account after quote creation

                //var json = new JObject { { "ContactID", newQuote.ContactRelationshipID } }; //contactrelationshi is childid
                //var contact = DAL.GetInstance().getContactByContactID(json);  //get email from customer

                //var tokens = new JObject();
                //tokens.Add("Email", contact.Email);
                //if (db.CheckifCustomerPortalAccountExist(tokens) == 1)
                //{

                //    JObject sqlParamsForCreateAccount= new JObject
                //             {
                //               {"CustomerID",newQuote.ContactRelationshipID},
                //               {"EnterUserStr",newQuote.EnterUserStr },

                //             };
                //    var customerResult = db.CreateNewCustomerPortalAccount(sqlParamsForCreateAccount);

                //    if (customerResult)
                //    {
                //        //send passwordreset email
                //        try { db.SendPasswordResetEmail(sqlParamsForCreateAccount["TimeStampLink"].ToString(), contact.Email); }
                //        catch (Exception e) { }
                //    }
                //}


                // update hubspot deal after quote created
                if (newQuote.QuoteType == "Rental")
                {
                    DealUpdateRequestDTO quoteDeal = new DealUpdateRequestDTO();
                    decimal amount = 0;
                string equipmentDescription = "";
                    if (newQuote.QuoteType == "Sales")
                    {
                        foreach (QuoteDetail qd in newQuote.quoteDetails)
                        {
                            amount += qd.UnitPrice;
                        }
                    }
                    else if (newQuote.QuoteType == "Rental")
                    {
                        foreach (QuoteDetail qd in newQuote.quoteDetails)
                        {
                            if (newQuote.MinimumTermUOM.ToLower() == "four_weeks" || newQuote.MinimumTermUOM.ToLower() == "month")
                                amount += qd.MonthlyRate * qd.Quantity;
                            else if (newQuote.MinimumTermUOM.ToLower() == "week")
                                amount += qd.WeeklyRate * qd.Quantity;
                            else if (newQuote.MinimumTermUOM.ToLower() == "day")
                                amount += qd.WeeklyRate/3 * qd.Quantity;
                        if (equipmentDescription != "")
                            equipmentDescription += " & ";
                        equipmentDescription += qd.Quantity + " " + qd.Description;
                          
                        }
                        amount *= (int)newQuote.MinimumTerm;
                    }
                
                    quoteDeal.DealId = newQuote.DealID;
                    quoteDeal.Amount = amount;
                    quoteDeal.DealEquipmentDescription = equipmentDescription;
                    quoteDeal.QuoteType = newQuote.QuoteType;
                    var sqlParams = new JObject { { "QuoteID", newQuote.quoteID } };
                    var quotenumber = db.GetQuoteNumberByQuoteID(sqlParams);
                    quoteDeal.QuoteNumber = quotenumber.ToString();
                    string Env = ConfigurationManager.ConnectionStrings["mach1"].ConnectionString.Contains("L3SQ") ? "Prod" : "Test";
                    JObject para = new JObject { { "Enviroment", Env } };
                    List<DealStages> dealstages = db.GetAllDealStageValue(para);
                    string quoted = dealstages.Find(s => s.Name.Trim() == "Quoted").value;          // quoted
                    quoteDeal.DealStage = quoted;
                    var date = new DateTime(1970, 1, 1, 0, 0, 0, ((DateTime)newQuote.startDate).Kind);
                    quoteDeal.Closedate = System.Convert.ToInt64((((DateTime)newQuote.startDate) - date).Ticks / TimeSpan.TicksPerMillisecond);
                    quoteDeal.QuoteID = newQuote.quoteID;
                    quoteDeal.MaxExpectedAmount = amount;

                    DealsHelper dealHelper = new DealsHelper();
                    dealHelper.UpdateDeal(quoteDeal);

                }
                return newQuote;
            }


            

            else return null;
        }

        
    }
}
