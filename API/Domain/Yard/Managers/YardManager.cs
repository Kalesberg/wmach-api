using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Data;
using API.Models;

namespace API.Managers
{
    public static class YardManager
    {
        public static List<Yard> GetYards()
        {
            var db = DAL.GetInstance();
            return db.getYardNames();
        }
    }
}