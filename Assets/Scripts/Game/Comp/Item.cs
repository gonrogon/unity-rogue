using UnityEngine;
using Rogue.Coe;
using Newtonsoft.Json;

namespace Rogue.Game.Comp
{
    public class Item : GameComponent<Item>
    {
        public int condition = 100;

        public bool sell = false;

        public Item()
        {}
    }
}
