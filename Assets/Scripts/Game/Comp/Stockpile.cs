using Rogue.Coe;
using Rogue.Core;

namespace Rogue.Game.Comp
{
    public class Stockpile : GameComponent<Stockpile>
    {
        public int id;

        public Stockpile() {}

        public Stockpile(int id)
        {
            this.id = id;
        }
    }
}
