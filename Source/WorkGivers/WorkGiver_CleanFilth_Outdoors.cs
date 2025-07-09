using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace PeteTimesSix.CategorizedCleaning
{
    public class WorkGiver_CleanFilth_Outdoors : WorkGiver_CleanFilth
    {
        //public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForGroup(ThingRequestGroup.Undefined);

        public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {
            return pawn.Map.GetComponent<FilthCache>().filthOutdoors;
            //return StaticFilthCache.GetMapFilthCache(pawn.Map).FilthOutdoors;
        }

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            return base.HasJobOnThing(pawn, t, forced) && t.IsOutsideOrInBarn();
        }
    }
}
