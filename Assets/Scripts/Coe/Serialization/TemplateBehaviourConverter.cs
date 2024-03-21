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
        public override bool CanWrite => false;

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
            JToken jtoken = serializer.Deserialize<JToken>(reader);
            TemplateBehaviour tb;

            if (hasExistingValue)
            {
                tb = existingValue;
            }
            else
            {
                tb = TemplateBehaviour.Create(null, false);
            }

            if (TemplateUtil.ParseBehaviourName((string)jtoken, out string name, out TemplateFlag flags))
            {
                tb.Flags |= flags;

                if (!GameBehaviourUtil.TryGetBehaviour(name, out _))
                {
                    #if UNITY_2017_1_OR_NEWER
                        UnityEngine.Debug.LogError($"Unable to create behaviour {name} in template {m_template.Name}, behaviour type not found");
                    #endif

                    return null;
                }
            }
            else
            {
                #if UNITY_2017_1_OR_NEWER
                    UnityEngine.Debug.LogError($"Unable to create behaviour {name} in template {m_template.Name}, invalid behaviour name");
                #endif

                return null;
            }
            // Template behaviours do not instantiate the behaviours theirself, instead they keep the name (type) of the
            // behaviours to instantiate when an entity is created.
            tb.behaviour = name;

            return tb;
        }

        public override void WriteJson(JsonWriter writer, TemplateBehaviour value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.behaviour);
        }
    }
}
