using Rogue.Core;
using GG.Mathe;

namespace Rogue.Game.Msg
{
    /// <summary>
    /// Define a message to perform a plow action.
    /// </summary>
    public class ActionPlow : Action<ActionPlow>
    {
        /// <summary>
        /// Crop to plow.
        /// </summary>
        public Ident target;

        /// <summary>
        /// Coordinate of the tile to plow.
        /// </summary>
        public Vec2i where = Vec2i.Zero;

        //public int job = -1;

        //public bool done = false;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ActionPlow() {}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="target">Target entity to plow.</param>
        /// <param name="where">Where to plow the soil.</param>
        public ActionPlow(Ident target, Vec2i where)
        {
            this.target = target;
            this.where  = where;
        }
    }
}