using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Rogue.Coe.Serialization
{
    /// <summary>
    /// Defines a converter for the behaviours of a template.
    /// </summary>
    public class TemplateBehaviourConverter : JsonConverter<TemplateBehaviour>
    {
        /// <summary>
        /// Template database.
        /// </summary>
        private TemplateDatabase m_database;

        /// <summary>
        /// Template that is being converted.
        /// </summary>
        private Template m_template;

        public TemplateBehaviourConverter() => Reset(null, null);

        public TemplateBehaviourConverter(TemplateDatabase database, Template template) => Reset(database, template);

        public void Reset(TemplateDatabase database, Template template)
        {
            m_database = database;
            m_template = template;
        }

        public override TemplateBehaviour ReadJson(JsonReader reader, Type objectType, TemplateBehaviour existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jtoken = serializer.Deserialize<JToken>(reader);
            TemplateBehaviour tb = null;

            if (TemplateUtil.ParseBehaviourName((string)jtoken, out string name, out var @override))
            {
                if (GameBehaviourUtil.TryGetBehaviour(name, out _))
                {
                    tb = m_template.OverrideBehaviour(name, @override);
                }
                else
                {
                    #if UNITY_2017_1_OR_NEWER
                        UnityEngine.Debug.LogError($"Unable to create behaviour {name} in template {m_template.Name}, behaviour type not found");
                    #endif
                }
            }

            if (tb != null)
            {
                // Template behaviour does not instantiate the behaviours theirself, instead the keep the name of the
                // behaviours to instantiate when an entity is created.
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, TemplateBehaviour value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.behaviour);
        }
    }
}
