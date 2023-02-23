using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Reflection;

namespace Rogue.Coe.Serialization
{
    public class TemplateDatabaseConverter : JsonConverter<TemplateDatabase>
    {
        //public override bool CanWrite => false;

        private TemplateDatabase m_database;

        public TemplateDatabaseConverter(TemplateDatabase database)
        {
            m_database = database;
        }

        public override TemplateDatabase ReadJson(JsonReader reader, Type objectType, TemplateDatabase existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JToken jtoken = serializer.Deserialize<JToken>(reader);
            TemplateDatabase database;

            if (hasExistingValue)
            {
                database = existingValue;
            }
            else
            {
                if (m_database != null)
                {
                    database = m_database;
                }
                else
                {
                    database = new ();
                }
            }
            // If the top level object of the JSON is an array it represents a list of templates (a database).
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
            // Otherwise, it should contain only one object that represents a template.
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
            writer.WriteStartArray();
            {
                foreach (var name in value.GetTemplateNames())
                {
                    serializer.Serialize(writer, value.Find(name));
                }
            }
            writer.WriteEndArray();
        }
    }
}
