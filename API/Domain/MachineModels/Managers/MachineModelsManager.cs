using API.Data;
using API.Models;
using API.Utilities;
using API.Utilities.Auth;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace API.Managers
{
    public static class MachineModelsManager
    {
        public static bool Create(InventoryMaster models)
        {
            var db = DAL.GetInstance();


            int InventoryMasterID = 0;

            try
            {
                db.BeginSqlTranscation();

                JObject sqlParamsForRentalReservationGroup = new JObject{
                            {"RentalReservationGroup", models.RentalReservationGroup},
                            {"EnterUserStr", "WWM\\"+ models.EnterUserStr}
                            };
                int? rentalReservationGroupID = db.CreateRentalreservationgroupifnotexist(sqlParamsForRentalReservationGroup);

                JObject sqlParamsForModels = new JObject{
                            {"ProductCategoryName", models.Category},
                            {"ManufacturerName",models.Make},
                            {"ModelNum", models.Model},
                            {"ModelNumStripped",RemoveSpecialCharacters(models.Model)  },
                            {"Weight", models.Weight},
                            {"WeightUnits", models.WeightUnits},
                            {"Height", models.Height},
                            {"HeightUnits", models.HeightUnits},
                            {"Length", models.Length},
                            {"LengthUnits", models.LengthUnits},
                            {"Width", models.Width},
                            {"WidthUnits", models.WidthUnits},
                            {"RentalReservationGroupID", rentalReservationGroupID},
                            {"EnterUserStr", "WWM\\"+ models.EnterUserStr}
                            };
                
                InventoryMasterID = db.CreateNewInventoryMaster(sqlParamsForModels);
                if (InventoryMasterID != 0)
                {
                    JObject sqlParamsForModelsExtension = new JObject{
                            {"InventoryMasterID", InventoryMasterID},
                            {"MonthlyRentalRate", models.MonthlyRentalRate },
                            {"FootRate", models.FootRate },
                            {"FootRateCAN", models.FootRateCAN },
                            {"FootRateLA", models.FootRateLA },
                            {"FootRateCE", models.FootRateCE },
                            {"FootRateAUD", models.FootRateAUD },
                            {"FootRateINT", models.FootRateINT },
                            {"ListRateCAN", models.ListRateCAN },
                            {"ListRateLA", models.ListRateLA },
                            {"ListRateCE", models.ListRateCE },
                            {"ListRateAUD", models.ListRateAUD },
                            {"ListRateINT", models.ListRateINT },
                            {"FrontAttachmentCategory", models.FrontAttachmentCategory },
                            {"FrontAttachmentType", models.FrontAttachmentType },
                            {"FrontAttachmentMake", models.FrontAttachmentMake },
                            {"FrontAttachmentModel", models.FrontAttachmentModel },
                            {"RearAttachmentCategory", models.RearAttachmentCategory },
                            {"RearAttachmentType", models.RearAttachmentType },
                            {"RearAttachmentMake", models.RearAttachmentMake },
                            {"RearAttachmentModel", models.RearAttachmentModel },
                            {"TertiaryAttachmentCategory", models.TertiaryAttachmentCategory },
                            {"TertiaryAttachmentType", models.TertiaryAttachmentType },
                            {"TertiaryAttachmentMake", models.TertiaryAttachmentMake },
                            {"TertiaryAttachmentModel", models.TertiaryAttachmentModel },
                            {"FullDescription", models.FullDescription },
                            {"MarketingDescription", models.MarketingDescription },
                            {"PreventiveMaintenance250Cost", models.PreventiveMaintenance250Cost },
                            {"PreventiveMaintenance250Price", models.PreventiveMaintenance250Price },
                            {"PreventiveMaintenance500Cost", models.PreventiveMaintenance500Cost },
                            {"PreventiveMaintenance500Price", models.PreventiveMaintenance500Price },
                            {"PreventiveMaintenance750Cost", models.PreventiveMaintenance750Cost }, 
                            {"PreventiveMaintenance750Price", models.PreventiveMaintenance750Price },
                            {"PreventiveMaintenance1000Cost", models.PreventiveMaintenance1000Cost },
                            {"PreventiveMaintenance1000Price", models.PreventiveMaintenance1000Price },
                            {"PreventiveMaintenanceCost", models.PreventiveMaintenanceCost },
                            {"PreventiveMaintenancePrice", models.PreventiveMaintenancePrice },
                            {"DamageInsPerHourCost", models.DamageInsPerHourCost },
                            {"DamageInsPerHourPrice", models.DamageInsPerHourPrice }, 
                            {"DamageInsPerMonthCost", models.DamageInsPerMonthCost },
                            {"DamageInsPerMonthPrice", models.DamageInsPerMonthPrice },
                            {"ShippingPerMile25Cost", models.ShippingPerMile25Cost },
                            {"ShippingPerMile25Price", models.ShippingPerMile25Price },
                            {"ShippingPerMile50Cost", models.ShippingPerMile50Cost },
                            {"ShippingPerMile50Price", models.ShippingPerMile50Price },
                            {"ShippingPerMile50AboveCost", models.ShippingPerMile50AboveCost },
                            {"ShippingPerMile50AbovePrice", models.ShippingPerMile50AbovePrice },
                            {"ShippingPerHour4MinimumCost", models.ShippingPerHour4MinimumCost },
                            {"ShippingPerHour4MinimumPrice", models.ShippingPerHour4MinimumPrice },
                            {"ShippingPerHour4AboveCost", models.ShippingPerHour4AboveCost },
                            {"ShippingPerHour4AbovePrice", models.ShippingPerHour4AbovePrice },
                            {"ShippingFlatRateCost", models.ShippingFlatRateCost },
                            {"ShippingFlatRatePrice", models.ShippingFlatRatePrice },
                            {"LauriniCostUSD" ,models.LauriniCostUSD},
                            {"LauriniCostEuro" ,models.LauriniCostEuro},
                            {"LauriniManufacturingTime", models.LauriniManufacturingTime},
                            {"LauriniManufacturingTimeUnit",models.LauriniManufacturingTimeUnit},
                            {"LauriniPriceUSD" ,models.LauriniPriceUSD},
                            {"LauriniPriceCanada" ,models.LauriniPriceCanada},
                            {"LauriniPriceEurope" ,models.LauriniPriceEurope},
                            {"LauriniPriceAustralia" ,models.LauriniPriceAustralia},
                            {"LauriniPriceLatinAmerica",models.LauriniPriceLatinAmerica},
                            {"LauriniPriceOther" ,models.LauriniPriceOther}
                                     };
                    db.CreateNewModelExtension(sqlParamsForModelsExtension);


                    foreach (ModelSpec spec in models.ModelSpecs)
                    {
                        JObject sqlParamsForModelsSpec = new JObject{
                            {"InventoryMasterID", InventoryMasterID},
                            {"SpecLabel", spec.SpecLabel},
                            {"SpecValue", spec.SpecValue},
                            {"SpecUnitType", spec.SpecUnitType},
                            {"EnterUserStr", "WWM\\"+ models.EnterUserStr}
                            };
                        db.CreateNewModelSpec(sqlParamsForModelsSpec);
                    }

                    foreach (ModelFile files in models.Files)
                    {
                        JObject sqlParamsForModelsFile = new JObject{
                            {"InventoryMasterID", InventoryMasterID},
                            {"FileName", files.FileName},
                            {"FileType", files.FileType},
                            {"FileUrl", files.FileUrl},
                            {"EnterUserStr", "WWM\\"+ models.EnterUserStr}
                            };
                        db.CreateNewModelFile(sqlParamsForModelsFile);
                    }

                    foreach (ModelPhoto photo in models.Photos)
                    {
                        JObject sqlParamsForModelsPhoto = new JObject{
                            {"InventoryMasterID", InventoryMasterID},
                            {"IsPrimary", photo.IsPrimary},
                            {"PhotoName", photo.PhotoName},
                            {"PhotoType", photo.PhotoType},
                            {"PhotoUrl", photo.PhotoUrl},
                            {"EnterUserStr", "WWM\\"+ models.EnterUserStr}
                            };
                        db.CreateNewModelPhoto(sqlParamsForModelsPhoto);
                    }

                    foreach (ModelCompetitor competitor in models.Competitors)
                    {
                        JObject sqlParamsForModelsCompetitor = new JObject{
                            {"InventoryMasterID", InventoryMasterID},
                            {"CompetitorName", competitor.CompetitorName},
                            {"CompetitorUSD", competitor.CompetitorUSD},
                            {"CompetitorEuro", competitor.CompetitorEuro},
                            {"EnterUserStr", "WWM\\"+ models.EnterUserStr}
                            };
                        db.CreateNewModelCompetitor(sqlParamsForModelsCompetitor);
                    }

                }

                db.CommitSqlTranscation();
                return true;
            }
            catch (Exception e)
            {
                db.RollBackSqlTranscation();
                return false;
            }

        }

        public static string RemoveSpecialCharacters(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static bool Update(InventoryMaster models)
        {
             var db = DAL.GetInstance();
             try
             {
                 if (models.InventoryMasterID != 0)
                 {
                     db.BeginSqlTranscation();
                     JObject sqlParamsForRentalReservationGroup = new JObject{
                            {"RentalReservationGroup", models.RentalReservationGroup},
                            {"EnterUserStr", "WWM\\"+ models.EnterUserStr}
                            };
                     int? rentalReservationGroupID = db.CreateRentalreservationgroupifnotexist(sqlParamsForRentalReservationGroup);
                     JObject sqlParamsForModels = new JObject{
                            {"InventoryMasterID", models.InventoryMasterID},
                            {"ProductCategoryName", models.Category},
                            {"ManufacturerName",models.Make},
                            {"ModelNum", models.Model},
                            {"ModelNumStripped",RemoveSpecialCharacters(models.Model)  },
                            {"Weight", models.Weight},
                            {"WeightUnits", models.WeightUnits},
                            {"Height", models.Height},
                            {"HeightUnits", models.HeightUnits},
                            {"Length", models.Length},
                            {"LengthUnits", models.LengthUnits},
                            {"Width", models.Width},
                            {"WidthUnits", models.WidthUnits},
                            {"RentalReservationGroupID", rentalReservationGroupID},
                            {"EditUserStr", "WWM\\"+ models.EnterUserStr}
                            };
                    
                     db.UpdateInventoryMaster(sqlParamsForModels);
                     #region models
                     JObject sqlParamsForModelsExtension = new JObject{
                            {"InventoryMasterID", models.InventoryMasterID},
                            {"MonthlyRentalRate", models.MonthlyRentalRate },
                            {"FootRate", models.FootRate },
                            {"FootRateCAN", models.FootRateCAN },
                            {"FootRateLA", models.FootRateLA },
                            {"FootRateCE", models.FootRateCE },
                            {"FootRateAUD", models.FootRateAUD },
                            {"FootRateINT", models.FootRateINT },
                            {"ListRateCAN", models.ListRateCAN },
                            {"ListRateLA", models.ListRateLA },
                            {"ListRateCE", models.ListRateCE },
                            {"ListRateAUD", models.ListRateAUD },
                            {"ListRateINT", models.ListRateINT },
                            {"FrontAttachmentCategory", models.FrontAttachmentCategory },
                            {"FrontAttachmentType", models.FrontAttachmentType },
                            {"FrontAttachmentMake", models.FrontAttachmentMake },
                            {"FrontAttachmentModel", models.FrontAttachmentModel },
                            {"RearAttachmentCategory", models.RearAttachmentCategory },
                            {"RearAttachmentType", models.RearAttachmentType },
                            {"RearAttachmentMake", models.RearAttachmentMake },
                            {"RearAttachmentModel", models.RearAttachmentModel },
                            {"TertiaryAttachmentCategory", models.TertiaryAttachmentCategory },
                            {"TertiaryAttachmentType", models.TertiaryAttachmentType },
                            {"TertiaryAttachmentMake", models.TertiaryAttachmentMake },
                            {"TertiaryAttachmentModel", models.TertiaryAttachmentModel },
                            {"FullDescription", models.FullDescription },
                            {"MarketingDescription", models.MarketingDescription },
                            {"PreventiveMaintenance250Cost", models.PreventiveMaintenance250Cost },
                            {"PreventiveMaintenance250Price", models.PreventiveMaintenance250Price },
                            {"PreventiveMaintenance500Cost", models.PreventiveMaintenance500Cost },
                            {"PreventiveMaintenance500Price", models.PreventiveMaintenance500Price },
                            {"PreventiveMaintenance750Cost", models.PreventiveMaintenance750Cost }, 
                            {"PreventiveMaintenance750Price", models.PreventiveMaintenance750Price },
                            {"PreventiveMaintenance1000Cost", models.PreventiveMaintenance1000Cost },
                            {"PreventiveMaintenance1000Price", models.PreventiveMaintenance1000Price },
                            {"PreventiveMaintenanceCost", models.PreventiveMaintenanceCost },
                            {"PreventiveMaintenancePrice", models.PreventiveMaintenancePrice },
                            {"DamageInsPerHourCost", models.DamageInsPerHourCost },
                            {"DamageInsPerHourPrice", models.DamageInsPerHourPrice }, 
                            {"DamageInsPerMonthCost", models.DamageInsPerMonthCost },
                            {"DamageInsPerMonthPrice", models.DamageInsPerMonthPrice },
                            {"ShippingPerMile25Cost", models.ShippingPerMile25Cost },
                            {"ShippingPerMile25Price", models.ShippingPerMile25Price },
                            {"ShippingPerMile50Cost", models.ShippingPerMile50Cost },
                            {"ShippingPerMile50Price", models.ShippingPerMile50Price },
                            {"ShippingPerMile50AboveCost", models.ShippingPerMile50AboveCost },
                            {"ShippingPerMile50AbovePrice", models.ShippingPerMile50AbovePrice },
                            {"ShippingPerHour4MinimumCost", models.ShippingPerHour4MinimumCost },
                            {"ShippingPerHour4MinimumPrice", models.ShippingPerHour4MinimumPrice },
                            {"ShippingPerHour4AboveCost", models.ShippingPerHour4AboveCost },
                            {"ShippingPerHour4AbovePrice", models.ShippingPerHour4AbovePrice },
                            {"ShippingFlatRateCost", models.ShippingFlatRateCost },
                            {"ShippingFlatRatePrice", models.ShippingFlatRatePrice },
                            {"LauriniCostUSD" ,models.LauriniCostUSD},
                            {"LauriniCostEuro" ,models.LauriniCostEuro},
                            {"LauriniManufacturingTime", models.LauriniManufacturingTime},
                            {"LauriniManufacturingTimeUnit",models.LauriniManufacturingTimeUnit},
                            {"LauriniPriceUSD" ,models.LauriniPriceUSD},
                            {"LauriniPriceCanada" ,models.LauriniPriceCanada},
                            {"LauriniPriceEurope" ,models.LauriniPriceEurope},
                            {"LauriniPriceAustralia" ,models.LauriniPriceAustralia},
                            {"LauriniPriceLatinAmerica",models.LauriniPriceLatinAmerica},
                            {"LauriniPriceOther" ,models.LauriniPriceOther}
                                     };
                     db.UpdateModelExtension(sqlParamsForModelsExtension);

                     #endregion
                     #region spec
                     foreach (ModelSpec spec in models.ModelSpecs)
                     {
                         if (spec.ModelsSpecID == 0)  //existing models spec insert.
                         {
                             JObject sqlParamsForModelsSpec = new JObject{
                            {"InventoryMasterID", models.InventoryMasterID},
                            {"SpecLabel", spec.SpecLabel},
                            {"SpecValue", spec.SpecValue},
                            {"SpecUnitType", spec.SpecUnitType},
                            {"EnterUserStr", "WWM\\"+ models.EnterUserStr}
                            };
                             db.CreateNewModelSpec(sqlParamsForModelsSpec);
                         }
                         else if( !spec.Active )
                         {
                             JObject sqlParamsForspec = new JObject{
                             {"ModelsSpecID", spec.ModelsSpecID},
                             {"EditUserStr", "WWM\\"+ models.EnterUserStr},
                            };
                             db.DeactivateModelsSpec(sqlParamsForspec);
                         }
                         else
                         {
                             JObject sqlParamsForModelsSpec = new JObject{
                             {"ModelsSpecID", spec.ModelsSpecID},
                            {"SpecLabel", spec.SpecLabel},
                            {"SpecValue", spec.SpecValue},
                            {"SpecUnitType", spec.SpecUnitType},
                            {"EditUserStr", "WWM\\"+ models.EnterUserStr}
                            };
                             db.UpdateModelsSpec(sqlParamsForModelsSpec);
                         }

                     }
                     #endregion
                     #region photo
                     foreach (ModelPhoto photo in models.Photos)
                     {
                         if (photo.ModelsPhotosID == 0)  //existing models photo insert.
                         {
                             JObject sqlParamsForModelsPhoto = new JObject{
                            {"InventoryMasterID", models.InventoryMasterID},
                            {"IsPrimary", photo.IsPrimary},
                            {"PhotoName", photo.PhotoName},
                            {"PhotoType", photo.PhotoType},
                            {"PhotoUrl", photo.PhotoUrl},
                            {"EnterUserStr", "WWM\\"+ models.EnterUserStr}
                            };
                             db.CreateNewModelPhoto(sqlParamsForModelsPhoto);
                         }
                         else if (!photo.Active)       //deativate models photo
                         {
                             JObject sqlParamsForPhoto = new JObject{
                             {"ModelsPhotosID", photo.ModelsPhotosID},
                             {"EditUserStr", "WWM\\"+ models.EnterUserStr},
                            };
                             db.DeactivateModelsPhoto(sqlParamsForPhoto);
                         }
                         else                      //update model photo
                         {
                             JObject sqlParamsForModelsPhoto = new JObject{
                            {"ModelsPhotosID", photo.ModelsPhotosID},
                            {"IsPrimary", photo.IsPrimary},
                            {"EditUserStr", "WWM\\"+ models.EnterUserStr}
                            };
                             db.UpdateModelsPhoto(sqlParamsForModelsPhoto);
                         }

                     }
                     #endregion
                     #region files
                     foreach (ModelFile files in models.Files)
                     {
                         if (files.ModelsFilesID == 0)  //existing models file insert.
                         {
                             JObject sqlParamsForModelsFile = new JObject{
                            {"InventoryMasterID", models.InventoryMasterID},
                            {"FileName", files.FileName},
                            {"FileType", files.FileType},
                            {"FileUrl", files.FileUrl},
                            {"EnterUserStr", "WWM\\"+ models.EnterUserStr}
                            };
                             db.CreateNewModelFile(sqlParamsForModelsFile);
                         }
                         else if (!files.Active)       //deativate models file
                         {
                             JObject sqlParamsForFile = new JObject{
                             {"ModelsFilesID", files.ModelsFilesID},
                             {"EditUserStr", "WWM\\"+ models.EnterUserStr},
                            };
                             db.DeactivateModelsFile(sqlParamsForFile);
                         }

                     }
                     #endregion
                     #region competitor
                     foreach (ModelCompetitor competitor in models.Competitors)
                     {
                         if (competitor.ModelsCompetitorsID == 0)  //existing models photo insert.
                         {
                             JObject sqlParamsForModelsCompetitor = new JObject{
                            {"InventoryMasterID", models.InventoryMasterID},
                            {"CompetitorName", competitor.CompetitorName},
                            {"CompetitorUSD", competitor.CompetitorUSD},
                            {"CompetitorEuro", competitor.CompetitorEuro},
                            {"EnterUserStr", "WWM\\"+ models.EnterUserStr}
                            };
                             db.CreateNewModelCompetitor(sqlParamsForModelsCompetitor);
                         }
                         else if (!competitor.Active)       //deativate models photo
                         {
                             JObject sqlParamsForPhoto = new JObject{
                             {"ModelsCompetitorsID", competitor.ModelsCompetitorsID},
                             {"EditUserStr", "WWM\\"+ models.EnterUserStr},
                            };
                             db.DeactivateModelsCompetitor(sqlParamsForPhoto);
                         }
                         else                      //update model photo
                         {
                             JObject sqlParamsForModelsPhoto = new JObject{
                            {"ModelsCompetitorsID", competitor.ModelsCompetitorsID},
                            {"CompetitorName", competitor.CompetitorName},
                            {"CompetitorUSD", competitor.CompetitorUSD},
                            {"CompetitorEuro", competitor.CompetitorEuro},
                            {"EditUserStr", "WWM\\"+ models.EnterUserStr}
                            };
                             db.UpdateModelsCompetitor(sqlParamsForModelsPhoto);
                         }

                     }
                     #endregion


                 }
                 

                 db.CommitSqlTranscation();
                 return true;
             }
             catch (Exception e)
             {
                 db.RollBackSqlTranscation();
                 return false;
             }
        }
    }


    public static class CategoryMachineManager
    {
        public static bool Create(ProductCategory cate)
        {
            var db = DAL.GetInstance();
            int ProductCategoryID = 0;

            try
            {
                db.BeginSqlTranscation();

                JObject sqlParams = new JObject{
                            {"CategoryName", cate.ProductCategoryName},
                            {"EnterUserStr", "WWM\\"+ cate.EnterUserStr}
                            };

                ProductCategoryID = db.CreateCategoryInTrans(sqlParams);
                if (ProductCategoryID != 0)
                 {
                     foreach (CategoryImages photo in cate.Images)
                     {
                         JObject sqlParamsForModelsPhoto = new JObject{
                            {"ProductCategoryID", ProductCategoryID},
                            {"IsPrimary", photo.IsPrimary},
                            {"ImageName", photo.ImageName},
                            {"ImageType", photo.ImageType},
                            {"ImageUrl", photo.ImageUrl},
                            {"EnterUserStr", "WWM\\"+ cate.EnterUserStr}
                            };
                         db.CreateNewCategoryImage(sqlParamsForModelsPhoto);
                     }

                     JObject sqlParamsForDetail = new JObject{
                            {"ProductCategoryID", ProductCategoryID},
                            {"FullDescription", cate.FullDescription},
                            {"MarketingDescription", cate.MarketingDescription},
                            {"EnterUserStr", "WWM\\"+ cate.EnterUserStr}
                            };
                     db.CreateProductCategoryDetail(sqlParamsForDetail);

                 }

                db.CommitSqlTranscation();
                return true;
            }
            catch (Exception e)
            {
                db.RollBackSqlTranscation();
                return false;
            }
        }

        public static bool Update(ProductCategory cate)
        {
            var db = DAL.GetInstance();
            try
            {
                if (cate.ProductCategoryID != 0)
                {

                    db.BeginSqlTranscation();
                    JObject sqlParamsForDetail = new JObject{
                            {"ProductCategoryID", cate.ProductCategoryID},
                            {"FullDescription", cate.FullDescription},
                            {"MarketingDescription", cate.MarketingDescription},
                            {"EnterUserStr", "WWM\\"+ cate.EnterUserStr}
                            };
                    db.UpdateProductCategoryDetail(sqlParamsForDetail);

                    foreach (CategoryImages photo in cate.Images)
                    {
                        if (photo.CategoryImagesID == 0)  //existing models photo insert.
                        {
                            JObject sqlParamsForModelsPhoto = new JObject{
                                   {"ProductCategoryID", cate.ProductCategoryID},
                                    {"IsPrimary", photo.IsPrimary},
                                    {"ImageName", photo.ImageName},
                                    {"ImageType", photo.ImageType},
                                    {"ImageUrl", photo.ImageUrl},
                                    {"EnterUserStr", "WWM\\"+ cate.EnterUserStr}
                                    };
                            db.CreateNewCategoryImage(sqlParamsForModelsPhoto);
                        }
                        else if (!photo.Active)       //deativate models photo
                        {
                            JObject sqlParamsForPhoto = new JObject{
                                     {"CategoryImagesID", photo.CategoryImagesID},
                                     {"EditUserStr", "WWM\\"+ cate.EnterUserStr},
                                    };
                            db.DeactivateCetegoryImage(sqlParamsForPhoto);
                        }
                        else                      //update model photo
                        {
                            JObject sqlParamsForModelsPhoto = new JObject{
                                    {"CategoryImagesID", photo.CategoryImagesID},
                                    {"IsPrimary", photo.IsPrimary},
                                    {"EditUserStr", "WWM\\"+ cate.EnterUserStr}
                                    };
                            db.UpdateCategoryImage(sqlParamsForModelsPhoto);
                        }

                    }
                }
                db.CommitSqlTranscation();
                return true;
            }
            catch (Exception e)
            {
                db.RollBackSqlTranscation();
                return false;

            }
        }

        public static ProductCategory View(JObject token)
        {
            var db = DAL.GetInstance();
            ProductCategory view = new ProductCategory();
            try
            {
                view = db.getProductCategoryViewByID(token);
                view.Images = db.getProductCategoryImage(token);
                return view;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}