using Rogue.Map.Data;

namespace Rogue.Map
{
    public class Wall
    {
        public int id = -1;

        public DataBiome biomeData = null;

        public DataWall wallData = null;

        public string biomeName = string.Empty;

        public string wallName = string.Empty;

        public string Title => wallData.title;

        public string Description => wallData.description;

        public bool Solid => wallData.solid;

        public int Durability => wallData.durability;

        public Wall() {}

        public Wall(DataBiome biome, DataWall wall)
        {
            Load(biome, wall);
        }

        public void Load(DataBiome biome, DataWall wall)
        {
            biomeData = biome;
            biomeName = biome.name;
            wallData  = wall;
            wallName  = wall.name;
        }
    }
}
