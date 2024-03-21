using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using UnityEngine;

namespace Rogue.Coe.Serialization
{
    public class GameComponentConverter : JsonConverter<IGameComponent>
    {
        public override bool CanWrite => false;

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

                    if (comp == null)
                    {
                        #if UNITY_2017_1_OR_NEWER
                            UnityEngine.Debug.LogWarning($"Unable to create component \"{name}\", component type not found");
                        #endif

                        return null;
                    }
                }
                else
                {
                    #if UNITY_2017_1_OR_NEWER
                        UnityEngine.Debug.LogWarning($"Unable to read component \"{name}\", invalid component name");
                    #endif

                    return null;
                }
            }
            // Populate the already created component with the JSON data.
            if (jarray.Count > 1)
            {
                serializer.Populate(jarray[1].CreateReader(), comp);
            }

            return comp;
        }

        public override void WriteJson(JsonWriter writer, IGameComponent value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
            /*
            m_preprocessed = true;

            writer.WriteStartArray();
            {
                writer.WriteValue(value.GetType().Name);
                serializer.Serialize(writer, value);
            }
            writer.WriteEndArray();

            m_preprocessed = false;
            */
        }
    }
}
