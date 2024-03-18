using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Rogue.Coe.Serialization
{
    /// <summary>
    /// Defines a converter for a list of template components.
    /// </summary>
    public class TemplateComponentListConverter : JsonConverter<List<TemplateComponent>>
    {
        /// <summary>
        /// Template database.
        /// </summary>
        private TemplateDatabase m_database;

        /// <summary>
        /// Template that is being converted.
        /// </summary>
        private Template m_template;

        public TemplateComponentListConverter() {}

        public TemplateComponentListConverter(TemplateDatabase database, Template template) => Reset(database, template);

        public void Reset(TemplateDatabase database, Template template)
        {
            m_database = database;
            m_template = template;
        }

        public override List<TemplateComponent> ReadJson(JsonReader reader, Type objectType, List<TemplateComponent> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jarray =  serializer.Deserialize<JArray>(reader);
            if (jarray == null)
            {
                #if UNITY_2017_1_OR_NEWER
                    UnityEngine.Debug.LogError("Invalid JSON, array of components expected");
                #endif

                return null;
            }
            // Create a new list or use the existing one.
            List<TemplateComponent> list = hasExistingValue ? existingValue : new();
            // Process each element of the array.
            foreach (JToken token in jarray)
            {
                if (token.Type != JTokenType.Array)
                {
                    #if UNITY_2017_1_OR_NEWER
                        UnityEngine.Debug.LogWarning("Invalid JSON, component array expected");
                    #endif

                    continue;
                }
                // Try to convert the component. Note that the template components are converted using a custom
                // converter, this converter returns null because it adds the components automatically to the template.
                serializer.Deserialize<TemplateComponent>(token.CreateReader());
            }

            return list;
        }

        public override void WriteJson(JsonWriter writer, List<TemplateComponent> value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            {
                for (int i = 0; i < value.Count; i++)
                {
                    if (value[i].IsInherited)
                    {
                        continue;
                    }

                    serializer.Serialize(writer, value[i]);
                }
            }
            writer.WriteEndArray();
        }
    }
}
