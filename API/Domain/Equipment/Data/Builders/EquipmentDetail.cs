using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models;
using Newtonsoft.Json.Linq;

namespace API.Data
{
    public class EquipmentDetail : IBuildParams<Equipment>
    {
        public Equipment _equipment;
        public void Build(JObject sqlParams)
        {
            _equipment = DAL.GetInstance().getEquipmentBySerial(sqlParams);
            GetAttachment();
            GetPictureFileNames();
            GetContract();
            GetTransport();
            GetCheckInOut();
        }

        public Equipment GetResult()
        {
            return _equipment;
        }

        private void GetAttachment()
        {
            var json = new JObject { { "EquipmentID", _equipment.EquipmentID } };
            _equipment.Attachments = DAL.GetInstance().getEquipmentAttachments(json).ToList();
        }

        private void GetPictureFileNames()
        {
            var json = new JObject { { "EquipmentID", _equipment.EquipmentID }, { "Size", "small" } };
            _equipment.Pictures = DAL.GetInstance().getPictureFileNames(json).ToList();
        }

        private void GetContract()
        {
            var json = new JObject { { "EquipmentID", _equipment.EquipmentID } };
            _equipment.ContractDetails = DAL.GetInstance().getContract(json);
        }

        private void GetTransport()
        {
            var json = new JObject { { "EquipmentID", _equipment.EquipmentID } };
            _equipment.Transportation = DAL.GetInstance().getTransport(json);
        }

        private void GetCheckInOut()
        {
            var json = new JObject { { "EquipmentID", _equipment.EquipmentID } };
            var checkinout = DAL.GetInstance().getCheckInOut(json);
            if(checkinout.Count() > 0)
            {
                _equipment.LastCheckInDate = checkinout.SingleOrDefault(c => c.Type == "CheckIn") != null? checkinout.SingleOrDefault(c => c.Type == "CheckIn").InspectionDate : null;
                _equipment.LastCheckOutDate = checkinout.SingleOrDefault(c => c.Type == "CheckOut") != null ? checkinout.SingleOrDefault(c => c.Type == "CheckOut").InspectionDate : null;
            }
           
        }
    }
}