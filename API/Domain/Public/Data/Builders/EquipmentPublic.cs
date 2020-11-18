using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models;
using Newtonsoft.Json.Linq;

namespace API.Data
{
    public class EquipmentPub : IBuildParams<IEnumerable<EquipmentPublic>>
    {
        private IEnumerable<EquipmentPublic> _equipment;
        public void Build(JObject sqlParams)
        {
            _equipment = DAL.GetInstance().getEquipmentPublic(sqlParams, "machine");
            BuildAttachment();
            GetPictureFileNames();
        }

        public IEnumerable<EquipmentPublic> GetResult()
        {
            return _equipment;
        }

        private void BuildAttachment()
        {
            DAL.GetInstance().getAttachments(_equipment);
        }

        private void GetPictureFileNames()
        {
            DAL.GetInstance().getPictureFileNames(_equipment);
        }
    }

    public class EquipmentPubWebsite : IBuild<IEnumerable<EquipmentPublic>>
    {
        private IEnumerable<EquipmentPublic> _equipment;
        public void Build()
        {
            _equipment = DAL.GetInstance().getEquipmentPublic();
            BuildAttachment();
            GetPictureFileNames();
        }

        public IEnumerable<EquipmentPublic> GetResult()
        {
            return _equipment;
        }

        private void BuildAttachment()
        {
            DAL.GetInstance().getAttachments(_equipment);
        }

        private void GetPictureFileNames()
        {
            DAL.GetInstance().getPictureFileNames(_equipment);
        }
    }
}