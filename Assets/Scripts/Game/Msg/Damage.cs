using Rogue.Coe;

namespace Rogue.Game.Msg
{
    /// <summary>
    /// Define a message to perform an action.
    /// </summary>
    public class Damage : GameMessage<Damage>
    {
        /// <summary>
        /// Type of damage.
        /// </summary>
        public DamageType type = DamageType.Blunt;

        /// <summary>
        /// Amount of damage.
        /// </summary>
        public int amount = 0;

        /// <summary>
        /// Constructor.
        /// </summary>
        public Damage() {}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="type">Type of damage.</param>
        /// <param name="amount">Amount of damage.</param>
        public Damage(DamageType type, int amount)
        {
            this.type   = type;
            this.amount = amount;
        }
    }
}