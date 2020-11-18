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

        public Contract getContractByEquipmentId(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["ContractDetails"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<Contract>(cmdText, sqlParams).FirstOrDefault();
        }

        public bool DeleteQARecords(string companyName)
        {
            string cmdText = ConfigurationManager.AppSettings["DeleteQaRecords"];
            JObject obj = new JObject { { "customer", companyName } };
            if (string.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecord(cmdText);
        }

        public Contract getContract(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["ContractDetails"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<Contract>(cmdText, sqlParams).FirstOrDefault();
        }

        public void getContractDetails(IEnumerable<Equipment> machines)
        {
            if (sqlConn.State == ConnectionState.Closed) sqlConn.Open();

            DataTable data = new DataTable("data");
            foreach (var machine in machines)
            {
                cmd.Connection = sqlConn;
                cmd.CommandText = "[m2].[getCurrentContractDetails]";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@equipmentID", machine.EquipmentID);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(data);
                machine.ContractDetails = data.ConvertToList<Contract>().FirstOrDefault();
                cmd.Parameters.Clear();
            }

            if (sqlConn.State != ConnectionState.Closed)
                sqlConn.Close();
        }


        public List<CustomerContractQuoteSummary> getContractQuoteSummary(JObject sqlParams)
        {
            string cmdText = ConfigurationManager.AppSettings["GetContractQuoteSummary"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<CustomerContractQuoteSummary>(cmdText, sqlParams);
        }

        /// <summary>
        /// Get all additional salespeople linked to a contract
        /// </summary>
        public DataTable getAdditionalSalespeople(JObject tokens)
        {
            string cmdText = ConfigurationManager.AppSettings["GetAdditionalSalespeople"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords(cmdText, tokens);
        }

        /// <summary>
        /// Get all additional salespeople linked to a contract
        /// </summary>
        public DataTable getAdditionalSalespeopleDelimited(JObject tokens)
        {
            string cmdText = ConfigurationManager.AppSettings["GetAdditionalSalespeopleDelimited"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords(cmdText, tokens);
        }

        /// <summary>
        /// Insert a new additional salesperson on a contract
        /// </summary>
        /// <param name="tokens">The tokens.</param>
        /// <returns></returns>
        public bool InsertAdditionalSalesperson(JObject tokens)
        {
            string cmdText = ConfigurationManager.AppSettings["InsertAdditionalSalesperson"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            var inserted = InsertRecord(cmdText, tokens);
            return inserted == 1;
        }

        /// <summary>
        /// Delete an existing additional salesperson on a contract
        /// </summary>
        /// <param name="tokens">The tokens.</param>
        /// <returns></returns>
        public bool DeleteAdditionalSalesperson(JObject tokens)
        {
            string cmdText = ConfigurationManager.AppSettings["DeleteAdditionalSalesperson"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecord(cmdText, tokens);
        }


        public DataTable getContractDetails(JObject tokens)
        {
            //if (sqlConn.State == ConnectionState.Closed) sqlConn.Open();

            //DataTable data = new DataTable("data");
            //foreach (var machine in machines)
            //{
            //    cmd.Connection = sqlConn;
            //    cmd.CommandText = "[m2].[getCurrentContractDetails]";
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.AddWithValue("@equipmentID", machine.EquipmentID);
            //    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            //    adapter.Fill(data);
            //    machine.ContractDetails = data.ConvertToList<Contract>().FirstOrDefault();
            //    cmd.Parameters.Clear();
            //}

            //if (sqlConn.State != ConnectionState.Closed)
            //    sqlConn.Close();

            //return data;

            string cmdText = ConfigurationManager.AppSettings["ContractDtls"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords(cmdText,tokens);
        }

        public ContractView getContractByContractID(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["GetContractByContractID"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<ContractView>(cmdText, sqlParams).FirstOrDefault();
        }
        public ContractView getContractByContractNum(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["GetContractByContractNum"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<ContractView>(cmdText, sqlParams).FirstOrDefault();
        }
        public List<ContractDetails> getContractDetailByContractID(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["GetContractDetailByContractID"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<ContractDetails>(cmdText, sqlParams);
        }

        public List<ContractDetailAttachment> getContractDetailAttachmentByContractDtlID(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["GetContractDetailAttachmentByContractDtlID"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<ContractDetailAttachment>(cmdText, sqlParams);
        }

        public bool InsertContractSignature(JObject tokens)
        {
            string cmdText = ConfigurationManager.AppSettings["ContractSignatureCreate"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return InsertData(cmdText, tokens);
            
        }
        public bool SaveImage(string ImgStr, string ImgName, string ImgType)
        {
            //string path =@"C:\Users\admin\Desktop\dbimage";
            string path = ConfigurationManager.AppSettings["ContractSignatureDirectory"]; 
            //Check if directory exist
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
            }

            string imageName = ImgName + "." + ImgType;

            //set the image path
            string imgPath = System.IO.Path.Combine(path, imageName);

            byte[] imageBytes = Convert.FromBase64String(ImgStr);

            System.IO.File.WriteAllBytes(imgPath, imageBytes);

            return true;
        }

        ///<summary>
        ///Get the insurance and credit status 
        ///</summary>
        ///<param name="contactID"></param>
        ///<returns></returns>
        public ContactInsuranceAndCreditStatus getStatus(int contactID)
        {
            string cmdText = ConfigurationManager.AppSettings["GetInsuranceStatus"];
            JObject obj = new JObject { { "ContactID", contactID } };
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<ContactInsuranceAndCreditStatus>(cmdText, obj).FirstOrDefault();
        }

        public List<ContractSignature> GetContractSignatureByContractID(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["GetContractSignatureByContractID"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<ContractSignature>(cmdText, sqlParams);
        }

        public List<ContractListCustomerPortal> GetListContractForCustomer(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["GetListofContractforCustomer"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<ContractListCustomerPortal>(cmdText, sqlParams);
        }

        public bool CheckContractIDMatchCustomerID(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["CheckContractIDMatchCustomerID"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return InsertRecord(cmdText,sqlParams)==1;
        }

        public bool JobInfoSheetSave(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["ContractJobSheetSave"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            return UpdateRecordInTrans(cmdText, sqlParams);
        }
        public JobInfoSheet JobInfoSheetView(JObject sqlParams = null)
        {
            string cmdText = ConfigurationManager.AppSettings["ContractJobSheetView"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            return getRecords<JobInfoSheet>(cmdText, sqlParams).FirstOrDefault();
        }
    }
}