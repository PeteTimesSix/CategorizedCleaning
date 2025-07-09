using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace PeteTimesSix.CategorizedCleaning
{
    public static class StaticFilthCache
    {
        public static Dictionary<Map, MapFilthCache> mapCaches = new();

        public static MapFilthCache GetMapFilthCache(Map map)
        {
            var found = mapCaches.TryGetValue(map, out MapFilthCache cache);
            if(found)
                return cache;
            else
            {
                var newCache = new MapFilthCache(map);
                mapCaches.Add(map, newCache);
                return newCache;
            }
        }
    }
}
