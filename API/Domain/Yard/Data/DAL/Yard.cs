using API.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace API.Data
{
    public partial class DAL
    {

        public List<Yard> getYardNames()
        {
            string cmdText = ConfigurationManager.AppSettings["YardNames"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            Func<DataTable, List<Yard>> transform = yardTransform;
            return getRecords<Yard>(cmdText, transform, null);
        }

        private List<Yard> yardTransform(DataTable data)
        {
            var yards = new List<Yard>();
            foreach (var row in data.AsEnumerable())
            {
                var yardLocation = row[0].ToString();
                var subYard = row[1].ToString();
                var yardExists = yards.Exists(y => y.YardName == yardLocation);

                if (yardExists)
                {
                    var yardToUpdate = yards.Find(y => y.YardName == yardLocation);
                    yardToUpdate.SubYards.Add(subYard);
                }
                else
                {
                    var yard = new Yard();
                    var subYards = new List<string>();

                    //ONLY ADD TO SUBYARD ID IT IS DIFFERENT FROM PARENTS
                    if ((yardLocation != subYard) && (!String.IsNullOrWhiteSpace(yardLocation))) subYards.Add(subYard);

                    //SET YARDNAME SAME AS SUBYARD IF YARDNAME IS EMPTY
                    if (String.IsNullOrWhiteSpace(yardLocation)) yard.YardName = subYard;
                    else yard.YardName = yardLocation;

                    yard.SubYards = subYards;
                    yards.Add(yard);
                }
            }
            return yards;
        }
    }
}