using HarmonyLib;
using RimWorld;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
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
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Room_UpdateRoomStatsAndRole_Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator ilGenerator)
        {
            var toMatch = new CodeMatch(OpCodes.Stfld, Field(typeof(Room), "role"));
            var storageLocal = ilGenerator.DeclareLocal(typeof(RoomRoleDef));

            var prefix = new CodeInstruction[]
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldfld, Field(typeof(Room), "role")),
                new CodeInstruction(OpCodes.Stloc, storageLocal.LocalIndex),
            };
            //split so that by the time we call PostChangeCompare role is already set for calls from other functions
            var postfix = new CodeInstruction[]
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldfld, Field(typeof(Room), "role")),
                new CodeInstruction(OpCodes.Ldloc, storageLocal.LocalIndex),
                new CodeInstruction(OpCodes.Call, Method(typeof(Room_UpdateRoomStatsAndRole_Patches), nameof(PostChangeCompare))),
            };


            foreach (CodeInstruction instruction in instructions)
            {
                bool isWriteToRoleField = toMatch.opcode == instruction.opcode && toMatch.operand == instruction.operand;
                if (isWriteToRoleField)
                {
                    foreach (var prefixInstruction in prefix)
                    {
                        yield return prefixInstruction;
                    }
                }

                yield return instruction;

                if (isWriteToRoleField)
                {
                    foreach (var prefixInstruction in postfix)
                    {
                        yield return prefixInstruction;
                    }
                }
            }
        }

        public static void PostChangeCompare(Room room, RoomRoleDef first, RoomRoleDef second)
        {
            //Log.Message($"room {room?.ToString() ?? "NULL"} type comparing {first?.defName ?? "NULL"} to {second?.defName ?? "NULL"}");
            if (first != second)
            {
                var filths = room.ContainedThings<Filth>();
                var cache = room.Map.GetComponent<FilthCache>();
                foreach (Filth filth in filths)
                {
                    cache.RemoveFilth(filth);
                    cache.AddFilth(filth);
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
