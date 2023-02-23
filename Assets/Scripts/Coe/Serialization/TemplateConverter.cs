using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Reflection;

namespace Rogue.Coe.Serialization
{
    public class TemplateConverter : JsonConverter<Template>
    {
        private readonly TemplateDatabase m_database;

        public TemplateConverter(TemplateDatabase database)
        {
            m_database = database;
        }

        public override Template ReadJson(JsonReader reader, Type objectType, Template existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject  jobj = serializer.Deserialize<JObject>(reader);
            Template template;

            if (hasExistingValue)
            {
                template = existingValue;
            }
            else
            {
                template = new ();
            }
            
            string extends = jobj.Value<string>("extends");
            if (!string.IsNullOrEmpty(extends))
            {
                if (m_database.TryGet(extends, out Template basetpl))
                {
                    template.Extend(basetpl);
                }
            }
            
            var tcompListCvt = new TemplateComponentListConverter(m_database, template);
            var tbehaListCvt = new TemplateBehaviourListConverter(m_database, template);
            var componentCvt = new TemplateComponentConverter(m_database, template);
            var behaviourCvt = new TemplateBehaviourConverter(m_database, template);
            serializer.Converters.Add(tcompListCvt);
            serializer.Converters.Add(tbehaListCvt);
            serializer.Converters.Add(componentCvt);
            serializer.Converters.Add(behaviourCvt);
            serializer.Populate(jobj.CreateReader(), template);
            serializer.Converters.Remove(tcompListCvt);
            serializer.Converters.Remove(tbehaListCvt);
            serializer.Converters.Remove(componentCvt);
            serializer.Converters.Remove(behaviourCvt);

            return template;
        }

        public override void WriteJson(JsonWriter writer, Template value, JsonSerializer serializer)
        {
            var tcompListCvt = new TemplateComponentListConverter(m_database, null);
            var tbehaListCvt = new TemplateBehaviourListConverter(m_database, null);
            var componentCvt = new TemplateComponentConverter(m_database, null);
            var behaviourCvt = new TemplateBehaviourConverter(m_database, null);
            serializer.Converters.Add(tcompListCvt);
            serializer.Converters.Add(tbehaListCvt);
            serializer.Converters.Add(componentCvt);
            serializer.Converters.Add(behaviourCvt);

            writer.WriteStartObject();
            writer.WritePropertyName("name");    writer.WriteValue(value.Name);
            writer.WritePropertyName("extends"); writer.WriteValue(value.Base);
            
            // COMPONENTS

            if (CountSerializableComponents(value) > 0)
            {
                writer.WritePropertyName("components");
                var prop = value.GetType().GetField("m_components", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                var list = prop.GetValue(value);
                serializer.Serialize(writer, list);
            }

            // BEHAVIOURS

            if (CountSerializableBehaviours(value) > 0)
            {
                writer.WritePropertyName("behaviours");
                var prop = value.GetType().GetField("m_behaviours", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                var list = prop.GetValue(value);
                serializer.Serialize(writer, list);
            }

            // VIEW

            if (value.GetViewInfo() != null)
            {
                writer.WritePropertyName("view");
                serializer.Serialize(writer, value.GetViewInfo());
            }

            writer.WriteEndObject();

            serializer.Converters.Remove(tcompListCvt);
            serializer.Converters.Remove(tbehaListCvt);
            serializer.Converters.Remove(componentCvt);
            serializer.Converters.Remove(behaviourCvt);
        }

        private static int CountSerializableComponents(Template template)
        {
            int count = 0;

            for (int i = 0; i < template.ComponentCount; i++)
            {
                if (!template.GetComponentInfo(i).Inherited)
                {
                    count++;
                }
            }

            return count;
        }

        private static int CountSerializableBehaviours(Template template)
        {
            int count = 0;

            for (int i = 0; i < template.BehaviourCount; i++)
            {
                if (!template.GetBehaviourInfo(i).Inherited)
                {
                    count++;
                }
            }

            return count;
        }
    }
}
