using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models;
using Newtonsoft.Json.Linq;

namespace API.Data
{
    public class EquipmentDetailCustomer : IBuildParams<EquipmentDetailCustomerPortal>
    {
        private EquipmentDetailCustomerPortal _equipmentdetail;
        public void Build(JObject sqlParams)
        {
            _equipmentdetail = DAL.GetInstance().GetEquipmentDetailOnCustomerContract(sqlParams);
            //get accountmanger
            Contact AccountManager = new Contact();
            Contact RentalCoordinator = new Contact();
            Contact ServiceManager = new Contact();
            int serviceid;
            GetContactInfo(_equipmentdetail.AccountManagerID, ref AccountManager);
            GetContactInfo(_equipmentdetail.RentalCoordinatorID, ref RentalCoordinator);
            if(int.TryParse( _equipmentdetail.ServiceManagerID ,out serviceid))
            {
                GetContactInfo(serviceid, ref ServiceManager);
            }
            
            AccountManager.ContactId = 0;
            RentalCoordinator.ContactId = 0;
            ServiceManager.ContactId = 0;
            _equipmentdetail.RentalCoordinator = RentalCoordinator;
            _equipmentdetail.AccountManager = AccountManager;
            _equipmentdetail.ServiceManager = ServiceManager;
            //get jobsite
            ContactAddress Jobsite = new ContactAddress();
            GetAddress(_equipmentdetail.JobsiteID, ref Jobsite);
            _equipmentdetail.Jobsite = Jobsite;
            GetPictureFileNames();
            _equipmentdetail.Attachments = GetContractDetailAttachmentName(_equipmentdetail.ContractDtlID);
        }

        public EquipmentDetailCustomerPortal GetResult()
        {
            return _equipmentdetail;
        }
        private void GetContactInfo(int contactID, ref Contact contact)
        {
            var json = new JObject { { "ContactID", contactID } };
            contact = DAL.GetInstance().getContactByContactID(json);
        }
        
        private void GetAddress(int contactID, ref ContactAddress address)
        {
            var json = new JObject { { "AddressID", contactID } };
            address = DAL.GetInstance().getAddressByAddressID(json);
        }

        private void GetPictureFileNames()
        {
            var json = new JObject { { "EquipmentID", _equipmentdetail.EquipmentID }, { "Size", "Large" } };
            _equipmentdetail.Pictures = DAL.GetInstance().getPictureFileNames(json).ToList();
        }

        private List<AttachmentNameAndPosition> GetContractDetailAttachmentName(int ContractDetailID)
        {
            var json = new JObject { { "ContractDtlID", ContractDetailID } };
            return DAL.GetInstance().getContractDetailAttachmentNameByContractDtlID(json);
        }

    }
}