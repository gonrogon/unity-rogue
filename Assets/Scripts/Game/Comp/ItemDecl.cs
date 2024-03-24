using Rogue.Coe;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Rogue.Game.Comp
{
    public class ItemDecl : GameComponent<ItemDecl>
    {
        [JsonIgnore]
        public CategoryId category = CategoryId.None;

        public ItemType type = ItemType.None;

        public int price = 100;

        public bool saleable = true;

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            category = Context.ItemTypes.GetCategory(type);
        }
    }
}
