using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Rogue.Coe.Serialization
{
    /// <summary>
    /// Defines a converter for the list of template behaviours.
    /// </summary>
    public class TemplateBehaviourListConverter : JsonConverter<List<TemplateBehaviour>>
    {
        /// <summary>
        /// Template database.
        /// </summary>
        private TemplateDatabase m_database;

        /// <summary>
        /// Template that is being converted.
        /// </summary>
        private Template m_template;

        public TemplateBehaviourListConverter() {}

        public TemplateBehaviourListConverter(TemplateDatabase database, Template template) => Reset(database, template);

        public void Reset(TemplateDatabase database, Template template)
        {
            m_database = database;
            m_template = template;
        }

        public override List<TemplateBehaviour> ReadJson(JsonReader reader, Type objectType, List<TemplateBehaviour> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jarray =  serializer.Deserialize<JArray>(reader);
            if (jarray == null)
            {
                #if UNITY_2017_1_OR_NEWER
                    UnityEngine.Debug.LogError("Invalid JSON, array of behaviours expected");
                #endif

                return null;
            }
            // Create a new list or use the existing one.
            List<TemplateBehaviour> list = hasExistingValue ? existingValue : new();
            // Process each element of the array.
            foreach (JToken token in jarray)
            {
                if (token.Type != JTokenType.String)
                {
                    #if UNITY_2017_1_OR_NEWER
                        UnityEngine.Debug.LogWarning("Invalid JSON, behaviour name expected");
                    #endif

                    continue;
                }
                // Try to convert the behaviour. Note that the template behaviours are converted using a custom
                // converter, this converter returns null because it adds the behaviours automatically to the template.
                var tb = serializer.Deserialize<TemplateBehaviour>(token.CreateReader());
                if (tb != null)
                {
                    list.Add(tb);
                }
            }

            return list;
        }

        public override void WriteJson(JsonWriter writer, List<TemplateBehaviour> value, JsonSerializer serializer)
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
