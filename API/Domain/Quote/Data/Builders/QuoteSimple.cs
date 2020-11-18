using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models;
using Newtonsoft.Json.Linq;

namespace API.Data
{
    public class QuoteSimple : IBuildParams<IEnumerable<QuoteSearchResults>>
    {
        private IEnumerable<QuoteSearchResults> _Quote;
        public void Build(JObject sqlParams)
        {
            _Quote = DAL.GetInstance().QuoteSearch(sqlParams);

        }

        public IEnumerable<QuoteSearchResults> GetResult()
        {
            return _Quote;
        }
    }
}