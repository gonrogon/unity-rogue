using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Rogue.Coe.Serialization
{
    public class GameComponentConverter : JsonConverter<IGameComponent>
    {
        private bool m_preprocessed = false;

        public override bool CanWrite
        {
            get
            {
                return !m_preprocessed;
            }
        }

        public override IGameComponent ReadJson(JsonReader reader, Type objectType, IGameComponent existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JArray jarray = serializer.Deserialize<JArray>(reader);
            IGameComponent comp;
            // Creates a new component if it is needed.
            if (hasExistingValue)
            {
                comp = existingValue;
            }
            else
            {
                comp = GameComponentUtil.CreateFromName((string)jarray[0]);
                // Invalid class name.
                if (comp == null)
                {
                    return null;
                }
            }
            
            if (jarray.Count > 1)
            {
                serializer.Populate(jarray[1].CreateReader(), comp);
            }

            return comp;
        }

        public override void WriteJson(JsonWriter writer, IGameComponent value, JsonSerializer serializer)
        {
            m_preprocessed = true;

            writer.WriteStartArray();
            {
                writer.WriteValue(value.GetType().Name);
                serializer.Serialize(writer, value);
            }
            writer.WriteEndArray();

            m_preprocessed = false;
        }
    }
}
