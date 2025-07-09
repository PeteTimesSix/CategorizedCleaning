using RimWorld;
using System.Collections.Generic;
using Verse;

namespace PeteTimesSix.CategorizedCleaning
{
    public class MapFilthCache
    {
        public Map map;

        public long lastCachedTick = -1;
        public List<Thing> filthOutdoors = new List<Thing>();
        public List<Thing> filthIndoors = new List<Thing>();
        public List<Thing> filthInSterileRooms = new List<Thing>();

        public List<Thing> FilthOutdoors { get { RecacheIfNeeded(); return filthOutdoors; } }
        public List<Thing> FilthIndoors { get { RecacheIfNeeded(); return filthIndoors; } }
        public List<Thing> FilthInSterileRooms { get { RecacheIfNeeded(); return filthInSterileRooms; } }

        public MapFilthCache(Map map)
        {
            this.map = map;
        }

        public void RecacheIfNeeded() 
        {
            var tick = Find.TickManager.TicksGame;
            if (tick != lastCachedTick)
            {
                filthOutdoors.Clear();
                filthIndoors.Clear();
                filthIndoors.Clear();

                foreach (var filth in map.listerFilthInHomeArea.FilthInHomeArea)
                {
                    if (filth.IsOutsideOrInBarn())
                        filthOutdoors.Add(filth);
                    else if (filth.IsInSterileRoom())
                        filthInSterileRooms.Add(filth);
                    else
                        filthIndoors.Add(filth);
                }
                lastCachedTick = tick;
            }
        }
    }
}

/*using RimWorld;
using System;
using System.Collections;
using System.Collections.Generic;
using Verse;

namespace PeteTimesSix.CategorizedCleaning
{
    public class MapFilthCache
    {
        public Map map;

        public long lastCachedTick = -1;
        public StreamedList<Thing> filthOutdoors;
        public StreamedList<Thing> filthIndoors;
        public StreamedList<Thing> filthInSterileRooms;

        public int baseListIndex = 0;

        public IEnumerable<Thing> FilthOutdoors { get { ClearCacheIfNeeded(); return filthOutdoors; } }
        public IEnumerable<Thing> FilthIndoors { get { ClearCacheIfNeeded(); return filthIndoors; } }
        public IEnumerable<Thing> FilthInSterileRooms { get { ClearCacheIfNeeded(); return filthInSterileRooms; } }

        public MapFilthCache(Map map)
        {
            this.map = map;
            filthOutdoors = new(this);
            filthIndoors = new(this);
            filthInSterileRooms = new(this);
        }

        public void ClearCacheIfNeeded() 
        {
            var tick = Find.TickManager.TicksGame;
            if (tick != lastCachedTick)
            {
                Log.Message("clearing cache...");
                filthOutdoors.matches.Clear();
                filthIndoors.matches.Clear();
                filthIndoors.matches.Clear();
                baseListIndex = 0;
                lastCachedTick = tick;
            }
        }

        private int inc = 0;
        public bool CacheNext()
        {
            Log.Message("cacheNext " + inc);
            inc++;
            if (baseListIndex >= map.listerFilthInHomeArea.FilthInHomeArea.Count)
                return false;

            var filthAt = map.listerFilthInHomeArea.FilthInHomeArea[baseListIndex];

            if (filthAt.IsOutsideOrInBarn())
                filthOutdoors.matches.Add(filthAt);
            else if (filthAt.IsInSterileRoom())
                filthInSterileRooms.matches.Add(filthAt);
            else
                filthIndoors.matches.Add(filthAt);
             
            Log.Message("cached " + baseListIndex + " of " + map.listerFilthInHomeArea.FilthInHomeArea.Count);

            baseListIndex++;

            return true;
        }
    }

    public class StreamedList<T> : IEnumerable<T>
    {
        public MapFilthCache parent;
        public List<T> matches = new List<T>();

        public StreamedList(MapFilthCache parent) 
        {
            this.parent = parent;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new StreamedListEnumerator<T>(this, parent);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class StreamedListEnumerator<T> : IEnumerator<T>
    {
        public MapFilthCache parent;
        public StreamedList<T> streamedList;
        public int curIndex = 0;

        public StreamedListEnumerator(StreamedList<T> streamedList, MapFilthCache parent)
        {
            this.streamedList = streamedList;
            this.parent = parent;
        }

        public T Current => streamedList.matches[curIndex];

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            curIndex++;
            if (curIndex >= streamedList.matches.Count)
            {
                do
                {
                    if (!parent.CacheNext())
                        return false;
                } while (curIndex >= streamedList.matches.Count);
            }
            return true;
        }

        public void Reset()
        {
            curIndex = 0;
        }
    }
}*/