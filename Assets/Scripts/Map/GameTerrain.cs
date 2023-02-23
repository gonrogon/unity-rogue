using System;
using System.Collections.Generic;

namespace Rogue.Map
{
    public class GameTerrain
    {
        private List<Floor> m_floors;

        private List<Wall> m_walls;

        private Dictionary<string, Biome> m_biomes;

        public GameTerrain()
        {
            m_floors = new List<Floor>();
            m_walls  = new List<Wall>();
            m_biomes = new Dictionary<string, Biome>();
        }

        public Floor GetDefaultFloor()
        {
            return m_floors[0];
        }

        public Floor GetFloor(int id)
        {
            return m_floors[id];
        }

        public Floor GetFloor(string biome, string name)
        {
            if (string.IsNullOrEmpty(biome) || string.IsNullOrEmpty(name))
            {
                return null;
            }

            if (GetOrCreateBiome(biome).TryGetFloor(name, out int id))
            {
                return m_floors[id];
            }

            return null;
        }

        public bool TryGetFloor(string biome, string name, out Floor floor)
        {
            floor = null;
            Biome thisBiome = GetBiome(biome);

            if (thisBiome != null && thisBiome.TryGetFloor(name, out int id))
            {
                floor = m_floors[id];
            }

            return floor != null;
        }

        public void AddFloor(Floor floor)
        {
            m_floors.Add(floor);
        }

        public void AddFloor(Data.DataBiome biome, string name)
        {
            Data.DataFloor item = biome.FindFloor(name);
            if (item == null)
            {
                return;
            }

            AddFloor(biome, item);
        }

        private void AddFloor(Data.DataBiome biome, Data.DataFloor floor)
        {
            GetOrCreateBiome(biome.name).AddFloor(floor.name, InsertFloor(new Floor(biome, floor)));
        }

        private int InsertFloor(Floor floor)
        {
            m_floors.Add(floor);
            
            return (floor.id = m_floors.Count - 1);
        }

        public Wall GetDefaultWall()
        {
            return m_walls[0];
        }

        public Wall GetWall(int id)
        {
            return m_walls[id];
        }

        public Wall GetWall(string biome, string name)
        {
            if (string.IsNullOrEmpty(biome) || string.IsNullOrEmpty(name))
            {
                return null;
            }

            if (GetOrCreateBiome(biome).TryGetWall(name, out int id))
            {
                return m_walls[id];
            }

            return null;
        }
        public bool TryGetWall(string biome, string name, out Wall wall)
        {
            wall = null;
            Biome thisBiome = GetBiome(biome);

            if (thisBiome != null && thisBiome.TryGetWall(name, out int id))
            {
                wall = m_walls[id];
            }

            return wall != null;
        }

        public void AddWall(Wall wall)
        {
            m_walls.Add(wall);
        }

        public void AddWall(Data.DataBiome biome, string name)
        {
            Data.DataWall item = biome.FindWall(name);
            if (item == null)
            {
                return;
            }

            AddWall(biome, item);
        }

        private void AddWall(Data.DataBiome biome, Data.DataWall wall)
        {
            GetOrCreateBiome(biome.name).AddWall(wall.name, InsertWall(new Wall(biome, wall)));
        }

        private int InsertWall(Wall wall)
        {
            m_walls.Add(wall);

            return (wall.id = m_walls.Count - 1);
        }

        public void Load(Data.DataBiome biome)
        {
            foreach (Data.DataFloor item in biome.floors)
            {
                AddFloor(biome, item);
            }

            foreach (Data.DataWall item in biome.walls)
            {
                AddWall(biome, item);
            }

            if (biome.defaultFloor != null)
            {
                GetBiome(biome.name).SetDefaultFloor(biome.defaultFloor.name);
            }

            if (biome.defaultWall != null)
            {
                GetBiome(biome.name).SetDefaultFloor(biome.defaultWall.name);
            }
        }

        public void Sync(IEnumerable<Data.DataBiome> biomes)
        {
            foreach (var biome in biomes)
            {
                Sync(biome);
            }
        }

        public void Sync(Data.DataBiome biome)
        {
            foreach (Floor floor in m_floors)
            {
                if (floor.biomeName != biome.name)
                {
                    continue;
                }

                floor.biomeData = biome;
                floor.floorData = biome.FindFloor(floor.floorName);
            }

            foreach (Wall wall in m_walls)
            {
                if (wall.biomeName != biome.name)
                {
                    continue;
                }

                wall.biomeData = biome;
                wall.wallData  = biome.FindWall(wall.wallName);
            }
        }

        // ------
        // BIOMES
        // ------

        private Biome GetBiome(string name)
        {
            if (m_biomes.TryGetValue(name, out Biome biome))
            {
                return biome;
            }

            return null;
        }

        private Biome GetOrCreateBiome(string name)
        {
            if (m_biomes.TryGetValue(name, out Biome biome))
            {
                return biome;
            }

            return m_biomes[name] = new Biome();
        }
    }
}
