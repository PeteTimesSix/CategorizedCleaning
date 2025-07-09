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
            var roomRole = t.GetRoom()?.Role;
            if (roomRole == null)
                return false;
            return 
                roomRole == RoomRoleDefOf.Hospital ||
                roomRole == RoomRoleDefOf.Laboratory ||
                roomRole == RoomRoleDefOf_Custom.Kitchen;
        }

        public static bool IsOutsideOrInBarn(this Thing t)
        {
            return t.IsOutside() || t.GetRoom()?.Role == RoomRoleDefOf_Custom.Barn;
        }
    }
}
