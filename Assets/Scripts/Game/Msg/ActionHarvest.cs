using Rogue.Core;
using GG.Mathe;

namespace Rogue.Game.Msg
{
    /// <summary>
    /// Define a message to perform a plow action.
    /// </summary>
    public class ActionHarvest : Action<ActionHarvest>
    {
        /// <summary>
        /// Crop to plow.
        /// </summary>
        public Ident target;

        /// <summary>
        /// Coordinate of the tile to harvest.
        /// </summary>
        public Vec2i where = Vec2i.Zero;

        /// <summary>
        /// Flag indicating whether the action was finalized or not.
        /// </summary>
        //public bool done = false;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ActionHarvest() {}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="target">Target entity to plow.</param>
        /// <param name="where">Where to plow the soil.</param>
        public ActionHarvest(Ident target, Vec2i where)
        {
            this.target = target;
            this.where  = where;
        }
    }
}