using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using UnityEngine;

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
            // Creates an instance of the component.
            if (hasExistingValue)
            {
                comp = existingValue;
            }
            else
            {
                if (TemplateUtil.ParseComponentName((string)jarray[0], out string name))
                {
                    comp = GameComponentUtil.CreateFromName(name);
                }
                else
                {
                    comp = null;
                }
            }
            // Read the component data.
            if (comp != null && jarray.Count > 1)
            {
                serializer.Populate(jarray[1].CreateReader(), comp);
            }
            // Done.
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
