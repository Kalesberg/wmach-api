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
    public static class RentalReservationGroupManager
    {
        public static bool Create(RentalReservationGroupData RentalReservationGroup)
        {
            var db = DAL.GetInstance();
            try
            {
                db.BeginSqlTranscation();
                JObject sqlParamsForRentalReservationGroup = new JObject{
                            {"RentalReservationGroup", RentalReservationGroup.RentalReservationGroup},
                            {"EnterUserStr", "WWM\\"+ RentalReservationGroup.EnterUserStr}
                            };
                int? rentalReservationGroupID = db.CreateRentalreservationgroupifnotexist(sqlParamsForRentalReservationGroup);
               
                if(rentalReservationGroupID == null || rentalReservationGroupID ==0)
                    throw new Exception();
                 
                JObject sqlParamsForRentalReservationGroupDetail = new JObject{
                            {"RentalReservationGroupID", rentalReservationGroupID},
                            {"MonthlyRentalRate", RentalReservationGroup.RentalReservationGroupDetail.MonthlyRentalRate },
                            {"FootRate", RentalReservationGroup.RentalReservationGroupDetail.FootRate },
                            {"FootRateCAN", RentalReservationGroup.RentalReservationGroupDetail.FootRateCAN },
                            {"FootRateLA", RentalReservationGroup.RentalReservationGroupDetail.FootRateLA },
                            {"FootRateCE", RentalReservationGroup.RentalReservationGroupDetail.FootRateCE },
                            {"FootRateAUD", RentalReservationGroup.RentalReservationGroupDetail.FootRateAUD },
                            {"FootRateINT", RentalReservationGroup.RentalReservationGroupDetail.FootRateINT },
                            {"ListRateCAN", RentalReservationGroup.RentalReservationGroupDetail.ListRateCAN },
                            {"ListRateLA", RentalReservationGroup.RentalReservationGroupDetail.ListRateLA },
                            {"ListRateCE", RentalReservationGroup.RentalReservationGroupDetail.ListRateCE },
                            {"ListRateAUD", RentalReservationGroup.RentalReservationGroupDetail.ListRateAUD },
                            {"ListRateINT", RentalReservationGroup.RentalReservationGroupDetail.ListRateINT },
                             {"EnterUserStr", "WWM\\"+ RentalReservationGroup.EnterUserStr},
                              {"FullDescription", RentalReservationGroup.RentalReservationGroupDetail.FullDescription },
                            {"MarketingDescription", RentalReservationGroup.RentalReservationGroupDetail.MarketingDescription },
                            {"PreventiveMaintenance250Cost", RentalReservationGroup.RentalReservationGroupDetail.PreventiveMaintenance250Cost },
                            {"PreventiveMaintenance250Price", RentalReservationGroup.RentalReservationGroupDetail.PreventiveMaintenance250Price },
                            {"PreventiveMaintenance500Cost", RentalReservationGroup.RentalReservationGroupDetail.PreventiveMaintenance500Cost },
                            {"PreventiveMaintenance500Price", RentalReservationGroup.RentalReservationGroupDetail.PreventiveMaintenance500Price },
                            {"PreventiveMaintenance750Cost", RentalReservationGroup.RentalReservationGroupDetail.PreventiveMaintenance750Cost }, 
                            {"PreventiveMaintenance750Price", RentalReservationGroup.RentalReservationGroupDetail.PreventiveMaintenance750Price },
                            {"PreventiveMaintenance1000Cost", RentalReservationGroup.RentalReservationGroupDetail.PreventiveMaintenance1000Cost },
                            {"PreventiveMaintenance1000Price", RentalReservationGroup.RentalReservationGroupDetail.PreventiveMaintenance1000Price },
                            {"PreventiveMaintenanceCost", RentalReservationGroup.RentalReservationGroupDetail.PreventiveMaintenanceCost },
                            {"PreventiveMaintenancePrice", RentalReservationGroup.RentalReservationGroupDetail.PreventiveMaintenancePrice },
                            {"DamageInsPerHourCost", RentalReservationGroup.RentalReservationGroupDetail.DamageInsPerHourCost },
                            {"DamageInsPerHourPrice", RentalReservationGroup.RentalReservationGroupDetail.DamageInsPerHourPrice }, 
                            {"DamageInsPerMonthCost", RentalReservationGroup.RentalReservationGroupDetail.DamageInsPerMonthCost },
                            {"DamageInsPerMonthPrice", RentalReservationGroup.RentalReservationGroupDetail.DamageInsPerMonthPrice },
                            {"ShippingPerMile25Cost", RentalReservationGroup.RentalReservationGroupDetail.ShippingPerMile25Cost },
                            {"ShippingPerMile25Price", RentalReservationGroup.RentalReservationGroupDetail.ShippingPerMile25Price },
                            {"ShippingPerMile50Cost", RentalReservationGroup.RentalReservationGroupDetail.ShippingPerMile50Cost },
                            {"ShippingPerMile50Price", RentalReservationGroup.RentalReservationGroupDetail.ShippingPerMile50Price },
                            {"ShippingPerMile50AboveCost", RentalReservationGroup.RentalReservationGroupDetail.ShippingPerMile50AboveCost },
                            {"ShippingPerMile50AbovePrice", RentalReservationGroup.RentalReservationGroupDetail.ShippingPerMile50AbovePrice },
                            {"ShippingPerHour4MinimumCost", RentalReservationGroup.RentalReservationGroupDetail.ShippingPerHour4MinimumCost },
                            {"ShippingPerHour4MinimumPrice", RentalReservationGroup.RentalReservationGroupDetail.ShippingPerHour4MinimumPrice },
                            {"ShippingPerHour4AboveCost", RentalReservationGroup.RentalReservationGroupDetail.ShippingPerHour4AboveCost },
                            {"ShippingPerHour4AbovePrice", RentalReservationGroup.RentalReservationGroupDetail.ShippingPerHour4AbovePrice },
                            {"ShippingFlatRateCost", RentalReservationGroup.RentalReservationGroupDetail.ShippingFlatRateCost },
                            {"ShippingFlatRatePrice", RentalReservationGroup.RentalReservationGroupDetail.ShippingFlatRatePrice }
                };
                int rentalReservationGroupDetailID = db.CreateRentalreservationgroupDetail(sqlParamsForRentalReservationGroupDetail);
                if (rentalReservationGroupDetailID == 0)
                    throw new Exception();

                if (RentalReservationGroup.RentalReservationGroupDetail.Files != null)
                {
                    foreach (RentalReservationFile files in RentalReservationGroup.RentalReservationGroupDetail.Files)
                    {
                        JObject sqlParamsForModelsFile = new JObject{
                            {"RentalReservationID", rentalReservationGroupID},
                            {"FileName", files.FileName},
                            {"FileType", files.FileType},
                            {"FileUrl", files.FileUrl},
                            {"EnterUserStr", "WWM\\"+ RentalReservationGroup.EnterUserStr}
                            };
                        db.CreateRentalReservationFile(sqlParamsForModelsFile);
                    }
                }
                if (RentalReservationGroup.RentalReservationGroupDetail.Photos!=null)
                {
                    foreach (RentalReservationPhoto photo in RentalReservationGroup.RentalReservationGroupDetail.Photos)
                    {
                        JObject sqlParamsForModelsPhoto = new JObject{
                                {"RentalReservationID", rentalReservationGroupID},
                                {"IsPrimary", photo.IsPrimary},
                                {"PhotoName", photo.PhotoName},
                                {"PhotoType", photo.PhotoType},
                                {"PhotoUrl", photo.PhotoUrl},
                                {"EnterUserStr", "WWM\\"+ RentalReservationGroup.EnterUserStr}
                                };
                        db.CreateRentalReservationPhoto(sqlParamsForModelsPhoto);
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

        public static bool Update(RentalReservationGroupData RentalReservationGroup)
        {
            var db = DAL.GetInstance();
            try
            {
                db.BeginSqlTranscation();
                JObject sqlParamsForRentalReservationGroup = new JObject{
                            {"RentalReservationGroup", RentalReservationGroup.RentalReservationGroup},
                             {"RentalReservationGroupID", RentalReservationGroup.RentalReservationGroupID},
                            {"EnterUserStr", "WWM\\"+ RentalReservationGroup.EnterUserStr}
                            };  
                bool rentalReservationGroupID = db.UpdateRentalreservationgroup(sqlParamsForRentalReservationGroup);

                JObject sqlParamsForRentalReservationGroupDetail = new JObject{
                            {"RentalReservationGroupID", RentalReservationGroup.RentalReservationGroupID},
                            {"MonthlyRentalRate", RentalReservationGroup.RentalReservationGroupDetail.MonthlyRentalRate },
                            {"FootRate", RentalReservationGroup.RentalReservationGroupDetail.FootRate },
                            {"FootRateCAN", RentalReservationGroup.RentalReservationGroupDetail.FootRateCAN },
                            {"FootRateLA", RentalReservationGroup.RentalReservationGroupDetail.FootRateLA },
                            {"FootRateCE", RentalReservationGroup.RentalReservationGroupDetail.FootRateCE },
                            {"FootRateAUD", RentalReservationGroup.RentalReservationGroupDetail.FootRateAUD },
                            {"FootRateINT", RentalReservationGroup.RentalReservationGroupDetail.FootRateINT },
                            {"ListRateCAN", RentalReservationGroup.RentalReservationGroupDetail.ListRateCAN },
                            {"ListRateLA", RentalReservationGroup.RentalReservationGroupDetail.ListRateLA },
                            {"ListRateCE", RentalReservationGroup.RentalReservationGroupDetail.ListRateCE },
                            {"ListRateAUD", RentalReservationGroup.RentalReservationGroupDetail.ListRateAUD },
                            {"ListRateINT", RentalReservationGroup.RentalReservationGroupDetail.ListRateINT },
                             {"EnterUserStr", "WWM\\"+ RentalReservationGroup.EnterUserStr},
                                    {"FullDescription", RentalReservationGroup.RentalReservationGroupDetail.FullDescription },
                            {"MarketingDescription", RentalReservationGroup.RentalReservationGroupDetail.MarketingDescription },
                            {"PreventiveMaintenance250Cost", RentalReservationGroup.RentalReservationGroupDetail.PreventiveMaintenance250Cost },
                            {"PreventiveMaintenance250Price", RentalReservationGroup.RentalReservationGroupDetail.PreventiveMaintenance250Price },
                            {"PreventiveMaintenance500Cost", RentalReservationGroup.RentalReservationGroupDetail.PreventiveMaintenance500Cost },
                            {"PreventiveMaintenance500Price", RentalReservationGroup.RentalReservationGroupDetail.PreventiveMaintenance500Price },
                            {"PreventiveMaintenance750Cost", RentalReservationGroup.RentalReservationGroupDetail.PreventiveMaintenance750Cost }, 
                            {"PreventiveMaintenance750Price", RentalReservationGroup.RentalReservationGroupDetail.PreventiveMaintenance750Price },
                            {"PreventiveMaintenance1000Cost", RentalReservationGroup.RentalReservationGroupDetail.PreventiveMaintenance1000Cost },
                            {"PreventiveMaintenance1000Price", RentalReservationGroup.RentalReservationGroupDetail.PreventiveMaintenance1000Price },
                            {"PreventiveMaintenanceCost", RentalReservationGroup.RentalReservationGroupDetail.PreventiveMaintenanceCost },
                            {"PreventiveMaintenancePrice", RentalReservationGroup.RentalReservationGroupDetail.PreventiveMaintenancePrice },
                            {"DamageInsPerHourCost", RentalReservationGroup.RentalReservationGroupDetail.DamageInsPerHourCost },
                            {"DamageInsPerHourPrice", RentalReservationGroup.RentalReservationGroupDetail.DamageInsPerHourPrice }, 
                            {"DamageInsPerMonthCost", RentalReservationGroup.RentalReservationGroupDetail.DamageInsPerMonthCost },
                            {"DamageInsPerMonthPrice", RentalReservationGroup.RentalReservationGroupDetail.DamageInsPerMonthPrice },
                            {"ShippingPerMile25Cost", RentalReservationGroup.RentalReservationGroupDetail.ShippingPerMile25Cost },
                            {"ShippingPerMile25Price", RentalReservationGroup.RentalReservationGroupDetail.ShippingPerMile25Price },
                            {"ShippingPerMile50Cost", RentalReservationGroup.RentalReservationGroupDetail.ShippingPerMile50Cost },
                            {"ShippingPerMile50Price", RentalReservationGroup.RentalReservationGroupDetail.ShippingPerMile50Price },
                            {"ShippingPerMile50AboveCost", RentalReservationGroup.RentalReservationGroupDetail.ShippingPerMile50AboveCost },
                            {"ShippingPerMile50AbovePrice", RentalReservationGroup.RentalReservationGroupDetail.ShippingPerMile50AbovePrice },
                            {"ShippingPerHour4MinimumCost", RentalReservationGroup.RentalReservationGroupDetail.ShippingPerHour4MinimumCost },
                            {"ShippingPerHour4MinimumPrice", RentalReservationGroup.RentalReservationGroupDetail.ShippingPerHour4MinimumPrice },
                            {"ShippingPerHour4AboveCost", RentalReservationGroup.RentalReservationGroupDetail.ShippingPerHour4AboveCost },
                            {"ShippingPerHour4AbovePrice", RentalReservationGroup.RentalReservationGroupDetail.ShippingPerHour4AbovePrice },
                            {"ShippingFlatRateCost", RentalReservationGroup.RentalReservationGroupDetail.ShippingFlatRateCost },
                            {"ShippingFlatRatePrice", RentalReservationGroup.RentalReservationGroupDetail.ShippingFlatRatePrice }
                };
                bool rentalReservationGroupDetailID = db.UpdateRentalreservationgroupDetail(sqlParamsForRentalReservationGroupDetail);

                #region photo
                if (RentalReservationGroup.RentalReservationGroupDetail.Photos != null)
                {
                    foreach (RentalReservationPhoto photo in RentalReservationGroup.RentalReservationGroupDetail.Photos)
                    {
                        if (photo.RentalReservationPhotosID == 0)  //existing models photo insert.
                        {
                            JObject sqlParamsForModelsPhoto = new JObject{
                            {"RentalReservationID", RentalReservationGroup.RentalReservationGroupID},
                            {"IsPrimary", photo.IsPrimary},
                            {"PhotoName", photo.PhotoName},
                            {"PhotoType", photo.PhotoType},
                            {"PhotoUrl", photo.PhotoUrl},
                            {"EnterUserStr", "WWM\\"+ RentalReservationGroup.EnterUserStr}
                            };
                            db.CreateRentalReservationPhoto(sqlParamsForModelsPhoto);
                        }
                        else if (!photo.Active)       //deativate models photo
                        {
                            JObject sqlParamsForPhoto = new JObject{
                             {"RentalReservationPhotosID", photo.RentalReservationPhotosID},
                             {"EditUserStr", "WWM\\"+ RentalReservationGroup.EnterUserStr},
                            };
                            db.DeactivateRentalReservationPhoto(sqlParamsForPhoto);
                        }
                        else                      //update model photo
                        {
                            JObject sqlParamsForModelsPhoto = new JObject{
                            {"RentalReservationPhotosID", photo.RentalReservationPhotosID},
                            {"IsPrimary", photo.IsPrimary},
                            {"EditUserStr", "WWM\\"+ RentalReservationGroup.EnterUserStr}
                            };
                            db.UpdateRentalReservationPhoto(sqlParamsForModelsPhoto);
                        }

                    }
                }
                #endregion
                #region files
                if (RentalReservationGroup.RentalReservationGroupDetail.Files != null)
                {
                    foreach (RentalReservationFile files in RentalReservationGroup.RentalReservationGroupDetail.Files)
                    {
                        if (files.RentalReservationFilesID == 0)  //existing models file insert.
                        {
                            JObject sqlParamsForModelsFile = new JObject{
                            {"RentalReservationID", RentalReservationGroup.RentalReservationGroupID},
                            {"FileName", files.FileName},
                            {"FileType", files.FileType},
                            {"FileUrl", files.FileUrl},
                            {"EnterUserStr", "WWM\\"+ RentalReservationGroup.EnterUserStr}
                            };
                            db.CreateRentalReservationFile(sqlParamsForModelsFile);
                        }
                        else if (!files.Active)       //deativate models file
                        {
                            JObject sqlParamsForFile = new JObject{
                             {"RentalReservationFilesID", files.RentalReservationFilesID},
                             {"EditUserStr", "WWM\\"+ RentalReservationGroup.EnterUserStr},
                            };
                            db.DeactivateRentalReservationFiles(sqlParamsForFile);
                        }

                    }
                }
                #endregion



                db.CommitSqlTranscation();
                return true;
            }
            catch (Exception e)
            {
                db.RollBackSqlTranscation();
                return false;
            }
        }

        public static RentalReservationGroupView View(JObject token)
        {
            var db = DAL.GetInstance();
            RentalReservationGroupView view = new RentalReservationGroupView();
            try
            {
                view = db.getRentalReservationGroupViewByID(token);
                var sqlParams = new JObject { { "RentalReservationGroup", view.RentalReservationGroup } };
                view.Models = db.getOtherModelonRentalReservationGroup(sqlParams);
                view.Photos = db.getRentalReservationPhoto(token);
                view.Files = db.getRentalReservationFiles(token);
                return view;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static bool Deactivate(ShowHide data)
        {
            var db = DAL.GetInstance();
         
            try
            {
                db.BeginSqlTranscation();
                foreach(ShowHideItem r in data.RentalReservationGroup)
                {
                    JObject sqlParamsForRentalReservationGroupDetail = new JObject{
                            {"RentalReservationGroupID", r.ID},
                            {"EditUserStr", data.EnterUserStr },
                            {"Active", r.Active }
                     };
                     db.DeactivateReservationGroupDetail(sqlParamsForRentalReservationGroupDetail);
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
}
