using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.Noise;
using static HarmonyLib.AccessTools;

namespace PeteTimesSix.CategorizedCleaning.HarmonyPatches
{
    public static class ListerFilthInHomeArea_MapAccess
    {
        public static FieldRef<ListerFilthInHomeArea, Map> field_map; 
        static ListerFilthInHomeArea_MapAccess() 
        {
            field_map = FieldRefAccess<ListerFilthInHomeArea, Map>("map");
        }

        public static Map GetMap(this ListerFilthInHomeArea lister) => field_map(lister);
    }

    [HarmonyPatch(typeof(ListerFilthInHomeArea), nameof(ListerFilthInHomeArea.RebuildAll))]
    public static class ListerFilthInHomeArea_RebuildAll_Patches
    {
        [HarmonyPostfix]
        public static void ListerFilthInHomeArea_RebuildAll_Postfix(ListerFilthInHomeArea __instance)
        {
            var map = __instance.GetMap();
            map.GetComponent<FilthCache>().RebuildAll();
        }
    }

    [HarmonyPatch(typeof(ListerFilthInHomeArea), nameof(ListerFilthInHomeArea.Notify_FilthSpawned))]
    public static class ListerFilthInHomeArea_Notify_FilthSpawned_Patches
    {
        [HarmonyPostfix]
        public static void ListerFilthInHomeArea_Notify_FilthSpawned_Postfix(ListerFilthInHomeArea __instance, Filth f) 
        {
            var map = __instance.GetMap();
            map.GetComponent<FilthCache>().Notify_FilthSpawned(f);
        }
    }

    [HarmonyPatch(typeof(ListerFilthInHomeArea), nameof(ListerFilthInHomeArea.Notify_FilthDespawned))]
    public static class ListerFilthInHomeArea_Notify_FilthDespawned_Patches
    {
        [HarmonyPostfix]
        public static void ListerFilthInHomeArea_Notify_FilthDespawned_Postfix(ListerFilthInHomeArea __instance, Filth f)
        {
            var map = __instance.GetMap();
            map.GetComponent<FilthCache>().Notify_FilthDespawned(f);
        }
    }

    [HarmonyPatch(typeof(ListerFilthInHomeArea), nameof(ListerFilthInHomeArea.Notify_HomeAreaChanged))]
    public static class ListerFilthInHomeArea_Notify_HomeAreaChanged_Patches
    {
        [HarmonyPostfix]
        public static void ListerFilthInHomeArea_Notify_HomeAreaChanged_Postfix(ListerFilthInHomeArea __instance, IntVec3 c)
        {
            var map = __instance.GetMap();
            map.GetComponent<FilthCache>().Notify_HomeAreaChanged(c);

        }
    }
}
