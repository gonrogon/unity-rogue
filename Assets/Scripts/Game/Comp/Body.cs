using Rogue.Coe;
using Rogue.Core;

namespace Rogue.Game.Comp
{
    public class Body : GameComponent<Body>
    {
        /// <summary>
        /// Body identifier.
        /// </summary>
        public Ident bid;

        public Body() {}

        public Body(Ident bid)
        {
            this.bid = bid;
        }
    }
}
