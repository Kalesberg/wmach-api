using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models;
using Newtonsoft.Json.Linq;

namespace API.Data
{
    //this will provide all the necessary detail for the customer portal front end to render
    public class ContractDetail : IBuildParams<ContractDetail>
    {
        public ContractView Contract;
        private ContactRelationship ContactRelationship;
        public Contact CustomerCompany;
        public Contact CustomerContact;
        public Contact Salesman;
        public Contact RentalCoordinator;
        public Contact ServiceManager;
        public DivisionDetail DivisionDetail;
        public ContactAddress CustomerMainOfficeAddress;
        public ContactAddress CustomerBillingAddress;
        public ContactAddress JobsiteAddress;
        public List<ContractDetails> ContractDetails = new List<ContractDetails>();

        public void Build(JObject sqlParams)
        {
            if (sqlParams["contractID"] != null)
            {
                Contract = DAL.GetInstance().getContractByContractID(sqlParams);
            }
            else
                return;
            GetContactRelationship(Contract.CustomerContactID);
            GetAddress(Contract.CustomerMainOfficeAddressID, ref CustomerMainOfficeAddress);
            GetAddress(Contract.CustomerBillingAddressID, ref CustomerBillingAddress);
            GetAddress(Contract.JobSiteAddressID, ref JobsiteAddress);
            GetContactInfo(ContactRelationship.ParentContactID, ref CustomerCompany);
            GetContactInfo(ContactRelationship.ChildContactID, ref CustomerContact);
            GetContactInfo(Contract.SalesmanContactID, ref Salesman);
            GetContactInfo(Contract.RentalCoordinatorID, ref RentalCoordinator);
            GetContactInfo(Contract.ServiceManagerID, ref ServiceManager);
            GetDivisionDetailInfo(Contract.WWMDivisionID);
            GetContractDetail(Contract.ContractID);
            

            //get all equpiment and it's attachments
            foreach(ContractDetails cd in ContractDetails)
            {
                List<ContractDetailAttachment> contractdetailattachments = new List<ContractDetailAttachment>();
                Equipment equipment = new Equipment();
                GetEquipment(cd.EquipmentID, ref equipment);
                var json = new JObject { { "EquipmentID", equipment.EquipmentID }, { "Size", "small" } };
                equipment.Pictures = DAL.GetInstance().getPictureFileNames(json).ToList();   //get equiipment picture
                cd.Equipment = equipment;
                contractdetailattachments = GetContractDetailAttachment(cd.ContractDtlID);   // get atachemnt from contract detail attachment 
                var jsonContractDtlID = new JObject { { "ContractDtlID", cd.ContractDtlID } };
                cd.Services = DAL.GetInstance().getServiceByContractDtlID(jsonContractDtlID); //get equipment service for this contractdetail
                cd.Transportations = DAL.GetInstance().getShipmentInvetoryByContractDtlID(jsonContractDtlID);     //get transportation for this contract equipment
                
                cd.Attachments = new List<Equipment>();
                foreach (ContractDetailAttachment attachment in contractdetailattachments) //get all attachment detail and picture
                {
                    var json1 = new JObject { { "EquipmentID", attachment.AttachmentID } };
                    Equipment attach = DAL.GetInstance().getAttachmentByEquipmentID(json1);
                    var json2 = new JObject { { "EquipmentID", attachment.AttachmentID }, { "Size", "small" } };
                    attach.Pictures = DAL.GetInstance().getPictureFileNames(json2).ToList();
                    cd.Attachments.Add(attach);
                }
                //add models field
                cd.ModelDetail = DAL.GetInstance().getModelFieldsByEquipmentID(new JObject { { "EquipmentID", equipment.EquipmentID }});
            }
           
        }

        private void GetContactRelationship(int ContactRelationshipID)
        {
            var json = new JObject { { "ContactRelationshipID", ContactRelationshipID } };
            ContactRelationship = DAL.GetInstance().getContactRelationshipByContactRelationshipID(json);
        }
        private void GetContactInfo(int contactID, ref Contact contact)
        {
            var json = new JObject { { "ContactID", contactID } };
            contact = DAL.GetInstance().getContactByContactID(json);
        }
        private void GetDivisionDetailInfo(int DivisionID)
        {
            var json = new JObject { { "DivisionID", DivisionID } };
            DivisionDetail = DAL.GetInstance().getDivisionDetailByDivisionID(json);
        }
        private void GetAddress(int contactID, ref ContactAddress address)
        {
            var json = new JObject { { "AddressID", contactID } };
            address = DAL.GetInstance().getAddressByAddressID(json);
        }
        private void GetContractDetail(int ContractID)
        {
            var json = new JObject { { "ContractID", ContractID } };
            ContractDetails = DAL.GetInstance().getContractDetailByContractID(json);
        }

        private void GetEquipment(int EquipmentID, ref Equipment equipment)
        {
            var json = new JObject { { "EquipmentID", EquipmentID } };
            equipment = DAL.GetInstance().getEquipmentByEquipmentID(json);
        }

        private List<ContractDetailAttachment> GetContractDetailAttachment(int ContractDetailID)
        {
            var json = new JObject { { "ContractDtlID", ContractDetailID } };
            return DAL.GetInstance().getContractDetailAttachmentByContractDtlID(json);
        }

        public ContractDetail GetResult()
        {
            return this;
        }
    }
}