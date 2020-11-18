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
        public DataTable getModelListRates(JObject tokens)
        {
            string cmdText = ConfigurationManager.AppSettings["GetModelListRates"];
            if (String.IsNullOrWhiteSpace(cmdText)) return new DataTable();
            return getRecords(cmdText, tokens);
        }

        public bool updateModelListRates(JObject tokens)
        {
            string cmdText = ConfigurationManager.AppSettings["UpdateModelListRates"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecord(cmdText, tokens);
        }

        public DataTable getAdditionalModelColumns(JObject tokens)
        {
            string cmdText = ConfigurationManager.AppSettings["GetModelAdditionalFields"];
            if (String.IsNullOrWhiteSpace(cmdText)) return new DataTable();
            return getRecords(cmdText, tokens);
        }

        public bool updateAdditionalModelColumns(JObject tokens)
        {
            string cmdText = ConfigurationManager.AppSettings["UpdateModelAdditionalFields"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecord(cmdText, tokens);
        }

        public Model getModelFields(JObject tokens)
        {
            string cmdText = ConfigurationManager.AppSettings["GetModelFields"];
            if (String.IsNullOrWhiteSpace(cmdText)) return new Model();
            return getRecords<Model>(cmdText, tokens).FirstOrDefault();
        }
        public Model getModelFieldsByEquipmentID(JObject tokens)
        {
            string cmdText = ConfigurationManager.AppSettings["GetModelFieldsByEquipmentID"];
            if (String.IsNullOrWhiteSpace(cmdText)) return new Model();
            return getRecords<Model>(cmdText, tokens).FirstOrDefault();
        }

        public List<ModelRentalData> getModelData()
        {
            string cmdText = ConfigurationManager.AppSettings["GetModelData"];
            if (String.IsNullOrWhiteSpace(cmdText)) return new List<ModelRentalData>();
            return getRecords<ModelRentalData>(cmdText);
        }

        public ModelMobileView getModelDetail(JObject tokens)
        {
            string cmdText = ConfigurationManager.AppSettings["GetModelDetail"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<ModelMobileView>(cmdText, tokens).FirstOrDefault();
        }

        public List<ModelSpec> getModelSpecs(JObject tokens)
        {
            string cmdText = ConfigurationManager.AppSettings["GetModelSpecsByInventoryMasterID"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<ModelSpec>(cmdText, tokens);
        }

        public List<ModelPhoto> getModelPhotos(JObject tokens)
        {
            string cmdText = ConfigurationManager.AppSettings["GetModelPhotosByInventoryMasterID"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<ModelPhoto>(cmdText, tokens);
        }

        public List<CategoryImages> getProductCategoryImage(JObject tokens)
        {
            string cmdText = ConfigurationManager.AppSettings["GetCategoryImagesByProductCategoryID"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<CategoryImages>(cmdText, tokens);
        }

        public List<ModelFile> getModelFiles(JObject tokens)
        {
            string cmdText = ConfigurationManager.AppSettings["GetModelFilesByInventoryMasterID"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<ModelFile>(cmdText, tokens);
        }
        public List<ModelCompetitor> getModelCompetitors(JObject tokens)
        {
            string cmdText = ConfigurationManager.AppSettings["GetModelCompetitorsByInventoryMasterID"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<ModelCompetitor>(cmdText, tokens);
        }
        public List<RentalReservationPhoto> getRentalReservationPhoto(JObject tokens)
        {
            string cmdText = ConfigurationManager.AppSettings["GetRentalReservationPhotosByRRGID"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<RentalReservationPhoto>(cmdText, tokens);
        }

        public List<RentalReservationFile> getRentalReservationFiles(JObject tokens)
        {
            string cmdText = ConfigurationManager.AppSettings["GetRentalReservationFilesByRRGID"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<RentalReservationFile>(cmdText, tokens);
        }

        public int CreateNewInventoryMaster(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["ModelsInventoryMasterCreate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return InsertRecordInTrans(cmdText, sqlParams);
        }

        public int CreateNewModelSpec(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["ModelsSpecCreate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return InsertRecordInTrans(cmdText, sqlParams);
        }

        public int CreateNewModelFile(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["ModelsFilesCreate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return InsertRecordInTrans(cmdText, sqlParams);
        }

        public int UpdateRentalReservationFile(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["RentalReservationFilesUpdate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return InsertRecordInTrans(cmdText, sqlParams);
        }

        public int CreateRentalReservationFile(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["RentalReservationFilesCreate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return InsertRecordInTrans(cmdText, sqlParams);
        }

        public int CreateNewModelPhoto(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["ModelsPhotosCreate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return InsertRecordInTrans(cmdText, sqlParams);
        }

        public bool UpdateRentalReservationPhoto(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["RentalReservationPhotosUpdate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecordInTrans(cmdText, sqlParams);
        }

        public int CreateRentalReservationPhoto(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["RentalReservationPhotosCreate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return InsertRecordInTrans(cmdText, sqlParams);
        }

        public int CreateNewCategoryImage(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["CategoryImagesCreate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return InsertRecordInTrans(cmdText, sqlParams);
        }
        public int CreateProductCategoryDetail(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["ProductCategoryDetailCreate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return InsertRecordInTrans(cmdText, sqlParams);
        }
        public int UpdateProductCategoryDetail(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["ProductCategoryDetailUpdate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return InsertRecordInTrans(cmdText, sqlParams);
        }

        public int CreateNewModelCompetitor(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["ModelsCompetitorCreate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return InsertRecordInTrans(cmdText, sqlParams);
        }
        public int CreateNewModelExtension(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["ModelsCreate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return InsertRecordInTrans(cmdText, sqlParams);
        }

        public bool UpdateModelExtension(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["ModelsUpdate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecordInTrans(cmdText, sqlParams);
        }


        public bool UpdateInventoryMaster(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["ModelsInventoryMasterUpdate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecordInTrans(cmdText, sqlParams);
        }

        public bool UpdateModelsSpec(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["ModelsSpecUpdate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecordInTrans(cmdText, sqlParams);
        }

        public bool DeactivateModelsSpec(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["ModelsSpecDeactivate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecordInTrans(cmdText, sqlParams);
        }

        public bool DeactivateModelsPhoto(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["ModelsPhotosDeactivate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecordInTrans(cmdText, sqlParams);
        }

         public bool DeactivateRentalReservationPhoto(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["RentalReservationPhotosDeactivate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecordInTrans(cmdText, sqlParams);
        }

         public bool DeactivateRentalReservationFiles(JObject sqlParams)
         {
             string cmdText = ConfigurationManager.AppSettings["RentalReservationFilesDeactivate"];
             if (String.IsNullOrWhiteSpace(cmdText)) return false;
             return UpdateRecordInTrans(cmdText, sqlParams);
         }

        public bool DeactivateCetegoryImage(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["CategoryImagesDeactivate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecordInTrans(cmdText, sqlParams);
        }

        public bool UpdateCategoryImage(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["CategoryImagesUpdate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecordInTrans(cmdText, sqlParams);
        }

        public bool UpdateModelsPhoto(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["ModelsPhotosUpdate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecordInTrans(cmdText, sqlParams);
        }
        public bool DeactivateModelsFile(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["ModelsFilesDeactivate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecordInTrans(cmdText, sqlParams);
        }
        public bool DeactivateModelsCompetitor(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["ModelsCompetitorDeactivate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecordInTrans(cmdText, sqlParams);
        }
        public bool UpdateModelsCompetitor(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["ModelsCompetitorUpdate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecordInTrans(cmdText, sqlParams);
        }

        public int CreateCategory(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["ProductCategoryCreate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return InsertRecord(cmdText, sqlParams);
        }
        public int CreateCategoryInTrans(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["ProductCategoryCreate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return InsertRecordInTrans(cmdText, sqlParams);
        }

        public int CreateManufacturer(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["ManufacturerCreate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return InsertRecord(cmdText, sqlParams);
        }

        public int? CreateRentalreservationgroupifnotexist(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["RentalReservationGroupCreate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return InsertRecordInTrans(cmdText, sqlParams);
        }
        public bool UpdateRentalreservationgroup(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["RentalReservationGroupUpdate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecordInTrans(cmdText, sqlParams);
        }

        public int CreateRentalreservationgroupDetail(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["RentalReservationGroupDetailCreate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return InsertRecordInTrans(cmdText, sqlParams);
        }
        public List<string> getRentalReservationGroupList()
        {
            string cmdText = ConfigurationManager.AppSettings["RentalReservationGroupSelect"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<string>(cmdText);
        }

        public List<string>getOtherModelonRentalReservationGroup(JObject json)
        {
            string cmdText = ConfigurationManager.AppSettings["RentalReservationGrouponModel"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<string>(cmdText, json);
        }

        public bool UpdateRentalreservationgroupDetail(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["RentalReservationGroupDetailUpdate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecordInTrans(cmdText, sqlParams);
        }
        public RentalReservationGroupView getRentalReservationGroupViewByID(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["RentalReservationGroupView"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<RentalReservationGroupView>(cmdText, sqlParams).FirstOrDefault();
        }

        public ProductCategory getProductCategoryViewByID(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["ProductCategoryDetailView"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<ProductCategory>(cmdText, sqlParams).FirstOrDefault();
        }

        public bool DeactivateReservationGroupDetail(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["RentalReservationGroupDetailDeactivate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecordInTrans(cmdText, sqlParams);
        }
        public List<RentalReservationGroupModelList> RentalReservationGroupModelList(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["RentalReservationGroupModelList"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<RentalReservationGroupModelList>(cmdText, sqlParams);
        }

        public List<ModelGrid> ModelList(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["ModelsList"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<ModelGrid>(cmdText);
        }

        public List<ReservationGroupGrid> RentalReservationGroupGridList(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["RentalReservationGroupList"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<ReservationGroupGrid>(cmdText);
        }

        public List<CategoryGrid> CategoryGridList(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["CategoryGridList"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<CategoryGrid>(cmdText);
        }

        public int GetModelIDByModelNum(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["GetModelIDByModelNum"];
            if (String.IsNullOrWhiteSpace(cmdText)) return 0;
            return getRecords<int>(cmdText, sqlParams).FirstOrDefault();
        }


        public List<RentalReservationList> RentalReservationList(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["RentalReservation_List"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<RentalReservationList>(cmdText);
        }
    }
}