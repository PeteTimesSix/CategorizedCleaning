using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace PeteTimesSix.CategorizedCleaning
{
    public class CategorizedCleaning_Mod : Mod
    {
        public static CategorizedCleaning_Mod ModSingleton { get; private set; }
        public static CategorizedCleaning_Settings Settings { get; internal set; }

        public static Harmony Harmony { get; internal set; }

        public CategorizedCleaning_Mod(ModContentPack content) : base(content)
        {
            ModSingleton = this;

            Harmony = new Harmony("PeteTimesSix.CategorizedCleaning");
            Harmony.PatchAll();
        }

        /*public override string SettingsCategory()
        {
            return "CategorizedCleaning_ModTitle".Translate();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Settings.DoSettingsWindowContents(inRect);
        }*/
    }


    [StaticConstructorOnStartup]
    public static class CategorizedCleaning_PostInit
    {
        static CategorizedCleaning_PostInit()
        {
            CategorizedCleaning_Mod.Settings = CategorizedCleaning_Mod.ModSingleton.GetSettings<CategorizedCleaning_Settings>();
        }
    }

}
