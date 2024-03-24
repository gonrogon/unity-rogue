using System.Collections.Generic;
using System.Diagnostics;
using Rogue.Core.Collections;
using Rogue.Core;
using GG.Mathe;

namespace Rogue.Map
{
    public class GameMap
    {
        public delegate void OnEntityAddedHandler(GameMap map, Ident entity, Vec2i where);

        public delegate void OnEntityRemovedHandler(GameMap map, Ident entity, Vec2i where);

        /// <summary>
        /// Event invoked when a new entity is added to the map.
        /// </summary>
        public event OnEntityAddedHandler OnEntityAdded;

        /// <summary>
        /// Event invoked when a entity is removed from the map.
        /// </summary>
        public event OnEntityRemovedHandler OnEntityRemoved;

        public int Rows { get; private set; } = 0;

        public int Cols { get; private set; } = 0;

        public int Width => Cols;

        public int Height => Rows;

        //private IdentBagMap m_bags = new();

        private BagMap<Ident, GameMapItem> m_items = new();

        //private List<IdentValuePair<Ident, > m_multicellItems = new();
        private Bag<Ident, IMulticell> m_multicells = new();

        private Grid<Cell> m_cells;

        private Grid<int> m_objs;

        private GameTerrain m_terrain;

        private System.Random m_random;

        private int m_nextZoneId = 0;

        public GameMap(int width, int height)
            :
            this(width, height, System.Environment.TickCount)
        {}

        public GameMap(int width, int height, int seed)
        {
            m_random  = new System.Random(seed);
            m_terrain = new GameTerrain();

            Rows = height;
            Cols = width;

            m_cells = new Grid<Cell>(Rows, Cols); // Grid for cells.
            m_objs  = new Grid<int> (Rows, Cols); // Grid for objects.
            // Initialize the grid.
            for (int i = 0; i < m_cells.Rows; i++)
            {
                for (int j = 0; j < m_cells.Cols; j++)
                {
                    m_cells.GetRef(Vec2i.FromRowCol(i, j)).Clear();
                }
            }
            // Initialize objects.
            for (int i = 0; i < m_objs.Rows; i++)
            {
                for (int j = 0; j < m_objs.Cols; j++)
                {
                    m_objs.Set(new Vec2i(j, i), -1);
                }
            }
        }

        // -----------
        // COORDINATES
        // -----------

        public bool HasCoord(Vec2i coord) => m_cells.HasCoord(coord);

        // --------
        // TERRAINS
        // --------

        public void LoadTerrainBiome(Data.DataBiome biome)
        {
            m_terrain.Load(biome);
        }

        public void SyncTerrain(IEnumerable<Data.DataBiome> biomes)
        {
            m_terrain.Sync(biomes);
        }

        public void SyncTerrain(Data.DataBiome biome)
        {
            m_terrain.Sync(biome);
        }

        // -----
        // CELLS
        // -----

        public Cell GetCell(Vec2i coord)
        {
            return m_cells.Get(coord);
        }

        public void SetCell(Vec2i coord, string biome, string floor, string wall)
        {
            SetCell(coord, m_terrain.GetFloor(biome, floor), m_terrain.GetWall(biome, wall));
        }

        public void SetCell(IEnumerable<Vec2i> coords, string biome, string floor, string wall)
        {
            SetCell(coords, m_terrain.GetFloor(biome, floor), m_terrain.GetWall(biome, wall));
        }

        private void SetCell(IEnumerable<Vec2i> coords, Floor floor, Wall wall)
        {
            foreach (Vec2i coord in coords)
            {
                SetCell(coord, floor, wall);
            }
        }

        private void SetCell(Vec2i coord, Floor floor, Wall wall)
        {
            m_cells.GetRef(coord).Reset(floor, wall, m_random);
        }

        public bool HasFloor(Vec2i coord)
        {
            return m_cells.GetRef(coord).floor >= 0;
        }

        public Floor GetFloor(Vec2i coord)
        {
            int id = m_cells.GetRef(coord).floor;
            if (id < 0)
            {
                return null;
            }

            return m_terrain.GetFloor(id);
        }

        public int GetFloorTileIndex(Vec2i coord)
        {
            return GetCell(coord).floorIndex;
        }

        public int GetWallTileIndex(Vec2i coord)
        {
            return GetCell(coord).wallIndex;
        }

        public Data.DataFloor GetFloorData(Vec2i coord)
        {
            return GetFloor(coord)?.floorData;
        }

        public void SetFloor(Vec2i coord, string biome, string floor)
        {
            SetFloor(coord, m_terrain.GetFloor(biome, floor));
        }

        private void SetFloor(IEnumerable<Vec2i> coords, Floor floor)
        {
            foreach (Vec2i coord in coords)
            {
                SetFloor(coord, floor);
            }
        }

