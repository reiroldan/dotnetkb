using System;
using System.Collections.Generic;
using System.Linq;
using DotNetKillboard.Data.Queries;

namespace DotNetKillboard.Data
{
    public static class FilterHelper
    {        
        public static Dictionary<Type,Type> GetFilterTypes() {
            var queryType = typeof(MongoQueryBase);
            var queries = queryType.Assembly.GetTypes().Where(x => !x.IsAbstract && !x.IsInterface && queryType.IsAssignableFrom(x));
            return queries.Select(x => new { Type = x, Interface = x.GetInterfaces()[1] }).ToDictionary(x => x.Interface, x => x.Type);
        }
    }
}