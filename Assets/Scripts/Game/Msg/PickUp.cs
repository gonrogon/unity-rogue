using Rogue.Coe;
using Rogue.Core;

namespace Rogue.Game.Msg
{
    /// <summary>
    /// Define a message to notify an entity to pick up something.
    /// </summary>
    public class PickUp : GameMessage<PickUp>
    {
        /// <summary>
        /// What to pick up.
        /// </summary>
        public Ident what;

        public bool done = false;

        /// <summary>
        /// Constructor.
        /// </summary>
        public PickUp() {}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="what">What to pick up.</param>
        public PickUp(Ident what)
        {
            this.what = what;
        }
    }
}