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

        public void Notify_FilthSpawned(Filth f)
        {
            if (map.areaManager.Home[f.Position])
            {
                AddFilth(f);
            }
        }

        public void Notify_FilthDespawned(Filth f)
        {
            RemoveFilth(f);
        }

        public void Notify_HomeAreaChanged(IntVec3 c)
        {
            if (map.areaManager.Home[c])
            {
                List<Thing> thingList = c.GetThingList(map);
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
                for (int index = filthOutdoors.Count - 1; index >= 0; index--)
                {
                    if (filthOutdoors[index].Position == c)
                        filthOutdoors.RemoveAt(index);
                }
                for (int index = filthIndoors.Count - 1; index >= 0; index--)
                {
                    if (filthIndoors[index].Position == c)
                        filthIndoors.RemoveAt(index);
                }
                for (int index = filthInSterileRooms.Count - 1; index >= 0; index--)
                {
                    if (filthInSterileRooms[index].Position == c)
                        filthInSterileRooms.RemoveAt(index);
                }
            }
        }
    }
}
