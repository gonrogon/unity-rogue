using UnityEngine;
using Rogue.Coe;
using Newtonsoft.Json;

namespace Rogue.Game.Comp
{
    public class ItemDecl : GameComponent<ItemDecl>
    {
        public CategoryId category;

        public ItemType type;

        public int price;

        public int condition;
    }
}
