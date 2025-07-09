using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using static HarmonyLib.AccessTools;

namespace PeteTimesSix.CategorizedCleaning.HarmonyPatches
{
    [HarmonyPatch(typeof(Room), nameof(Room.Notify_RoomShapeChanged))]
    public static class Room_Notify_RoomShapeChanged_Patches
    {
        public static List<Filth> filthsPre = new();

        [HarmonyPrefix]
        public static void Room_Notify_RoomShapeChanged_Prefix(Room __instance)
        {
            filthsPre.AddRange(__instance.ContainedThings<Filth>());
        }

        [HarmonyPostfix]
        public static void Room_Notify_RoomShapeChanged_Postfix(Room __instance) 
        {
            var cache = __instance.Map.GetComponent<FilthCache>();
            foreach (Filth filth in filthsPre)
            {
                cache.RemoveFilth(filth);
                cache.AddFilth(filth);
            }
            filthsPre.Clear();
        }
    }

    
    [HarmonyPatch(typeof(Room), "UpdateRoomStatsAndRole")]
    public static class Room_UpdateRoomStatsAndRole_Patches
    {
        public static FieldRef<Room, RoomRoleDef> field_roleInt;

        public static RoomRoleDef rolePre;
        public static List<Filth> filthsPre = new();

        static Room_UpdateRoomStatsAndRole_Patches() 
        {
            field_roleInt = FieldRefAccess<Room, RoomRoleDef>("role");
        }

        [HarmonyPrefix]
        public static void Room_UpdateRoomStatsAndRole_Prefix(Room __instance)
        {
            rolePre = field_roleInt(__instance);
            filthsPre.AddRange(__instance.ContainedThings<Filth>());
        }

        [HarmonyPostfix]
        public static void Room_UpdateRoomStatsAndRole_Postfix(Room __instance)
        {
            if (rolePre != field_roleInt(__instance))
            {
                var cache = __instance.Map.GetComponent<FilthCache>();
                foreach (Filth filth in filthsPre)
                {
                    cache.RemoveFilth(filth);
                    cache.AddFilth(filth);
                }
            }
            rolePre = null;
            filthsPre.Clear();
        }
    }
}
