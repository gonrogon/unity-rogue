using Rogue.Coe;
using Rogue.Core;

namespace Rogue.Game.Msg
{
    /// <summary>
    /// Define a message to notify an entity (usually a weapon) to hit something.
    /// </summary>
    public class Hit : GameMessage<Hit>
    {
        /// <summary>
        /// Who initiated the hit.
        /// </summary>
        public Ident source;

        /// <summary>
        /// What to hit.
        /// </summary>
        public Ident target;

        /// <summary>
        /// Flag indicating whether it was done or not.
        /// </summary>
        public bool done = false;

        /// <summary>
        /// Constructor.
        /// </summary>
        public Hit() {}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="source">Who initiated the hit.</param>
        /// <param name="target">What to hit.</param>
        public Hit(Ident source, Ident target)
        {
            this.source = source;
            this.target = target;
        }
    }
}