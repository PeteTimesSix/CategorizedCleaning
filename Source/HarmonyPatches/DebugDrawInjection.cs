using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace PeteTimesSix.CategorizedCleaning.HarmonyPatches
{

    [HarmonyPatch(typeof(DoorsDebugDrawer), nameof(DoorsDebugDrawer.DrawDebug))]
    public static class DebugDrawInjection
    {
        [HarmonyPostfix]
        public static void AdditionalDebugDrawingInjection()
        {
            DrawOnMap();
        }

        public static void DrawOnMap()
        {
            if (!CategorizedCleaning_Debug.drawFilthCategoryOverlay)
            {
                return;
            }

            var map = Find.CurrentMap;
            if (map == null)
                return;

            var filthCache = map.GetComponent<FilthCache>();

            CellRect currentViewRect = Find.CameraDriver.CurrentViewRect;
            List<Thing> allFilth = Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.Filth);
            for (int i = 0; i < allFilth.Count; i++)
            {
                var filth = allFilth[i] as Filth;
                if (filth == null || !filth.Spawned)
                    continue;
                if (!currentViewRect.Contains(filth.Position))
                    continue;

                if(filthCache.filthOutdoors.Contains(filth))
                    CellRenderer.RenderCell(filth.Position, SolidColorMaterials.SimpleSolidColorMaterial(new Color(1f, 0f, 0f, 0.25f)));
                if (filthCache.filthIndoors.Contains(filth))
                    CellRenderer.RenderCell(filth.Position, SolidColorMaterials.SimpleSolidColorMaterial(new Color(0f, 1f, 0f, 0.25f)));
                if (filthCache.filthInSterileRooms.Contains(filth))
                    CellRenderer.RenderCell(filth.Position, SolidColorMaterials.SimpleSolidColorMaterial(new Color(0f, 0f, 1f, 0.25f)));
            }
        }
    }
}
