using Rogue.Core;

namespace Rogue.Game.Msg
{
    /// <summary>
    /// Define a message to perform a blow attack.
    /// </summary>
    public class ActionAttackBlow : Action<ActionAttackBlow>
    {
        /// <summary>
        /// Origin entity.
        /// </summary>
        public Ident origin;
        
        /// <summary>
        /// Target entity.
        /// </summary>
        public Ident target;

        /// <summary>
        /// Weapon to attack with.
        /// </summary>
        public Ident weapon;
    }
}