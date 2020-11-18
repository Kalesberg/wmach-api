using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models;
using Newtonsoft.Json.Linq;

namespace API.Data
{
    public class AttachmentSimple : IBuildParams<IEnumerable<Equipment>>
    {
        private IEnumerable<Equipment> _equipment;
        public void Build(JObject sqlParams)
        {
            _equipment = DAL.GetInstance().getEquipment(sqlParams, "attachment");
            GetPictureFileNames();
            GetContract();
        }

        public IEnumerable<Equipment> GetResult()
        {
            return _equipment;
        }

        private void GetPictureFileNames()
        {
            DAL.GetInstance().getPictureFileNames(_equipment);
        }

        private void GetContract()
        {
            foreach (var equipment in _equipment)
            {
                var json = new JObject { { "EquipmentID", equipment.EquipmentID }, { "EquipmentType", "Attachment" } };
                equipment.ContractDetails = DAL.GetInstance().getContract(json);
            }

        }
    }
}