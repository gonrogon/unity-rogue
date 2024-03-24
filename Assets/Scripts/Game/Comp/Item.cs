using Rogue.Coe;

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
