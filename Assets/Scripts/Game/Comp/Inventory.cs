using Rogue.Coe;
using Rogue.Core;

namespace Rogue.Game.Comp
{
    public class Inventory : GameComponent<Inventory>
    {
        /// <summary>
        /// Inventory identifier.
        /// </summary>
        public Ident iid;

        public Inventory() {}

        public Inventory(Ident iid)
        {
            this.iid = iid;
        }
    }
}
