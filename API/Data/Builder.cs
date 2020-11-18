using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Data
{
    public static class Builder
    {
        public static T Build<T>(IBuild<T> builder)
        {
            builder.Build();
            return builder.GetResult();
        }

        public static T Build<T>(IBuildParams<T> builder, JObject sqlParams)
        {
            builder.Build(sqlParams);
            return builder.GetResult();
        }
    }

    public interface IBuild<T>
    {
        void Build();
        T GetResult();
    }

    public interface IBuildParams<T>
    {   
        void Build(JObject sqlParams);
        T GetResult();
    }
}