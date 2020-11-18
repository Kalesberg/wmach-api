using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models;
using Newtonsoft.Json.Linq;

namespace API.Data
{
    public class AttachmentDetail : IBuildParams<Equipment>
    {
        public Equipment _equipment;
        public void Build(JObject sqlParams)
        {
            _equipment = DAL.GetInstance().getAttachmentBySerial(sqlParams);
            GetPictureFileNames();
            GetContract();
            GetTransport();
        }

        public Equipment GetResult()
        {
            return _equipment;
        }


        private void GetPictureFileNames()
        {
            var json = new JObject { { "EquipmentID", _equipment.EquipmentID }, { "Size", "small" } };
            _equipment.Pictures = DAL.GetInstance().getPictureFileNames(json).ToList();
        }

        private void GetContract()
        {
            var json = new JObject { { "EquipmentID", _equipment.EquipmentID }, { "EquipmentType", "Attachment" } };
            _equipment.ContractDetails = DAL.GetInstance().getContract(json);
        }

        private void GetTransport()
        {
            var json = new JObject { { "EquipmentID", _equipment.EquipmentID } , { "EquipmentType", "Attachment" } };
            _equipment.Transportation = DAL.GetInstance().getTransport(json);
        }
    }
}