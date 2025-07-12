using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace PeteTimesSix.CategorizedCleaning
{
    public class FilthCache : MapComponent
    {
        public List<Filth> filthOutdoors = new List<Filth>();
        public List<Filth> filthIndoors = new List<Filth>();
        public List<Filth> filthInSterileRooms = new List<Filth>();

        public FilthCache(Map map) : base(map)
        {
        }

        public void UpdateFilth(Filth filth)
        {
            if (!map.areaManager.Home[filth.Position])
            {
                RemoveFilth(filth);
                return;
            }
            else
            {
                if (filth.IsInSterileRoom())
                {
                    if(!filthInSterileRooms.Contains(filth))
                    {
                        bool removed =
                            filthIndoors.Remove(filth);
                        if (!removed)
                            removed = filthOutdoors.Remove(filth);
                        filthInSterileRooms.Add(filth);
                    }
                }
                else if (!filth.IsOutsideOrInBarn())
                {
                    if(!filthIndoors.Contains(filth))
                    {
                        bool removed =
                            filthInSterileRooms.Remove(filth);
                        if (!removed)
                            removed = filthOutdoors.Remove(filth);
                        filthIndoors.Add(filth);
                    }
                }
                else
                {
                    if(!filthOutdoors.Contains(filth))
                    {
                        bool removed =
                            filthInSterileRooms.Remove(filth);
                        if (!removed)
                            removed = filthIndoors.Remove(filth);
                        filthOutdoors.Add(filth);
                    }
                }
            }
        }

        public bool RemoveFilth(Filth toRemove)
        {
            bool removed = 
                filthOutdoors.Remove(toRemove);
            if(!removed)
                removed = filthIndoors.Remove(toRemove);
            if (!removed)
                removed = filthInSterileRooms.Remove(toRemove);
            return removed;
        }

        public void AddFilth(Filth filth)
        {
            if (!map.areaManager.Home[filth.Position])
                return;

            if (filth.IsOutsideOrInBarn())
                filthOutdoors.Add(filth);
            else if (filth.IsInSterileRoom())
                filthInSterileRooms.Add(filth);
            else
                filthIndoors.Add(filth);
        }

        public void RebuildAll()
        {
            filthOutdoors.Clear();
            filthIndoors.Clear();
            filthInSterileRooms.Clear();

            foreach (IntVec3 allCell in map.AllCells)
            {
                Notify_HomeAreaChanged(allCell);
            }
        }

        public void Notify_HomeAreaChanged(IntVec3 c)
        {
            List<Thing> thingList = c.GetThingList(map);
            if (map.areaManager.Home[c])
            {
                for (int i = 0; i < thingList.Count; i++)
                {
                    if (thingList[i] is Filth filth)
                    {
                        AddFilth(filth);
                    }
                }
            }
            else
            {
                for (int i = 0; i < thingList.Count; i++)
                {
                    if (thingList[i] is Filth filth)
                    {
                        RemoveFilth(filth);
                    }
                }
            }
        }
    }
}
