using LudeonTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace PeteTimesSix.CategorizedCleaning
{
    public static class CategorizedCleaning_Debug
    {
        public static bool drawFilthCategoryOverlay = false;
    }

    [StaticConstructorOnStartup]
    public static partial class DebugMenuEntries
    {
        private const string CATEGORY = "Categorized Cleaning";

        [DebugAction(category = CATEGORY, actionType = DebugActionType.Action)]
        static void ToggleFilthCategorizationDrawing()
        {
            CategorizedCleaning_Debug.drawFilthCategoryOverlay = !CategorizedCleaning_Debug.drawFilthCategoryOverlay;
            Log.Message($"Toggled drawing of Filth categorization {(CategorizedCleaning_Debug.drawFilthCategoryOverlay ? "on" : "off")}");
        }
    }
}
