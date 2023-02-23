using Rogue.Coe;

namespace Rogue.Game.Comp
{
    public class Crop : GameComponent<Crop>
    {
        public int cropId = -1;

        public Crop() {}

        public Crop(int cropId)
        {
            this.cropId = cropId;
        }
    }
}
