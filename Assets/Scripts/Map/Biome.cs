using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Rogue.Map
{
    public class Biome
    {
        public int m_defaultFloor = -1;

        public int m_defaultWall = -1;

        public int DefaultFloor => m_defaultFloor;

        public int DefaultWall => m_defaultWall;

        public Dictionary<string, int> m_floors = new Dictionary<string, int>();

        public Dictionary<string, int> m_walls = new Dictionary<string, int>();

        public void SetDefaultFloor(string name)
        {
            if (!TryGetFloor(name, out int floor))
            {
                throw new Exception("Unable to set default floor, floor not found");
            }

            m_defaultFloor = floor;
        }

        public bool TryGetFloor(string name, out int floor)
        {
            return m_floors.TryGetValue(name, out floor);
        }

        public void AddFloor(string name, int id)
        {
            m_floors[name] = id;
        }

        public void SetDefaultWall(string name)
        {
            if (!TryGetWall(name, out int wall))
            {
                throw new Exception("Unable to set default wall, wall not found");
            }

            m_defaultWall = wall;
        }

        public bool TryGetWall(string name, out int wall)
        {
            return m_walls.TryGetValue(name, out wall);
        }

        public void AddWall(string name, int id)
        {
            m_walls[name] = id;
        }
    }
}
