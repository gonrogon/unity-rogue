using UnityEngine;
using Rogue.Coe;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Rogue.Game.Comp
{
    public class ItemDecl : GameComponent<ItemDecl>
    {
        [JsonIgnore]
        public CategoryId category;

        public ItemType type;

        public int price;

        public int condition;

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            category = Context.ItemTypes.GetCategory(type);
        }
    }
}
