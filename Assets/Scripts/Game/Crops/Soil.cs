using GG.Mathe;

namespace Rogue.Game.Crops
{
    public class Soil
    {
        public Vec2i coord;

        public bool plowed = false;

        public int water = 0;

        public int temperature = 0;

        public int job = -1;

        public void Level()
        {
            plowed = false;
        }
    }
}
