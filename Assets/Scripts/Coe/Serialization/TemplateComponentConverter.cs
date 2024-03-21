using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Reflection;

namespace Rogue.Coe.Serialization
{
    /// <summary>
    /// Defines a converter for the components of a template.
    /// </summary>
    public class TemplateComponentConverter : JsonConverter<TemplateComponent>
    {
        public override bool CanWrite => false;

        /// <summary>
        /// Template database.
        /// </summary>
        private TemplateDatabase m_database;

        /// <summary>
        /// Template that is being converted.
        /// </summary>
        private Template m_template;

        public TemplateComponentConverter() => Reset(null, null);

        public TemplateComponentConverter(TemplateDatabase database, Template template) => Reset(database, template);

        public void Reset(TemplateDatabase database, Template template)
        {
            m_database = database;
            m_template = template;
        }

        public override TemplateComponent ReadJson(JsonReader reader, Type objectType, TemplateComponent existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JArray jarray = serializer.Deserialize<JArray>(reader);
            TemplateComponent tc;

            if (hasExistingValue)
            {
                tc = existingValue;
            }
            else
            {
                tc = TemplateComponent.Create(null, false);
            }
            // Get the information about the type of component and the parameters for the template component.
            if (TemplateUtil.ParseComponentName((string)jarray[0], out string name, out TemplateFlag flags, out int overrideIndex))
            {
                tc.Flags         |= flags;
                tc.OverrideIndex  = overrideIndex;

                if (!GameComponentUtil.TryGetComponent(name, out Type type))
                {
                    #if UNITY_2017_1_OR_NEWER
                        UnityEngine.Debug.LogError($"Unable to create component {name} in template {m_template.Name}, component type not found");
                    #endif

                    return null;
                }
            }
            else
            {
                #if UNITY_2017_1_OR_NEWER
                    UnityEngine.Debug.LogError($"Unable to create component {name} in template {m_template.Name}, invalid component name");
                #endif

                return null;
            }
            // Get the list of fields and properties of the component that have been modified.
            if (jarray.Count > 1)
            {
                JObject jobj = jarray[1].ToObject<JObject>();

                if (jobj.HasValues)
                {
                    foreach (JProperty property in jobj.Properties())
                    {
                        tc.RecordChange(property.Name);
                    }
                }
            }
            // Deserialize the component.
            tc.component = serializer.Deserialize<IGameComponent>(jarray.CreateReader());

            return tc;
        }

        public override void WriteJson(JsonWriter writer, TemplateComponent value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
