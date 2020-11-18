using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models;
using Newtonsoft.Json.Linq;

namespace API.Data
{
    //this will provide all the necessary detail for the mobile front end to render
    public class QuoteDetails : IBuildParams<QuoteDetails>
    {
        private List<Equipment> Equipments = new List<Equipment>();
        public Address Address;
        public ContactAddress JobSiteAddress;
        public Quote Quote;
        public Contact Company;
        public Contact Person;
        public Contact AccountManager;
        public Contact Coordinator;
        public Contact SalesManager;
        public DivisionDetail DivisionDetail;
        private ContactRelationship ContactRelationship;
        public List<QuoteDetail> QuoteDetail = new List<QuoteDetail>();
        public ShipmentQuotes ShipmentQuotes = new ShipmentQuotes();
        private List<ShipmentQuotesInventory> ShipmentQuotesInventory = new List<ShipmentQuotesInventory>();
        private Equipment _equipment;
        public void Build(JObject sqlParams)
        {
            if (sqlParams["QuoteID"] != null)
            {
                Quote = DAL.GetInstance().getQuoteByQuoteID(sqlParams);
            }
            else if (sqlParams["HashID"] != null)
            {
                Quote = DAL.GetInstance().getQuoteByQuoteHashID(sqlParams);
            }
            else
                return;
            GetContactRelationship(Quote.ContactRelationshipID);
            GetAddress(ContactRelationship.ParentContactID);
            GetCompanyInfo(ContactRelationship.ParentContactID);
            GetPersonInfo(ContactRelationship.ChildContactID);
            GetAccountManagerInfo(Quote.AccountManagerID);
            GetSalesManagerInfo(Quote.SalesManager);
            GetCoordinatorInfo(Quote.CoordinatorID);
            GetPersonInfo(ContactRelationship.ChildContactID);
            GetDivisionDetailInfo(Quote.DivisionID);
            GetQuoteDetail(Quote.quoteID);
            GetShipmentQuotes(Quote.quoteID);
            if(ShipmentQuotes!= null)
                 GetShipmentQuotesInventoryByShipmentQuotesIDForCustomer(ShipmentQuotes.ShipmentQuotesID);
            if (Quote.JobSiteAddressID != 0)
                GetJobSiteAddress(Quote.JobSiteAddressID);
            //for quotedetail More field new table 
            foreach(QuoteDetail qd in QuoteDetail)
            {
                qd.QuoteDetailMoreFields= GetQuoteDetailMoreFields(qd.QuoteDetailID);
            }
            foreach(QuoteDetail qd in QuoteDetail)
            {
                if(qd.QuoteDetailMoreFields !=null || qd.ProductCategoryID !=1)
                {
                    if(qd.ProductCategoryID !=1)
                    {
                        JObject sqlParamsForProductCategory = new JObject { { "ProductCategoryID", qd.ProductCategoryID } };
                        CategoryType cate = DAL.GetInstance().getCategroyTypeByProductCategoryID(sqlParamsForProductCategory);
                        JObject sqlParamsForModelDetail = new JObject
                         {
                               {"Category", cate.Category},
                               {"Make", qd.ManufacturerName},
                               {"ModelNum", qd.Model },
                               {"ProductType", cate.ProductType},
                               {"Type", cate.Type},
                             
                         };
                        qd.ModelDetail = DAL.GetInstance().getModelFields(sqlParamsForModelDetail);
                    }
                    else if (qd.QuoteDetailMoreFields.Category != "" )
                    {
                        JObject sqlParamsForModelDetail = new JObject
                         {
                               {"Category", qd.QuoteDetailMoreFields.Category},
                               {"Make", qd.QuoteDetailMoreFields.Make},
                               {"ModelNum", qd.QuoteDetailMoreFields.Model },
                               {"ProductType", "Machine"},
                             
                         };
                        qd.ModelDetail = DAL.GetInstance().getModelFields(sqlParamsForModelDetail);

                    }
                    else
                    {
                        JObject sqlParamsForModelDetail = new JObject
                         {
                               {"Category", qd.QuoteDetailMoreFields.AttachmentCategory},
                              // {"Make", qd.QuoteDetailMoreFields.m},
                              // {"ModelNum", qd.QuoteDetailMoreFields.Model },
                               {"Type", qd.QuoteDetailMoreFields.AttachmentType},
                               {"ModelNum",qd.QuoteDetailMoreFields.AttachmentModel},
                               {"ProductType", "Attachment"},
                             
                         };
                        qd.ModelDetail = DAL.GetInstance().getModelFields(sqlParamsForModelDetail);
                    }

                    if (qd.QuoteDetailMoreFields !=null && qd.QuoteDetailMoreFields.InventoryMasterID != 0)       //get model spec file for sales quote superior manfacture
                    {
                        var tokens = new JObject { { "InventoryMasterID", qd.QuoteDetailMoreFields.InventoryMasterID } };
                        List<int> specs = qd.QuoteDetailMoreFields.Specs.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
                        if (specs.Count > 0)
                        {
                            qd.ModelDetail.ModelSpecs = DAL.GetInstance().getModelSpecs(tokens);
                            qd.ModelDetail.ModelSpecs = qd.ModelDetail.ModelSpecs.Where(s => specs.Any(id => id == s.ModelsSpecID));
                        }

                        List<int> files = qd.QuoteDetailMoreFields.Files.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
                        if (files.Count > 0)
                        {
                            qd.ModelDetail.ModelFiles = DAL.GetInstance().getModelFiles(tokens);
                            qd.ModelDetail.ModelFiles = qd.ModelDetail.ModelFiles.Where(s => files.Any(id => id == s.ModelsFilesID));
                        }

                        qd.ModelDetail.ModelPhotos = DAL.GetInstance().getModelPhotos(tokens);
                    }
                }

                
                
            }

            //for sales quote get equipment from equipmentid
            if (Quote.QuoteType == "Sales" || Quote.QuoteDetailType =="RPO")
            {
                foreach (QuoteDetail qd in QuoteDetail)
                {
                    if (qd.EquipmentID != 0)
                    {
                        GetEquipment(qd.EquipmentID);
                        if (_equipment != null)//qd.Equipment.Add(_equipment);
                        {
                            Equipments.Add(_equipment);
                        }
                            
                        if (Equipments != null)
                        {
                            BuildAttachment();
                            GetPictureFileNames();
                        }
                        qd.Equipment = _equipment;
                        Equipments.Clear();
                    }

                }
              
            }
            if(Quote.HashID !="")
            {
                string Env = System.Configuration.ConfigurationManager.ConnectionStrings["mach1"].ConnectionString;
                if (Env.Contains("galsql01") || Env.Contains("localhost"))
                {
                    Quote.HashID = "https://staging.customers.wwmach.com/quote/" + Quote.HashID;
                }
                else
                {
                    Quote.HashID = "https://customers.wwmach.com/quote/" + Quote.HashID;
                }
            }
            
        }

