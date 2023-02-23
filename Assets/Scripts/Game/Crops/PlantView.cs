using System.Collections.Generic;

namespace Rogue.Game.Crops
{
    public class PlantView
    {
        /// <summary>
        /// Biome.
        /// </summary>
        public string biome = string.Empty;

        /// <summary>
        /// Wall to use.
        /// </summary>
        public string wall = string.Empty;

        /// <summary>
        /// Floor to use.
        /// </summary>
        public string floor = string.Empty;

        /// <summary>
        /// Minimum level of growth to use the view.
        /// </summary>
        public int growth = 0;
    }
}