        public void SetFloor(IEnumerable<Vec2i> coords, string biome, string floor)
        {
            SetFloor(coords, m_terrain.GetFloor(biome, floor));
        }

        private void SetFloor(Vec2i coord, Floor floor)
        {
            m_cells.GetRef(coord).SetFloor(floor, m_random);
        }

        public bool HasWall(Vec2i coord)
        {
            return m_cells.GetRef(coord).wall >= 0;
        }

        public Wall GetWall(Vec2i coord)
        {
            int id = m_cells.GetRef(coord).wall;
            if (id < 0)
            {
                return null;
            }

            return m_terrain.GetWall(id);
        }

        public Data.DataWall GetWallData(Vec2i coord)
        {
            return GetWall(coord)?.wallData;
        }

        public void SetWall(IEnumerable<Vec2i> coords, string biome, string wall)
        {
            SetWall(coords, m_terrain.GetWall(biome, wall));
        }

        public void SetWall(Vec2i coord, string biome, string wall)
        {
            SetWall(coord, m_terrain.GetWall(biome, wall));
        }

        private void SetWall(IEnumerable<Vec2i> coords, Wall wall)
        {
            foreach (Vec2i coord in coords)
            {
                SetWall(coord, wall);
            }
        }

        private void SetWall(Vec2i coord, Wall wall)
        {
            m_cells.GetRef(coord).SetWall(wall, m_random);
        }

        public bool IsSolid(Vec2i coord)
        {
            // Map boundaries are considered solid.
            if (coord.x < 0 || coord.x >= Width)  { return true; }
            if (coord.y < 0 || coord.y >= Height) { return true; }

            return GetCell(coord).IsSolid;
        }

        // -----
        // ZONES
        // -----

        public int CreateZone() => m_nextZoneId++;

        public bool IsZone(Vec2i coord) => m_cells.GetRef(coord).zone >= 0;

        public bool IsZone(Vec2i coord, int zone) => zone >= 0 && m_cells.GetRef(coord).zone == zone;

        public int GetZone(Vec2i coord) => m_cells.GetRef(coord).zone;

        public void SetZone(Vec2i coord, int zone) => m_cells.GetRef(coord).zone = zone;

        public void SetZone(IEnumerable<Vec2i> coords, int zone)
        {
            foreach (var coord in coords)
            {
                m_cells.GetRef(coord).zone = zone;
            }
        }

        public void ClearZone(Vec2i coord) => SetZone(coord, -1);

        public void ClearZone(IEnumerable<Vec2i> coords) => SetZone(coords, -1);

        // --------
        // ENTITIES
        // --------

        /// <summary>
        /// Checks if there is at least one entity in a coordinate.
        /// </summary>
        /// <param name="coord">Coordinate.</param>
        /// <returns>True if there is a least one entity; otherwise, false.</returns>
        public bool Empty(Vec2i coord)
        {
            return Count(coord) == 0;
        }

        /// <summary>
        /// Gets the number of entities in a coordinate.
        /// </summary>
        /// <param name="coord">Coordinate.</param>
        /// <returns>Number of entities in the cell.</returns>
        public int Count(Vec2i coord) => CountRegular(coord) + CountMulticell(coord);
        
        private int CountRegular(Vec2i coord)
        {
            int bag = GetBag(coord);
            if (bag < 0)
            {
                return 0;
            }

            return m_items.Count(bag);
        }

