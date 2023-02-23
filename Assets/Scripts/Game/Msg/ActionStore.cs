using Rogue.Coe;
using Rogue.Core;
using GG.Mathe;

namespace Rogue.Game.Msg
{
    /// <summary>
    /// Define a message to notify an entity to drop something.
    /// </summary>
    public class ActionStore : Action<ActionStore>
    {
        /// <summary>
        /// What to store.
        /// </summary>
        public Ident what;

        /// <summary>
        /// Where to store.
        /// </summary>
        public Ident where;

        /// <summary>
        /// Flag indicating whether the action was done or not.
        /// </summary>
        public bool done = false;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ActionStore() {}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="what">What to drop.</param>
        /// <param name="where">Where to drop it.</param>
        public ActionStore(Ident what, Ident where)
        {
            this.what  = what;
            this.where = where;
        }
    }
}