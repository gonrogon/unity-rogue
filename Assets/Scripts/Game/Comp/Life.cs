using UnityEngine;
using Rogue.Coe;

namespace Rogue.Game.Comp
{
    public class Life : GameComponent<Life>
    {
        /// <summary>
        /// Maximum life level.
        /// </summary>
        public int max = 100;

        /// <summary>
        /// Initial life level.
        /// </summary>
        public int initial = 100;

        /// <summary>
        /// Current life level.
        /// </summary>
        public int current = 100;

        public Life() {}

        public Life(int max, int initial, int current)
        {
            this.max     = max;
            this.initial = initial > max ? max : initial;
            this.current = current > max ? max : current;
        }

        public int Add(int amount)
        {
            return current = Mathf.Clamp(current + amount, 0, max);
        }
    }
}
