using Rogue.Coe;
using GG.Mathe;

namespace Rogue.Game.Msg
{
    public class Plow : GameMessage<Plow>
    {
        public Vec2i where;

        public int job = -1;

        public bool done = false;

        public Plow() {}

        public Plow(Vec2i where)
        {
            this.where = where;
        }
    }
}