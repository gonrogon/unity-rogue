using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Reflection;

namespace Rogue.Coe.Serialization
{
    /// <summary>
    /// Defines a converter to serialize/deserialize a template.
    /// </summary>
    public class TemplateConverter : JsonConverter<Template>
    {
        public override bool CanWrite => false;

        /// <summary>
        /// Database where to find references.
        /// </summary>
        private readonly TemplateDatabase m_database;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="database">Database where to find references.</param>
        public TemplateConverter(TemplateDatabase database)
        {
            m_database = database;
        }

        public override Template ReadJson(JsonReader reader, Type objectType, Template existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var  jobj    = serializer.Deserialize<JObject>(reader);
            var template = hasExistingValue ? existingValue : new ();
            // Checks if the templates extends a base template.
            string  name = jobj.Value<string>("name");
            string @base = jobj.Value<string>("extends");
            // Check for valid name.
            if (string.IsNullOrEmpty(name))
            {
                #if UNITY_2017_1_OR_NEWER
                    UnityEngine.Debug.Log($"Invalid template name \"{name}\"");
                #endif

                return null;
            }

            TemplateUtil.PushConverters(serializer, m_database, template);
            serializer.Populate(jobj.CreateReader(), template);
            TemplateUtil.PopConverters(serializer);

            return template;
        }

        public override void WriteJson(JsonWriter writer, Template value, JsonSerializer serializer)
        {
            throw new NotImplementedException("template serialization not implemented");
        }

        /*
        public override void WriteJson(JsonWriter writer, Template value, JsonSerializer serializer)
        {
            TemplateUtil.PushConverters(serializer, m_database, null);

            writer.WriteStartObject();
            writer.WritePropertyName("name");    writer.WriteValue(value.Name);
            writer.WritePropertyName("extends"); writer.WriteValue(value.Base);
            // Write the components.
            if (CountSerializableComponents(value) > 0)
            {
                writer.WritePropertyName("components");
                var prop = value.GetType().GetField("m_components", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                var list = prop.GetValue(value);
                serializer.Serialize(writer, list);
            }
            // Write the behaviours.
            if (CountSerializableBehaviours(value) > 0)
            {
                writer.WritePropertyName("behaviours");
                var prop = value.GetType().GetField("m_behaviours", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                var list = prop.GetValue(value);
                serializer.Serialize(writer, list);
            }
            // Write the view.
            if (value.GetViewInfo() != null)
            {
                writer.WritePropertyName("view");
                serializer.Serialize(writer, value.GetViewInfo());
            }
            // Done.
            writer.WriteEndObject();

            TemplateUtil.PopConverters(serializer);
        }

        /// <summary>
        /// Get the number of serializable components in a template.
        /// </summary>
        /// <param name="template">Template.</param>
        /// <returns>Number of serializable components.</returns>
        private static int CountSerializableComponents(Template template)
        {
            int count = 0;

            for (int i = 0; i < template.ComponentCount; i++)
            {
                if (!template.GetComponentInfo(i).IsInherited)
                {
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Gets the number of serializable behaviours in a template.
        /// </summary>
        /// <param name="template">Template.</param>
        /// <returns>Number of serializable behaviours.</returns>
        private static int CountSerializableBehaviours(Template template)
        {
            int count = 0;

            for (int i = 0; i < template.BehaviourCount; i++)
            {
                if (!template.GetBehaviourInfo(i).IsInherited)
                {
                    count++;
                }
            }

            return count;
        }
        */
    }
}
