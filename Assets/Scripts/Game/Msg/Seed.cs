using Rogue.Coe;
using GG.Mathe;

namespace Rogue.Game.Msg
{
    public class Seed : GameMessage<Seed>
    {
        public Vec2i where;

        public int job = -1;

        public bool done = false;

        public Seed() {}

        public Seed(Vec2i where)
        {
            this.where = where;
        }
    }
}