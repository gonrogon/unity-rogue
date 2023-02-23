using Rogue.Coe;
using GG.Mathe;

namespace Tsc.Game.Msg
{
    /// <summary>
    /// Define a message to notify an entity to drop something.
    /// </summary>
    public class DropAll : GameMessage<DropAll>
    {
        /// <summary>
        /// Where to drop all.
        /// </summary>
        Vec2i where;

        /// <summary>
        /// Flag indicating whether it was done or not.
        /// </summary>
        bool done = false;

        /// <summary>
        /// Constructor.
        /// </summary>
        public DropAll() {}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="where">Where to drop all.</param>
        public DropAll(int what, Vec2i where)
        {
            this.where = where;
        }
    }
}