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
        [HarmonyPrefix]
        public static void Room_Notify_RoomShapeChanged_Prefix(Room __instance)
        {
            var cache = __instance.Map.GetComponent<FilthCache>();
            var filths = __instance.ContainedThings<Filth>().ToList();
            foreach (Filth filth in filths)
            {
                cache.RemoveFilth(filth);
                cache.AddFilth(filth);
            }
        }
    }

    
    [HarmonyPatch(typeof(Room), "UpdateRoomStatsAndRole")]
    public static class Room_UpdateRoomStatsAndRole_Patches
    {
        public static FieldRef<Room, RoomRoleDef> field_roleInt;

        public class State
        {
            public RoomRoleDef rolePre;
            public List<Filth> filthsPre = new();
        }

        static Room_UpdateRoomStatsAndRole_Patches() 
        {
            field_roleInt = FieldRefAccess<Room, RoomRoleDef>("role");
        }

        [HarmonyPrefix]
        public static void Room_UpdateRoomStatsAndRole_Prefix(Room __instance, out State __state)
        {
            __state = new State() { rolePre = field_roleInt(__instance) };
            __state.filthsPre.AddRange(__instance.ContainedThings<Filth>());
        }

        [HarmonyPostfix]
        public static void Room_UpdateRoomStatsAndRole_Postfix(Room __instance, State __state)
        {
            if (__state.rolePre != field_roleInt(__instance))
            {
                var cache = __instance.Map.GetComponent<FilthCache>();
                foreach (Filth filth in __state.filthsPre)
                {
                    cache.RemoveFilth(filth);
                    cache.AddFilth(filth);
                }
            }
        }
    }
}
