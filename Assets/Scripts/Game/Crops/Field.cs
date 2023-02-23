using GG.Mathe;

namespace Rogue.Game.Crops
{
    public class Field
    {
        private Rect2i m_zone;

        private Soil[] m_soils;

        private Plant[] m_plants;

        public Rect2i Zone => m_zone;

        public int Count => m_zone.Area;

        public Field(Rect2i zone)
        {
            m_zone   = zone;
            m_soils  = new Soil [zone.Area];
            m_plants = new Plant[zone.Area];
            // Initialize the soil.
            for (int i = 0; i < m_soils.Length; i++)
            {
                m_soils[i] = new Soil();
            }
        }

        public void Update(int elapsed)
        {
            foreach (Plant plant in m_plants)
            {
                if (plant == null)
                {
                    continue;
                }

                plant.Update(elapsed);
            }
        }

        #region @@@ POSITION @@@

        public bool HasCoord(Vec2i coord) => m_zone.Contains(coord);

        public Vec2i GetCoord(int index) => IndexToCoord(index) + m_zone.Min;

        public bool Overlaps(Rect2i zone) => m_zone.Overlaps(zone);

        #endregion

        #region @@@ JOBS @@@

        public bool HasPendingJob(int index)
        {
            return GetSoil(index).job >= 0 || (GetPlant(index) != null && GetPlant(index).job >= 0);
        }

        public void ClearPendingJob(Vec2i coord)
        {
            ClearPendingJob(WorldCoordToIndex(coord));
        }

        public void ClearSoilJob(Vec2i coord)
        {
            GetSoil(WorldCoordToIndex(coord)).job = -1;
        }

        public void ClearPlantJob(Vec2i coord)
        {
            int index = WorldCoordToIndex(coord);

            if (GetPlant(index) != null)
            {
                GetPlant(index).job = -1;
            }
        }

        public void ClearPendingJob(int index)
        {
            GetSoil(index).job = -1;

            if (GetPlant(index) != null)
            {
                GetPlant(index).job = -1;
            }
        }

        #endregion

        #region @@@ SOIL @@@

        public bool HasEmptySoil()
        {
            for (int i = 0; i < m_plants.Length; i++)
            {
                if (m_plants[i] == null)
                {
                    return true;
                }
            }

            return false;
        }

        public bool TryGetEmptySoil(out Vec2i coord)
        {
            coord = Vec2i.Zero;

            for (int i = 0; i < m_plants.Length; i++)
            {
                if (m_plants[i] == null)
                {
                    coord = IndexToCoord(i) + m_zone.Min;
                    return true;
                }
            }

            return false;
        }

        public Soil GetSoil(Vec2i coord)
        {
            if (coord.x < m_zone.Min.x || coord.y < m_zone.Min.y) { return null; }
            if (coord.x > m_zone.Max.x || coord.y > m_zone.Max.y) { return null; }

            return GetSoil(WorldCoordToIndex(coord));
        }

        public Soil GetSoil(int index)
        {
            return m_soils[index];
        }

        public bool Level(Vec2i coord)
        {
            m_soils[WorldCoordToIndex(coord)].Level();

            return true;
        }

        public bool Level(int index)
        {
            m_soils[index].Level();

            return true;
        }

        public bool Plow(Vec2i coord)
        {
            Soil soil = GetSoil(coord);

            soil.plowed      = true;
            soil.temperature = 20;
            soil.water       = 0;
            
            return true;
        }

        #endregion

        #region @@@ PLANTS @@@@

        public bool Seed(Vec2i coord, PlantType plantType)
        {
            if (!GetSoil(coord).plowed)
            {
                return false;
            }

            if (GetPlant(coord) != null)
            {
                return false;
            }

            Plant plant = new()
            {   
                type        = plantType,
                growth      = 0,
                temperature = 0,
                water       = 0,
            };

            m_plants[WorldCoordToIndex(coord)] = plant;

            return true;
        }

        public bool Harvest(Vec2i coord)
        {
            Plant plant = GetPlant(coord);
            if (plant == null)
            {
                return false;
            }

            if (!plant.Harvest())
            {
                return false;
            }

            RemovePlant(coord);
            Level(coord);

            return true;
        }

        public Plant GetPlant(Vec2i coord)
        {
            if (coord.x < m_zone.Min.x || coord.y < m_zone.Min.y) { return null; }
            if (coord.x > m_zone.Max.x || coord.y > m_zone.Max.y) { return null; }

            return GetPlant(WorldCoordToIndex(coord));
        }

        public Plant GetPlant(int index)
        {
            return m_plants[index];
        }

        public void AddPlant(Vec2i coord, Plant plant)
        {
            m_plants[WorldCoordToIndex(coord)] = plant;
        }

        public void RemovePlant(Vec2i coord)
        {
            m_plants[WorldCoordToIndex(coord)] = null;
        }

        #endregion

        #region @@@ HELPERS @@@



        private int WorldCoordToIndex(Vec2i coord)
        {
            return CoordToIndex(coord - m_zone.Min);
        }

        private int CoordToIndex(Vec2i coord)
        {
            return coord.Row * m_zone.Width + coord.Col;
        }

        private Vec2i IndexToCoord(int index)
        {
            return Vec2i.FromRowCol(index / m_zone.Width, index % m_zone.Width);
        }

        #endregion
    }
}
