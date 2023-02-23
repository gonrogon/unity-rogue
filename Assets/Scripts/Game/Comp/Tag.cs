using Rogue.Coe;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Rogue.Game.Comp
{
    public class Tag : GameComponent<Tag>
    {
        /// <summary>
        /// Type of damage.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public TagType tags = TagType.None;

        public Tag() {}

        public Tag(TagType tags)
        {
            this.tags = tags;
        }

        public bool ContainsOne(TagType tags)
        {
            return (this.tags & tags) != TagType.None;
        }

        
        public bool ContainsAll(TagType tags)
        {
            return (this.tags & tags) == tags;
        }
        
        public void Add(TagType tags)
        {
            this.tags |= tags;
        }

        public void Remove(TagType tags)
        {
            this.tags ^= tags;
        }
    }
}
