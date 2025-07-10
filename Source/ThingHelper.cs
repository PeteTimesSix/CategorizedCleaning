using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace PeteTimesSix.CategorizedCleaning
{
    public static class ThingHelper
    {
        public static bool IsInSterileRoom(this Thing t)
        {
            return CategorizedCleaning_Settings.sterileRooms.Contains(t.GetRoom()?.Role ?? RoomRoleDefOf.None);
        }

        public static bool IsOutsideOrInBarn(this Thing t)
        {
            return t.IsOutside() || CategorizedCleaning_Settings.outdoorRooms.Contains(t.GetRoom()?.Role ?? RoomRoleDefOf.None); //t.GetRoom()?.Role == RoomRoleDefOf_Custom.Barn;
        }
    }
}
