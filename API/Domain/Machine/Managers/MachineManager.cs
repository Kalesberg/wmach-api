using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Utilities;
using System.IO;
using API.Data;
using Newtonsoft.Json.Linq;

namespace API.Managers
{
    public static class MachineManager
    {
        public static int CreateEquipmentPhotos(JObject tokens, byte[] data)
        {
            try
            {
                //INSERT DB RECORD AND RETURN ID
                var db = DAL.GetInstance();
                var photoID = db.createEquipmentPicture(tokens);

                //SCALE IMAGES AND WRITE TO FS
                Photos.Save(data, photoID.ToString(), "EquipmentPhotoDirectory");
                return photoID;
            }
            catch(Exception e)
            {
                throw;
            }
        }
    }
}