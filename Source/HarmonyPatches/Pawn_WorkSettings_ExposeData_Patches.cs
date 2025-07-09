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
    [HarmonyPatch(typeof(Pawn_WorkSettings), nameof(Pawn_WorkSettings.ExposeData))]
    public static class Pawn_WorkSettings_ExposeData_Patches
    {
        public static FieldRef<Pawn_WorkSettings, Pawn> field_pawn;
        static Pawn_WorkSettings_ExposeData_Patches()
        {
            field_pawn = FieldRefAccess<Pawn_WorkSettings, Pawn>("pawn");
        }

        [HarmonyPostfix]
        public static void Pawn_WorkSettings_ExposeData_Postfix(Pawn_WorkSettings __instance)
        {
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                var pawn = field_pawn(__instance);
                if (pawn.IsColonyMech)
                    __instance.EnableAndInitialize();
            }
        }
    }
}
