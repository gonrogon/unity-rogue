using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Rogue.Coe.Serialization
{
    /// <summary>
    /// Defines a converter for the list of components of a template.
    /// </summary>
    public class TemplateComponentListConverter : JsonConverter<List<TemplateComponent>>
    {
        /// <summary>
        /// Template database.
        /// </summary>
        private readonly TemplateDatabase m_database;

        /// <summary>
        /// Template that is being converted.
        /// </summary>
        private readonly Template m_template;

        public TemplateComponentListConverter(TemplateDatabase database, Template template)
        {
            m_database = database;
            m_template = template;
        }

        public override List<TemplateComponent> ReadJson(JsonReader reader, Type objectType, List<TemplateComponent> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jarray =  serializer.Deserialize<JArray>(reader);
            if (jarray == null)
            {
                return null;
            }
            // Create a new list or use the existing one.
            List<TemplateComponent> list = hasExistingValue ? existingValue : new();
            // Process each element of the array.
            foreach (JToken token in jarray)
            {
                // List can only contains object, other elements are ignore.
                if (token is not JObject jobj)
                {
                    continue;
                }
                // Try to convert the component. Note that the template components are converted using a custom
                // converter, this converter returns null if the component is an overwrite because there is no need
                // to create a new one.
                TemplateComponent tc = serializer.Deserialize<TemplateComponent>(jobj.CreateReader());
                if (tc != null)
                {
                    list.Add(tc);
                }
            }

            return list;
        }

        public override void WriteJson(JsonWriter writer, List<TemplateComponent> value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            {
                for (int i = 0; i < value.Count; i++)
                {
                    if (value[i].Inherited)
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
