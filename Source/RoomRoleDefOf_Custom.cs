using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace PeteTimesSix.CategorizedCleaning
{
    [DefOf]
    public static class RoomRoleDefOf_Custom
    {
        public static RoomRoleDef Kitchen;
        public static RoomRoleDef Barn;

        static RoomRoleDefOf_Custom() 
        { 
            DefOfHelper.EnsureInitializedInCtor(typeof(RoomRoleDefOf_Custom));    
        }
    }
}
