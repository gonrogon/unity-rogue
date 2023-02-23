using Rogue.Coe;
using Rogue.Core;
using GG.Mathe;

namespace Rogue.Game.Msg
{
    /// <summary>
    /// Define a message to notify an entity to drop something.
    /// </summary>
    public class Drop : GameMessage<Drop>
    {
        /// <summary>
        /// What to drop.
        /// </summary>
        public Ident what;

        /// <summary>
        /// Where to drop it.
        /// </summary>
        public Vec2i where;

        /// <summary>
        /// Flag indicating whether it was done or not.
        /// </summary>
        public bool done = false;

        /// <summary>
        /// Constructor.
        /// </summary>
        public Drop() {}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="what">What to drop.</param>
        /// <param name="where">Where to drop it.</param>
        public Drop(Ident what, Vec2i where)
        {
            this.what  = what;
            this.where = where;
        }
    }
}