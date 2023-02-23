using GG.Mathe;

namespace Rogue.Game.Crops
{
    public class Plant
    {
        public PlantType type = null;

        public int water = 0;

        public int temperature = 0;

        public int growth = 0;

        public int growthElapsed = 0;

        public int job = -1;

        public bool HarvestReady => growth >= 100;

        public void Update(int elapsed)
        {
            growthElapsed += elapsed;

            if (growthElapsed >= type.GetGrowthInterval())
            {
                growthElapsed = 0;
                Grow(1);
            }
        }

        public void Grow(int amount)
        {
            growth += amount;

            if (growth > 100)
            {
                growth = 100;
            }
        }

        public bool Harvest()
        {
            if (!HarvestReady)
            {
                return false;
            }

            growth        = 0;
            growthElapsed = 0;

            return true;
        }
    }
}
