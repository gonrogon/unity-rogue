using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Rogue.Game
{
    public class CategoryIdConverter : JsonConverter<CategoryId>
    {
        public override CategoryId ReadJson(JsonReader reader, Type objectType, CategoryId existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jobj = JToken.ReadFrom(reader);
            var path = (string)jobj;
            // Try to find a valid category.
            var category = Context.Categories.FindAny(path);
            if (category == null)
            {
                return CategoryId.None;
            }

            return category.Id;
        }

        public override void WriteJson(JsonWriter writer, CategoryId value, JsonSerializer serializer)
        {
            writer.WriteValue(Context.Categories.GeneratePath(value));
        }
    }
}
