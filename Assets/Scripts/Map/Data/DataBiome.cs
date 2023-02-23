using System;
using UnityEngine;

namespace Rogue.Map.Data
{
    /// <summary>
    /// Define a type of floor.
    /// </summary>
    [CreateAssetMenu(menuName = "Map/Biome")]
    public class DataBiome : ScriptableObject
    {
        /// <summary>
        /// Default floor.
        /// </summary>
        public DataFloor defaultFloor;

        /// <summary>
        /// Default wall.
        /// </summary>
        public DataWall defaultWall;

        /// <summary>
        /// List of floors.
        /// </summary>
        public DataFloor[] floors;

        /// <summary>
        /// List of walls.
        /// </summary>
        public DataWall[] walls;

        public DataFloor FindFloor(string name)
        {
            int index = Array.FindIndex(floors, item => item.name == name);
            if (index < 0)
            {
                return null;
            }

            return floors[index];
        }

        public DataWall FindWall(string name)
        {
            int index = Array.FindIndex(walls, item => item.name == name);
            if (index < 0)
            {
                return null;
            }

            return walls[index];
        }

        private void OnValidate()
        {
            if (defaultFloor != null)
            {
                if (Array.FindIndex(floors, floor => ReferenceEquals(floor, defaultFloor)) < 0)
                {
                    Debug.LogWarning("Default floor have to be in the list of floors");
                    defaultFloor = null;
                }
            }

            if (defaultWall != null)
            {
                if (Array.FindIndex(walls, wall => ReferenceEquals(wall, defaultWall)) < 0)
                {
                    Debug.LogWarning("Default wall have to be in the list of walls");
                    defaultWall = null;
                }
            }
        }
    }
}
