using System.Collections.Generic;

namespace Rogue.Game.Crops
{
    public class PlantDatabase
    {
        /// <summary>
        /// Dictionary with the different plan types.
        /// </summary>
        private Dictionary<string, PlantType> m_plants = new();

        /// <summary>
        /// Checks if the database has a type of plant.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <returns>True on success; otherwise, false.</returns>
        public bool Contains(string name) => m_plants.ContainsKey(name);

        /// <summary>
        /// Adds a plant type.
        /// 
        /// Note that the name of the plant is got from the type.
        /// </summary>
        /// <param name="type">Type to add.</param>
        public void Add(PlantType type)
        {
            if (type == null)
            {
                return;
            }

            m_plants[type.name] = type;
        }

        /// <summary>
        /// Tries to get a plant type.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="type">Type.</param>
        /// <returns>True if the type of plant is found; otherwise, false.</returns>
        public bool TryGet(string name, out PlantType type) => m_plants.TryGetValue(name, out type);
    }
}
