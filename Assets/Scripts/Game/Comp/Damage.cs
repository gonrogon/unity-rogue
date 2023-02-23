using Rogue.Coe;

namespace Rogue.Game.Comp
{
    public class Damage : GameComponent<Damage>
    {
        /// <summary>
        /// Type of damage.
        /// </summary>
        public DamageType type = DamageType.Blunt;

        /// <summary>
        /// Maximum damage level.
        /// </summary>
        public int max = 100;

        /// <summary>
        /// Initial damage level.
        /// </summary>
        public int initial = 100;

        /// <summary>
        /// Current damage level.
        /// </summary>
        public int current = 100;

        public Damage() {}

        public Damage(DamageType type, int damage)
            :
            this(type, damage, damage, damage)
        {}

        public Damage(DamageType type, int max, int initial, int current)
        {
            this.type    = type;
            this.max     = max;
            this.initial = initial > max ? max : initial;
            this.current = current > max ? max : current;
        }
    }
}
