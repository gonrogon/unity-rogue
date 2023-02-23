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
        //public override bool CanWrite => false;

        /// <summary>
        /// Template database.
        /// </summary>
        private readonly TemplateDatabase m_database;

        /// <summary>
        /// Template that is being converted.
        /// </summary>
        private readonly Template m_template;

        public TemplateBehaviourConverter(TemplateDatabase database, Template template)
        {
            m_database = database;
            m_template = template;
        }

        public override TemplateBehaviour ReadJson(JsonReader reader, Type objectType, TemplateBehaviour existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JToken jobj = serializer.Deserialize<JToken>(reader);
            TemplateBehaviour behav;
            // Flag indicating whether or not this template behaviour overwrite one inherited from the base template.
            bool overwrite = false;
            // Creates a new component if it is needed.
            if (hasExistingValue)
            {
                behav = existingValue;
            }
            else
            {
                var type  = (string)jobj;

                if (type[0] == '-')
                {
                    m_template.RemoveBehaviour(type.Substring(1));
                    overwrite = true;
                    behav     = null;
                }
                else
                {
                    int index = m_template.FindBehaviourIndex(type);

                    if (index < 0)
                    {
                        behav = TemplateBehaviour.CreateNew(type);
                    }
                    else
                    {
                        behav           = m_template.GetBehaviourInfo(index);
                        behav.Inherited = false;
                        behav.type      = type;
                        overwrite       = true;
                    }
                }
            }

            if (overwrite)
            {
                return null;
            }
            else
            {
                return behav;
            }
        }

        public override void WriteJson(JsonWriter writer, TemplateBehaviour value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.type);
        }
    }
}