        private int CountMulticell(Vec2i coord)
        {
            int count = 0;

            for (int i = 0; i < m_multicells.Count; i++)
            {
                var pair = m_multicells.At(i);

                if (pair.Value.ContainsCoord(pair.Key, coord))
                {
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Tries to get the first entity in a coordinate.
        /// </summary>
        /// <param name="coord">Coordinate.</param>
        /// <param name="eid">Entitiy id.</param>
        /// <returns>True if the entity exists; otherwise, false.</returns>
        public bool TryGetFirst(Vec2i coord, out Ident eid) => TryGetNth(coord, 0, out eid);

        /// <summary>
        /// Tries to get the nth entity in a coordinate.
        /// </summary>
        /// <param name="coord">Coordinate.</param>
        /// <param name="nth">Number.</param>
        /// <param name="eid">Entity id.</param>
        /// <returns>True if the entity exists; otherwise, false.</returns>
        public bool TryGetNth(Vec2i coord, int nth, out Ident eid)
        {
            if (TryGetRegularNth(coord, nth, out eid))
            {
                return true;
            }

            if (TryGetMulticellNth(coord, 0, nth - CountRegular(coord), out eid))
            {
                return true;
            }

            return false;
        }

        private bool TryGetRegularNth(Vec2i coord, int nth, out Ident eid)
        {
            int bag = GetBag(coord);
                eid = Ident.Zero;

            if (bag < 0)
            {
                return false;
            }

            if (!m_items.TryGetNth(bag, nth, out var pair))
            {
                return false;
            }

            eid = pair.Key;
            return true;
        }

        private bool TryGetMulticellNth(Vec2i coord, int cur, int nth, out Ident eid)
        {
                eid   = Ident.Zero;
            int count = 0;

            for (int i = cur; i < m_multicells.Count; i++)
            {
                var pair = m_multicells.At(i);

                if (pair.Value.ContainsCoord(pair.Key, coord))
                {
                    if (count == nth)
                    {
                        eid = pair.Key;
                        return true;
                    }

                    count++;
                }
            }

            return false;
        }
        
        /// <summary>
        /// Tries to find the first entity in a coordinate that matches the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="coord">Coordinate.</param>
        /// <param name="pred">Predicate.</param>
        /// <param name="eid">Entity id.</param>
        /// <returns>True if the entity exists; otherwise, false.</returns>
        public bool TryFindFirst(Vec2i coord, System.Func<Ident, bool> pred, out Ident eid)
        {


            /*
            int bag = GetBag(coord);
                eid = Ident.Zero;

            if (bag >= 0)
            {
                //for (int i = 0; m_bags.TryGetNth(bag, i, out Ident idOut); i++)
                for (int i = 0; m_items.TryGetNth(bag, i, out var pair); i++)
                {
                    //if (pred.Invoke(idOut))
                    if (pred.Invoke(pair.ident))
                    {
                        //eid = idOut;
                        eid = pair.ident;
                        return true;
                    }
                }
            }

            return false;
            */

            eid = Ident.Zero;

            if (!HasCoord(coord))
            {
                return false;
            }

            for (int i = 0; TryGetNth(coord, i, out Ident idOut); i++)
            {
                if (pred.Invoke(idOut))
                {
                    eid = idOut;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Tries to find the first entity in a list of coordinates that matches the conditions defined by the specified
        /// predicate.
        /// </summary>
        /// <param name="coords">List of coordinates.</param>
        /// <param name="pred">Predicate.</param>
        /// <param name="eid">Entity id.</param>
        /// <returns>True if the entity exists; otherwise, false.</returns>
        public bool TryFindFirst(IEnumerable<Vec2i> coords, System.Func<Ident, bool> pred, out Ident eid)
        {
            eid = Ident.Zero;

            foreach (var coord in coords)
            {
                if (TryFindFirst(coord, pred, out eid))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Performs the specified action on each entitiy in a coordinate.
        /// </summary>
        /// <param name="coord">Coordinate.</param>
        /// <param name="action">Action.</param>
        public void ForEach(Vec2i coord, System.Action<Ident> action)
        {
            /*
            int bag = GetBag(coord);
            if (bag < 0)
            {
                return;
            }

            //m_bags.ForEach(bag, action);
            m_items.ForEach(bag, (ident, item) => 
            {
                action(ident);
            });
            */
            ForEachRegular  (coord, action);
            ForEachMulticell(coord, action);
        }

        /// <summary>
        /// Performs the specified action on each entitiy in a list of coordinates.
        /// </summary>
        /// <param name="coords">List of coordinates.</param>
        /// <param name="pred">Predicate.</param>
        public void ForEach(IEnumerable<Vec2i> coords, System.Action<Ident> pred)
        {
            foreach (var coord in coords)
            {
                ForEach(coord, pred);
            }
        }

        private void ForEachRegular(Vec2i coord, System.Action<Ident> action)
        {
            int bag = GetBag(coord);
            if (bag < 0)
            {
                return;
            }

            //m_bags.ForEach(bag, action);
            m_items.ForEach(bag, (ident, item) => 
            {
                action(ident);
            });
        }

        private void ForEachMulticell(Vec2i coord, System.Action<Ident> action)
        {
            for (int i = 0; i < m_multicells.Count; i++)
            {
                var pair = m_multicells.At(i);

                if (pair.Value.ContainsCoord(pair.Key, coord))
                {
                    action(pair.Key);
                }
            }
        }

        /// <summary>
        /// Adds an entity to a coordiante.
        /// </summary>
        /// <param name="coord">Coordiante.</param>
        /// <param name="eid">Entity id.</param>
        public void Add(Vec2i coord, Ident eid) => ImplAdd(coord, eid, true);

        /// <summary>
        /// Adds a entity that encompasses multiple cells.
        /// </summary>
        /// <param name="eid">Entity id.</param>
        /// <param name="multicell">Multicell implementation.</param>
        public void AddMulticell(Ident eid, IMulticell multicell) => ImplAddMulticell(eid, multicell);

        /// <summary>
        /// Removes an entity from a coordinate.
        /// </summary>
        /// <param name="coord">Coordinate.</param>
        /// <param name="eid">Entity id.</param>
        public void Remove(Vec2i coord, Ident eid) => ImplRemove(coord, eid, true);

        /// <summary>
        /// Removes all entities in a coordiante.
        /// </summary>
        /// <param name="coord">Coordinate.</param>
        public void RemoveAll(Vec2i coord)
        {
            int bag = GetBag(coord);
            if (bag < 0)
            {
                return;
            }
            // Unlink the bag from the cell.
            SetBag(coord, -1);
            // Notify and clear the bag.
            //m_bags.ForEach  (bag, eid => { onEntityRemoved?.Invoke(this, eid, coord); });
            //m_bags.RemoveAll(bag);
            //m_bags.Release  (bag);
            m_items.ForEach  (bag, (eid, item) => { OnEntityRemoved?.Invoke(this, eid, coord); });
            m_items.RemoveAll(bag);
            m_items.Release  (bag);
        }

        public void RemoveMulticell(Ident eid) => ImplRemoveMulticell(eid, true);

        /// <summary>
        /// Move an entity from a coordinate to another.
        /// </summary>
        /// <param name="origin">Origin</param>
        /// <param name="target"></param>
        /// <param name="eid">Entity id.</param>
        public void Move(Vec2i origin, Vec2i target, Ident eid)
        {
            ImplRemove(origin, eid, false);
            ImplAdd   (target, eid, false);
        }

        /// <summary>
        /// Move all entities from a coordinate to another.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="target"></param>
        public void MoveAll(Vec2i origin, Vec2i target)
        {
            if (Empty(origin))
            {
                return;
            }

            int originBag = GetBag(origin);
            int targetBag = GetBag(target);

            //m_bags.MoveAll(originBag, targetBag);
            m_items.MoveAll(originBag, targetBag);
            ReleaseBag(origin);
        }

        private void ImplAdd(Vec2i coord, Ident eid, bool notify)
        {
            int bag = GetBag(coord);
            if (bag < 0)
            {
                //bag = SetBag(coord, m_bags.Create());
                bag = SetBag(coord, m_items.Create());
            }

            //m_bags.Add(bag, eid);
            m_items.Add(bag, eid, GameMapItem.CreateMovable());

            if (notify)
            {
                OnEntityAdded?.Invoke(this, eid, coord);
            }
        }

        private void ImplAddMulticell(Ident eid, IMulticell item)
        {
            m_multicells.Add(eid, item);
        }

        private void ImplRemove(Vec2i coord, Ident eid, bool notify)
        {
            int bag = GetBag(coord);
            if (bag < 0)
            {
                return;
            }

            //if (m_bags.Remove(bag, eid) > 0)
            if (m_items.Remove(bag, eid) > 0)
            {
                //if (m_bags.Empty(bag))
                if (m_items.Empty(bag))
                {
                    ReleaseBag(coord);
                }

                if (notify)
                {
                    OnEntityRemoved?.Invoke(this, eid, coord);
                }
            }
        }

        private void ImplRemoveMulticell(Ident eid, bool notify)
        {
            int index = m_multicells.FindFirst(eid);
            if (index < 0)
            {
                return;
            }

            Vec2i origin = m_multicells.At(index).Value.GetOrigin();
                           m_multicells.Remove(index);

            if (notify)
            {
                OnEntityRemoved?.Invoke(this, eid, origin);
            }
        }

        // ----
        // BAGS
        // ----

        /// <summary>
        /// Get the bag in a coordinate.
        /// </summary>
        /// <param name="coord">Coordinate.</param>
        /// <returns>Bag.</returns>
        private int GetBag(Vec2i coord)
        {
            return m_objs.Get(coord);
        }

        /// <summary>
        /// Set the bag in a coordinate.
        /// </summary>
        /// <param name="coord">Coordinate.</param>
        /// <param name="bag">Bag to set.</param>
        /// <returns></returns>
        private int SetBag(Vec2i coord, int bag)
        {
            Debug.Assert(Empty(coord), "Set a bag in a non-empty coordinate");

            m_objs.Set(coord, bag);

            return bag;
        }

        /// <summary>
        /// Releases a bag.
        /// </summary>
        /// <param name="coord">Coordinate.</param>
        private void ReleaseBag(Vec2i coord)
        {
            Debug.Assert(Empty(coord), "Release a non-empty bag");

            int bag = GetBag(coord);
                      SetBag(coord, -1);

            //m_bags.Release(bag);
            m_items.Release(bag);
        }

        /// <summary>
        /// Swap two bags.
        /// </summary>
        /// <param name="a">Coordinate.</param>
        /// <param name="b">Coordinate.</param>
        private void SwapBags(Vec2i a, Vec2i b)
        {
            int bagA = GetBag(a);
            int bagB = GetBag(b);

            SetBag(a, bagB);
            SetBag(b, bagA);
        }
    }
}