        public QuoteDetails GetResult()
        {
            return this;
        }

        private void GetContactRelationship(int ContactRelationshipID)
        {
            var json = new JObject { { "ContactRelationshipID", ContactRelationshipID } };
            ContactRelationship = DAL.GetInstance().getContactRelationshipByContactRelationshipID(json);
        }

        private void GetAddress(int contactID)
        {
            var json = new JObject { { "ContactID", contactID } };
            Address = DAL.GetInstance().getAddressByContactID(json).FirstOrDefault();
        }

        private void GetJobSiteAddress(int addressID)
        {
            var json = new JObject { { "AddressID", addressID } };
            JobSiteAddress = DAL.GetInstance().getAddressByAddressID(json);
        }

        private void GetCompanyInfo(int contactID)
        {
            var json = new JObject { { "ContactID", contactID } };
            Company = DAL.GetInstance().getContactByContactID(json);
        }

        private void GetPersonInfo(int contactID)
        {
            var json = new JObject { { "ContactID", contactID } };
            Person = DAL.GetInstance().getContactByContactID(json);
        }
        private void GetAccountManagerInfo(int contactID)
        {
            var json = new JObject { { "ContactID", contactID } };
            AccountManager = DAL.GetInstance().getContactByContactID(json);
        }
        private void GetSalesManagerInfo(int contactID)
        {
            var json = new JObject { { "ContactID", contactID } };
            SalesManager = DAL.GetInstance().getContactByContactID(json);
        }
        private void GetCoordinatorInfo(int contactID)
        {
            var json = new JObject { { "ContactID", contactID } };
            Coordinator = DAL.GetInstance().getContactByContactID(json);
        }
        private void GetDivisionDetailInfo(int DivisionID)
        {
            var json = new JObject { { "DivisionID", DivisionID } };
            DivisionDetail = DAL.GetInstance().getDivisionDetailByDivisionID(json);
        }

        private void GetQuoteDetail(int QuoteID)
        {
            var json = new JObject { { "QuoteID", QuoteID } };
            QuoteDetail = DAL.GetInstance().getQuoteDetailsByQuoteID(json);
        }
        private QuoteDetailMoreFields GetQuoteDetailMoreFields(int QuoteDetailID)
        {
            var json = new JObject { { "QuoteDetailID", QuoteDetailID } };
            return DAL.GetInstance().getQuoteDetailExtensionByQuoteDetailID(json);
        }


        private void GetEquipment(int EquipmentID)
        {
            var json = new JObject { { "EquipmentID", EquipmentID } };
            _equipment = DAL.GetInstance().getEquipmentByEquipmentID(json);
        }
        private void BuildAttachment()
        {
            DAL.GetInstance().getAttachments(Equipments);
        }

        private void GetPictureFileNames()
        {
            DAL.GetInstance().getPictureFileNames(Equipments);

        }
        private void GetShipmentQuotes(int QuoteID)
        {
            var json = new JObject { { "QuoteID", QuoteID } };
            ShipmentQuotes = DAL.GetInstance().getShipmentQuotesByQuoteIDForCustomer(json);
        }

        private void GetShipmentQuotesInventoryByShipmentQuotesIDForCustomer(int ShipmentQuotesID)
        {
            var json = new JObject { { "ShipmentQuotesID", ShipmentQuotesID } };
            ShipmentQuotesInventory = DAL.GetInstance().getShipmentQuotesInventoryByShipmentQuotesIDForCustomer(json);
            ShipmentQuotes.ShipmentQuotesInventories = ShipmentQuotesInventory;
        }

    }
}