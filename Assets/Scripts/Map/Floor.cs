using Rogue.Map.Data;

namespace Rogue.Map
{
    public class Floor
    {
        public int id = -1;

        public DataBiome biomeData = null;

        public DataFloor floorData = null;

        public string biomeName = string.Empty;

        public string floorName = string.Empty;

        public string Title => floorData.title;

        public string Description => floorData.description;

        public Floor() {}

        public Floor(DataBiome biome, DataFloor floor)
        {
            Load(biome, floor);
        }

        public void Load(DataBiome biome, DataFloor floor)
        {
            biomeData   = biome;
            biomeName   = biome.name;
            floorData   = floor;
            floorName   = floor.name;
        }
    }
}
