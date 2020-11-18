using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Data;

namespace API.Data
{
    public class YardListBuilder : IBuild<List<Yard>>
    {
        private List<Yard> _yards = null;
        public void Build()
        {
            BuildYard();
        }

        public List<Yard> GetResult()
        {
            return _yards;
        }

        private void BuildYard()
        {
            var db = DAL.GetInstance();
            _yards = db.getYardNames();
        }
    }
}