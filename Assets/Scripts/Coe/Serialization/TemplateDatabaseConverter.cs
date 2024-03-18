using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Rogue.Coe.Serialization
{
    /// <summary>
    /// Defines a converter to deserialize a template database.
    /// </summary>
    public class TemplateDatabaseConverter : JsonConverter<TemplateDatabase>
    {
        public override bool CanWrite => false;

        /// <summary>
        /// Database where to store the templates.
        /// </summary>
        private TemplateDatabase m_database;

        /// <summary>
        /// Constructor.
        /// </summary>
        public TemplateDatabaseConverter() {}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="database">Database where to store the templates or null to create a new one.</param>
        public TemplateDatabaseConverter(TemplateDatabase database) => Reset(database);

        /// <summary>
        /// Resets the converters.
        /// </summary>
        /// <param name="database">Database where to store the templates or null to create a new one.</param>
        public void Reset(TemplateDatabase database)
        {
            m_database = database;
        }

        public override TemplateDatabase ReadJson(JsonReader reader, Type objectType, TemplateDatabase existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jtoken   = serializer.Deserialize<JToken>(reader);
            var database = hasExistingValue ? existingValue : m_database;
            // Check if a valid database was set.
            if (database == null)
            {
                #if UNITY_2017_1_OR_NEWER
                    UnityEngine.Debug.Log("Unable to read database, database not set");
                #endif

                return null;
            }
            // If the top level token is an array, the document represents a list of templates.
            if (jtoken is JArray array)
            {
                for (int i = 0; i < array.Count; i++)
                {
                    Template template = serializer.Deserialize<Template>(array[i].CreateReader());
                    if (template != null)
                    {
                        database.Add(template);
                    }
                }

                return database;
            }
            // If the top level token is an object, the document can only contain a template.
            if (jtoken is JObject jobj)
            {
                Template template = serializer.Deserialize<Template>(jobj.CreateReader());
                if (template != null)
                {
                    database.Add(template);
                }

                return database;
            }
            // Invalid JSON.
            return null;
        }

        public override void WriteJson(JsonWriter writer, TemplateDatabase value, JsonSerializer serializer)
        {
            throw new NotImplementedException("database serialization not implemented");
        }
    }
}
