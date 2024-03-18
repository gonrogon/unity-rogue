using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Rogue.Game
{
    public class ItemTypeConverter : JsonConverter<ItemType>
    {
        public override ItemType ReadJson(JsonReader reader, Type objectType, ItemType existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            //var jobj = JToken.ReadFrom(reader);
            //var name = jobj.Value<string>();

            return Context.ItemTypes.GetType((string)JToken.ReadFrom(reader));
        }

        public override void WriteJson(JsonWriter writer, ItemType value, JsonSerializer serializer)
        {
            writer.WriteValue(Context.ItemTypes.GetName(value));
        }
    }
}
