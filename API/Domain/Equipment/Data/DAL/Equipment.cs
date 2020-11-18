using API.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace API.Data
{
    public partial class DAL
    {
        public IEnumerable<Equipment> getEquipment(JObject sqlParams, string EquipmentType = null)
        {
            string cmdText = ConfigurationManager.AppSettings["EquipmentSearch"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            if (EquipmentType != null) cmd.Parameters.AddWithValue("@EquipmentOrAttachment", EquipmentType);
            return getRecords<Equipment>(cmdText, sqlParams);
        }

        public IEnumerable<EquipmentPublic> getEquipmentPublic(JObject sqlParams, string EquipmentType = null)
        {
            string cmdText = ConfigurationManager.AppSettings["EquipmentSearch"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            if (EquipmentType != null) cmd.Parameters.AddWithValue("@EquipmentOrAttachment", EquipmentType);
            cmd.Parameters.AddWithValue("@PublicViewable", true);
            return getRecords<EquipmentPublic>(cmdText, sqlParams);
        }

        public DataSet getEquipmentAdmin(JObject sqlParams, string EquipmentType = null)
        {
            string cmdText = ConfigurationManager.AppSettings["EquipmentAdminSearch"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getDataSetEquipmentAdmin(cmdText, sqlParams);
        }

        public Equipment getEquipmentByEquipmentID(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["getEquipmentByEquipmentID"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<Equipment>(cmdText, sqlParams).FirstOrDefault();
        }

        public IEnumerable<Attachment> getEquipmentAttachments(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["EquipmentAttachments"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<Attachment>(cmdText, sqlParams);
        }

        public IEnumerable<Equipment> getMachinesForSale(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["EquipmentSearch"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            cmd.Parameters.AddWithValue("@ForSale", true);
            var data = getRecords<Equipment>(cmdText, sqlParams);
            //ADD IN PICTURE FILENAMES AND ATTACHMENTS

            getPictureFileNames(data);
            getAttachments(data);

            return data;
        }

        public IEnumerable<Equipment> getMachinesAndAttachmentsForSale(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["EquipmentSearch"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            cmd.Parameters.AddWithValue("@ForSale", true);
            cmd.Parameters.AddWithValue("@EquipmentOrAttachment", "Both");
            var data = getRecords<Equipment>(cmdText, sqlParams);

            getPictureFileNames(data);
            getAttachments(data);

            return data;
        }


        public IEnumerable<Equipment> getEquipmentComponentHistory(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["ComponentHistory"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<Equipment>(cmdText, sqlParams);
        }

        public Equipment getEquipmentBySerial(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["MachineBySerial"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            var equipment = getRecords<Equipment>(cmdText, sqlParams);
            return equipment.FirstOrDefault();
        }

        public Equipment getAttachmentBySerial(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["AttachmentBySerial"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            var equipment = getRecords<Equipment>(cmdText, sqlParams);
            return equipment.FirstOrDefault();
        }
        public Equipment getAttachmentByEquipmentID(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["AttachmentByEquipmentID"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            var equipment = getRecords<Equipment>(cmdText, sqlParams);
            return equipment.FirstOrDefault();
        }

        private RateData equipmentRateTransform(DataTable data)
        {
            List<RateContainer> rates = new List<RateContainer>();
            string CurrentCategory = null;
            string CurrentSubCategory = null;
            foreach (var row in data.AsEnumerable())
            {
                var EquipmentType = row[0].ToString();
                var EquipmentCategory = row[1].ToString();
                var EquipmentModel = row[2].ToString();
                var MonthlyRate = row[3].ToString();
                var WeeklyRate = row[4].ToString();
                var SubCategory = row[5].ToString();
                var CategoryChange = false;
                var SubCategoryChange = false;
                if (EquipmentCategory != CurrentCategory)
                {
                    CurrentCategory = EquipmentCategory;
                    CategoryChange = true;
                }
                if (CurrentSubCategory != SubCategory && !String.IsNullOrEmpty(SubCategory))
                {
                    CurrentSubCategory = SubCategory;
                    SubCategoryChange = true;
                }

                var EquipmentTypeAlreadyExists = rates.Exists(r => r.EquipmentType == EquipmentType);

                if (EquipmentTypeAlreadyExists)
                {
                    var EquipmentTypeToUpdate = rates.Find(r => r.EquipmentType == EquipmentType);
                    var EquipmentCategoryAlreadyExists = EquipmentTypeToUpdate.EquipmentCategories.Exists(cat => cat.EquipmentCategory == EquipmentCategory);

                    if (EquipmentCategoryAlreadyExists)
                    {
                        var EquipmentCategoryToUpdate = EquipmentTypeToUpdate.EquipmentCategories.Find(cat => cat.EquipmentCategory == EquipmentCategory);
                        var EquipmentSubCategoryToUpdate = GetSubCategoryToUpdate(EquipmentCategoryToUpdate, CurrentSubCategory);

                        if (EquipmentSubCategoryToUpdate != null && !CategoryChange)
                        {
                            var rate = new Rate();
                            rate.model = EquipmentModel;
                            rate.weekly = WeeklyRate;
                            rate.monthly = MonthlyRate;

                            EquipmentSubCategoryToUpdate.Rates.Add(rate);
                        }
                        else if (SubCategoryChange)
                        {
                            var rateHeader = new RateSubCategory();
                            rateHeader.SubCategory = SubCategory;
                            rateHeader.Rates = new List<Rate>();

                            EquipmentCategoryToUpdate.Values.Add(rateHeader);

                            EquipmentCategoryToUpdate = EquipmentTypeToUpdate.EquipmentCategories.Find(cat => cat.EquipmentCategory == EquipmentCategory);
                            EquipmentSubCategoryToUpdate = GetSubCategoryToUpdate(EquipmentCategoryToUpdate, CurrentSubCategory);
                            var rate = new Rate();
                            rate.model = EquipmentModel;
                            rate.weekly = WeeklyRate;
                            rate.monthly = MonthlyRate;

                            EquipmentSubCategoryToUpdate.Rates.Add(rate);

                        }
                        else
                        {
                            var rate = new Rate();
                            rate.model = EquipmentModel;
                            rate.weekly = WeeklyRate;
                            rate.monthly = MonthlyRate;
                            EquipmentCategoryToUpdate.Values.Add(rate);
                        }
                    }
                    else if (String.IsNullOrEmpty(SubCategory))
                    {
                        var rateCategory = new RateCategory();
                        rateCategory.EquipmentCategory = EquipmentCategory;
                        rateCategory.Values = new List<dynamic>();

                        var rate = new Rate();
                        rate.model = EquipmentModel;
                        rate.weekly = WeeklyRate;
                        rate.monthly = MonthlyRate;

                        rateCategory.Values.Add(rate);
                        EquipmentTypeToUpdate.EquipmentCategories.Add(rateCategory);
                    }
                    else if (!String.IsNullOrEmpty(SubCategory))
                    {
                        var rateCategory = new RateCategory();
                        rateCategory.EquipmentCategory = EquipmentCategory;
                        rateCategory.Values = new List<dynamic>();

                        var rateHeader = new RateSubCategory();
                        rateHeader.SubCategory = SubCategory;
                        rateHeader.Rates = new List<Rate>();

                        rateCategory.Values.Add(rateHeader);
                        EquipmentTypeToUpdate.EquipmentCategories.Add(rateCategory);



                        EquipmentTypeToUpdate = rates.Find(r => r.EquipmentType == EquipmentType);
                        CurrentSubCategory = SubCategory;
                        var EquipmentCategoryToUpdate = EquipmentTypeToUpdate.EquipmentCategories.Find(cat => cat.EquipmentCategory == EquipmentCategory);
                        var EquipmentSubCategoryToUpdate = GetSubCategoryToUpdate(EquipmentCategoryToUpdate, CurrentSubCategory);
                        var rate = new Rate();
                        rate.model = EquipmentModel;
                        rate.weekly = WeeklyRate;
                        rate.monthly = MonthlyRate;
                        EquipmentSubCategoryToUpdate.Rates.Add(rate);
                    }
                }
                else
                {
                    var rateContainer = new RateContainer();
                    rateContainer.EquipmentType = EquipmentType;
                    rateContainer.EquipmentCategories = new List<RateCategory>();

                    var rateCategory = new RateCategory();
                    rateCategory.EquipmentCategory = EquipmentCategory;
                    rateCategory.Values = new List<dynamic>();

                    var rate = new Rate();
                    rate.model = EquipmentModel;
                    rate.weekly = WeeklyRate;
                    rate.monthly = MonthlyRate;

                    rateContainer.EquipmentCategories.Add(rateCategory);
                    int flag = 0;
                    if (String.IsNullOrEmpty(SubCategory))
                    {
                        rateCategory.Values.Add(rate);
                    }
                    else
                    {
                        var rateSubCategory = new RateSubCategory();
                        rateSubCategory.SubCategory = SubCategory;
                        rateSubCategory.Rates = new List<Rate>();
                        rateCategory.Values.Add(rateSubCategory);
                    

                    
                        flag = 1;
                    }


                    rates.Add(rateContainer);

                    if(flag ==1)
                    {
                        var EquipmentTypeToUpdate = rates.Find(r => r.EquipmentType == EquipmentType);
                        CurrentSubCategory = SubCategory;
                        var EquipmentCategoryToUpdate = EquipmentTypeToUpdate.EquipmentCategories.Find(cat => cat.EquipmentCategory == EquipmentCategory);
                        var EquipmentSubCategoryToUpdate = GetSubCategoryToUpdate(EquipmentCategoryToUpdate, CurrentSubCategory);
                       
                        EquipmentSubCategoryToUpdate.Rates.Add(rate);
                    }
                }
            }

            var rateData = new RateData();
            rateData.Rates = rates;
            rateData.Currencies = getRecords<RateCurrency>("m2.getRateCurrencyMultipliers");
            return rateData;
        }

        private bool PropertyExists(dynamic obj, string prop)
        {
            return obj.GetType().GetProperty(prop) != null;
        }

        private dynamic GetSubCategoryToUpdate(dynamic obj, string SubCategory)
        {
            foreach (var category in obj.Values)
            {
                if (PropertyExists(category, "SubCategory"))
                {
                    if (category.SubCategory == SubCategory)
                        return category;
                }
            }

            return null;
        }

        public void getAttachments(IEnumerable<Equipment> machines)
        {
            if (sqlConn.State == ConnectionState.Closed) sqlConn.Open();

            foreach (var machine in machines)
            {
                List<Attachment> attachments = new List<Attachment>();
                cmd.Connection = sqlConn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[m2].[GetAttachmentsMobile]";
                cmd.Parameters.AddWithValue("@equipmentID", machine.EquipmentID);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var attachment = new Attachment();
                    attachment.EquipmentID = rdr.GetInt32(0);
                    attachment.SerialNum = rdr.GetString(1);
                    attachment.AttachmentPosition = rdr.GetString(2);
                    attachment.AttachmentType = rdr.GetString(5);
                    attachments.Add(attachment);
                }

                rdr.Close();
                rdr.Dispose();
                cmd.Parameters.Clear();
                machine.Attachments = attachments;
            }


            if (sqlConn.State != ConnectionState.Closed)
                sqlConn.Close();
        }

        public void getAttachments(IEnumerable<EquipmentPublic> machines)
        {
            if (sqlConn.State == ConnectionState.Closed) sqlConn.Open();

            foreach (var machine in machines)
            {
                List<Attachment> attachments = new List<Attachment>();
                cmd.Connection = sqlConn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[m2].[GetAttachmentsMobile]";
                cmd.Parameters.AddWithValue("@equipmentID", machine.EquipmentID);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var attachment = new Attachment();
                    attachment.EquipmentID = rdr.GetInt32(0);
                    attachment.SerialNum = rdr.GetString(1);
                    attachment.AttachmentPosition = rdr.GetString(2);
                    attachment.AttachmentType = rdr.GetString(5);
                    attachments.Add(attachment);
                }

                rdr.Close();
                rdr.Dispose();
                cmd.Parameters.Clear();
                machine.Attachments = attachments;
            }


            if (sqlConn.State != ConnectionState.Closed)
                sqlConn.Close();
        }

        public void getPictureFileNames(IEnumerable<Equipment> machines)
        {
            if (sqlConn.State == ConnectionState.Closed) sqlConn.Open();

            foreach (var machine in machines)
            {
                List<string> pictureUrls = new List<string>();
                cmd.Connection = sqlConn;
                cmd.CommandText = "[m2].[EquipmentPhotoURLsByEquipmentID]";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@equipmentID", machine.EquipmentID);
                cmd.Parameters.AddWithValue("@size", "small");
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    pictureUrls.Add(ConfigurationManager.AppSettings["PublicSiteBaseURL"] + "/images/inventory/" + rdr.GetString(0));
                }

                rdr.Close();
                rdr.Dispose();
                cmd.Parameters.Clear();
                machine.Pictures = pictureUrls;
            }

            if (sqlConn.State != ConnectionState.Closed)
                sqlConn.Close();
        }

        public void getPictureFileNames(IEnumerable<EquipmentPublic> machines)
        {
            if (sqlConn.State == ConnectionState.Closed) sqlConn.Open();

            foreach (var machine in machines)
            {
                List<string> pictureUrls = new List<string>();
                cmd.Connection = sqlConn;
                cmd.CommandText = "[m2].[EquipmentPhotoURLsByEquipmentID]";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@equipmentID", machine.EquipmentID);
                cmd.Parameters.AddWithValue("@size", "small");
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    pictureUrls.Add(ConfigurationManager.AppSettings["PublicSiteBaseURL"] + "/images/inventory/" +rdr.GetString(0));
                }
                rdr.Close();
                rdr.Dispose();
                cmd.Parameters.Clear();
                machine.Pictures = pictureUrls;
            }

            if (sqlConn.State != ConnectionState.Closed)
                sqlConn.Close();
        }

        public void getContract(IEnumerable<Equipment> machines)
        {
            if (sqlConn.State == ConnectionState.Closed) sqlConn.Open();

            foreach (var machine in machines)
            {
                Contract contract = new Contract();
                cmd.Connection = sqlConn;
                cmd.CommandText = "[m2].[getCurrentContractDetails]";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@equipmentID", machine.EquipmentID);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    contract.RentingCompany = rdr.GetString(6);
                }

                rdr.Close();
                rdr.Dispose();
                cmd.Parameters.Clear();
                machine.ContractDetails = contract;
            }

            if (sqlConn.State != ConnectionState.Closed)
                sqlConn.Close();
        }

        public bool updateEquipmentListRates(JObject tokens)
        {
            string cmdText = ConfigurationManager.AppSettings["UpdateEquipmentListRates"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecord(cmdText, tokens);
        }

        public bool updateEquipmentListPrices(JObject tokens)
        {
            string cmdText = ConfigurationManager.AppSettings["UpdateEquipmentListPrices"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecord(cmdText, tokens);
        }

        public bool updateEquipment(JObject tokens)
        {
            string cmdText = ConfigurationManager.AppSettings["EquipmentUpdate"];
            if (string.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecord(cmdText, tokens);
        }

        public DataTable getEquipmentAudit(JObject tokens)
        {
            string cmdText = ConfigurationManager.AppSettings["GetEquipmentAudit"];
            if (String.IsNullOrWhiteSpace(cmdText)) return new DataTable();
            return getRecords(cmdText, tokens);
        }

        public DataTable getEquipmentOwnerHistory(JObject tokens)
        {
            string cmdText = ConfigurationManager.AppSettings["GetEquipmentOwnerHistory"];
            if (String.IsNullOrWhiteSpace(cmdText)) return new DataTable();
            return getRecords(cmdText, tokens);
        }

        public DataTable getEquipmentListRateHistory(JObject tokens)
        {
            string cmdText = ConfigurationManager.AppSettings["GetEquipmentListRateHistory"];
            if (String.IsNullOrWhiteSpace(cmdText)) return new DataTable();
            return getRecords(cmdText, tokens);
        }

        public DataTable getEquipmentPriceHistory(JObject tokens)
        {
            string cmdText = ConfigurationManager.AppSettings["GetEquipmentPriceHistory"];
            if (String.IsNullOrWhiteSpace(cmdText)) return new DataTable();
            return getRecords(cmdText, tokens);
        }

        public DataTable getEquipmentListRates(JObject tokens)
        {
            string cmdText = ConfigurationManager.AppSettings["GetEquipmentListRates"];
            if (String.IsNullOrWhiteSpace(cmdText)) return new DataTable();
            return getRecords(cmdText, tokens);
        }

        public DataTable getLastRateChangeDates(JObject tokens)
        {
            string cmdText = ConfigurationManager.AppSettings["GetAllLastRateChangeDates"];
            if (String.IsNullOrWhiteSpace(cmdText)) return new DataTable();
            return getRecords(cmdText, tokens);
        }

        public DataTable getEquipmentListPrices(JObject tokens)
        {
            string cmdText = ConfigurationManager.AppSettings["GetEquipmentListPrices"];
            if (String.IsNullOrWhiteSpace(cmdText)) return new DataTable();
            return getRecords(cmdText, tokens);
        }

        public RateData getMachineRates()
        {
            string cmdText = ConfigurationManager.AppSettings["MachineRates"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            Func<DataTable, RateData> transform = equipmentRateTransform;
            return getRecords<RateData>(cmdText, transform);
        }

        public int createEquipmentPicture(JObject tokens)
        {
            string cmdText = ConfigurationManager.AppSettings["CreateEquipmentPictures"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return InsertRecord(cmdText, tokens);
        }

        public bool deactivateEquipmentPhoto(JObject tokens)
        {
            string cmdText = ConfigurationManager.AppSettings["DeactivateEquipmentPicture"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecord(cmdText, tokens);
        }

        public IEnumerable<string> getEquipmentCategories(string EquipmentType = null)
        {
            string cmdText = ConfigurationManager.AppSettings["CategoryFilters"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            if (EquipmentType != null) cmd.Parameters.AddWithValue("@EquipmentOrAttachment", EquipmentType);
            return getRecords<string>(cmdText);
        }

        public IEnumerable<MachineSimple> getMachineDataByModelNum(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["MachineInfoByModel"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<MachineSimple>(cmdText, sqlParams);
        }

        public IEnumerable<string> getEquipmentManufacturers(JObject sqlParams = null, string EquipmentType = null)
        {
            string cmdText = ConfigurationManager.AppSettings["ManufacturerFilters"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            if (EquipmentType != null) cmd.Parameters.AddWithValue("@EquipmentOrAttachment", EquipmentType);
            return getRecords<string>(cmdText, sqlParams);
        }

        public IEnumerable<string> getEquipmentModels(JObject sqlParams = null, string EquipmentType = null)
        {
            string cmdText = ConfigurationManager.AppSettings["ModelFilters"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            if (EquipmentType != null) cmd.Parameters.AddWithValue("@EquipmentOrAttachment", EquipmentType);
            return getRecords<string>(cmdText, sqlParams);
        }

        public IEnumerable<string> getEquipmentLocations(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["LocationFilters"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<string>(cmdText, sqlParams);
        }

        public IEnumerable<string> getAttachmentTypes(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["AttachmentTypes"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<string>(cmdText, sqlParams);
        }

        public IEnumerable<string> getAttachmentCategoriesOrTypes(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["GetAttachmentCategoriesOrTypes"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<string>(cmdText, sqlParams);
        }
        public IEnumerable<string> GetAttachmentCategoryfromFitsOn(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["GetAttachmentCategoryfromFitsOn"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<string>(cmdText, sqlParams);
        }

        public IEnumerable<string> getEquipmentPhotos(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["EquipmentPhotos"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<string>(cmdText, sqlParams);
        }

        public IEnumerable<string> getRentalStatuses(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["RentalStatus"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<string>(cmdText, sqlParams);
        }

        public IEnumerable<string> getPictureFileNames(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["EquipmentPhotos"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            List<string> filesnames =  getRecords<string>(cmdText, sqlParams);
            return filesnames.Select(s => ConfigurationManager.AppSettings["PublicSiteBaseURL"] + "/images/inventory/" + s).ToList();
            
        }

        public IEnumerable<Equipment> getMachinesForSale()
        {
            return getMachinesForSale(null);
        }

        public IEnumerable<decimal> getModelMonthlyRate(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["ModelMonthlyRate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<decimal>(cmdText, sqlParams);
        }

        public EquipmentDetailCustomerPortal GetEquipmentDetailOnCustomerContract(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["GetEquipmentDetailOnCustomerContract"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<EquipmentDetailCustomerPortal>(cmdText, sqlParams).FirstOrDefault();
        }

        public List<AttachmentNameAndPosition> getContractDetailAttachmentNameByContractDtlID(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["GetContractDetailAttachmentNameByContractDtlID"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<AttachmentNameAndPosition>(cmdText, sqlParams);
        }
        //CP-193

        public IEnumerable<EquipmentOnRent> GetAllEquipmentOnRentByEmail(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["GetAllEquipmentOnRentByEmail"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<EquipmentOnRent>(cmdText, sqlParams);
        }

        public IEnumerable<CheckInOutLast> getCheckInOut(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["GetLastCheckInOutDateByEquipmentID"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<CheckInOutLast>(cmdText, sqlParams);
        }

    }
}