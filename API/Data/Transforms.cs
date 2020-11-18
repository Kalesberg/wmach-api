using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace API.Data
{
    //CONVERTS DATATABLE TO TO LIST<T>
    //USES CACHED PROPERTIES FROM API.MODELS BELOW
    //REFLECTION CACHE ONLY POPULATED ON INIT TO PREVENT REPEATED REFLECTION CALLS
    public static class Transforms
    {
        public static List<T> ConvertToList<T>(this DataTable data)
        {
            var newObjs = New<List<T>>.Instance();
            var hasConstructor = !(typeof(T).IsPrimitive || typeof(T) == typeof(String) || typeof(T) == typeof(Decimal));
            var fieldNames = data.Columns.Cast<DataColumn>().Select(x => x.ColumnName.ToLower()).ToList<string>();
            var props = ReflectionCache.GetInstance().Where(type => type.Key == typeof(T).Name.ToString())
                                                     .Select(p => p.Value)
                                                     .FirstOrDefault();

            foreach (DataRow row in data.Rows)
            {
                T obj = default(T);
                if (hasConstructor)
                {
                    obj = New<T>.Instance();
                    foreach (var prop in props)
                    {
                        if (fieldNames.Contains(prop.Name.ToLower()))
                        {
                            var val = row[prop.Name] == DBNull.Value ? null: row[prop.Name];
                            var type = data.Columns[prop.Name].DataType;
                            val = val == null ? DefaultCache.GetValue(type) : val;
                            prop.SetValue(obj, val);
                        }
                    }
                }
                else
                {
                    obj = (T)(row[0] == DBNull.Value ? null : row[0]);
                }

                newObjs.Add(obj);
            }

            return newObjs;
        }
    }

    //SINGLETON. CACHES ALL MODEL TYPE PROPERTIES FOR DATATABLE TRANSFORMS
    public static class ReflectionCache
    {
        private static Dictionary<string, PropertyInfo[]> _reflectionCache = null;

        public static Dictionary<string, PropertyInfo[]> GetInstance()
        {
            if (_reflectionCache != null) return _reflectionCache;
            else
            {
                _reflectionCache = InitializeCache();
                return _reflectionCache;
            }
        }

        private static Dictionary<string, PropertyInfo[]> InitializeCache()
        {
            Dictionary<string, PropertyInfo[]> reflectionCache = new Dictionary<string, PropertyInfo[]>();
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass && t.Namespace == "API.Models");
            foreach(var t in types)
            {
                reflectionCache.Add(t.Name, t.GetProperties());
            }

            return reflectionCache;
        }
    }

    public static class DefaultCache
    {
        private static Dictionary<Type, Object> _defaultCache = null;

        public static object GetValue(Type type)
        {
            try
            {
                if (_defaultCache == null) _defaultCache = InitializeCache();
                if (_defaultCache.ContainsKey(type))
                    return _defaultCache[type];
                else return null;
            }
            catch(Exception e)
            {
               return null;
            }
        }

        private static Dictionary<Type, Object> InitializeCache()
        {
            Dictionary<Type, Object> defaultCache = new Dictionary<Type, Object>();

            defaultCache.Add(typeof(string), String.Empty);
            defaultCache.Add(typeof(decimal), Decimal.Zero);
            defaultCache.Add(typeof(int), 0);
            
            return defaultCache;
        }
    }

    //USES COMPILED LAMBDAS FOR VERY EFFICIENT CREATION OF GENERIC OBJECTS. USED INSTEAD OF ACTIVATOR.CREATEINSTANCE
    public static class New<T>
    {
        public static readonly Func<T> Instance = Expression.Lambda<Func<T>>
                                                  (
                                                   Expression.New(typeof(T))
                                                  ).Compile();
    }
}