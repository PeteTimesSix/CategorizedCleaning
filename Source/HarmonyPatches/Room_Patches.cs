using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
                cache.UpdateFilth(filth);
            }
        }
    }

    
    [HarmonyPatch(typeof(Room), "UpdateRoomStatsAndRole")]
    public static class Room_UpdateRoomStatsAndRole_Patches
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Room_UpdateRoomStatsAndRole_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var toMatch = new CodeMatch(OpCodes.Stfld, Field(typeof(Room), "role"));

            var prefix = new CodeInstruction[]
            {
                new CodeInstruction(OpCodes.Dup),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldfld, Field(typeof(Room), "role")),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, Method(typeof(Room_UpdateRoomStatsAndRole_Patches), nameof(PreChangeCompare))),
            };


            foreach (CodeInstruction instruction in instructions)
            {
                if(toMatch.opcode == instruction.opcode && toMatch.operand == instruction.operand)
                {
                    foreach (var prefixInstruction in prefix)
                    {
                        yield return prefixInstruction;
                    }
                }
                yield return instruction;
            }
        }

        public static void PreChangeCompare(RoomRoleDef first, RoomRoleDef second, Room room)
        {
            //Log.Message("comparing " + first.defName + " to " + second.defName);
            if(first != second)
            {
                var filths = room.ContainedThings<Filth>();
                var cache = room.Map.GetComponent<FilthCache>();
                foreach (Filth filth in filths)
                {
                    cache.UpdateFilth(filth);
                }
            }
        }

        /*public static FieldRef<Room, RoomRoleDef> field_roleInt;

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
            Log.Message("call coming from");
        }

        [HarmonyPostfix]
        public static void Room_UpdateRoomStatsAndRole_Postfix(Room __instance, State __state)
        {
            if (__state.rolePre != field_roleInt(__instance))
            {
                var cache = __instance.Map.GetComponent<FilthCache>();
                foreach (Filth filth in __state.filthsPre)
                {
                    cache.UpdateFilth(filth);
                }
            }
        }*/
    }
}
