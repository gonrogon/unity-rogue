using Rogue.Coe;
using GG.Mathe;

namespace Rogue.Game.Comp
{
    public class Location : GameComponent<Location>
    {
        public Vec2i position = Vec2i.Zero;

        public Location() {}

        public Location(Vec2i position)
        {
            this.position = position;
        }
    }
}
