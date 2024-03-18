using Rogue.Coe;
using Rogue.Core;

namespace Rogue.Game.Comp
{
    public class Body : GameComponent<Body>
    {
        /// <summary>
        /// Name of the template to create the body.
        /// </summary>
        public string template;

        /// <summary>
        /// Body identifier.
        /// </summary>
        public Ident bid;

        public Body() {}

        public Body(string template, Ident bid)
        {
            this.template = template;
            this.bid      = bid;
        }

        public Body(Ident bid)
        {
            this.bid = bid;
        }
    }
}
