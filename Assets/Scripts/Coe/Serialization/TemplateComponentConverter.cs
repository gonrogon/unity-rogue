using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

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
            if (reader.TokenType == JsonToken.StartObject)
            {
                return ReadJsonObj(reader, objectType, existingValue, hasExistingValue, serializer);
            }

            if (reader.TokenType == JsonToken.StartArray)
            {
                return ReadJsonArray(reader, objectType, existingValue, hasExistingValue, serializer);
            }

            return null;            
        }

        public TemplateComponent ReadJsonArray(JsonReader reader, Type objectType, TemplateComponent existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jarray = serializer.Deserialize<JArray>(reader);
            TemplateComponent tc = null;

            if (TemplateUtil.ParseComponentName((string)jarray[0], out string name, out bool flyweight, out var @override, out int overrideIndex))
            {
                if (GameComponentUtil.TryGetComponent(name, out Type type))
                {
                    if (flyweight)
                    {
                        tc = m_template.OverrideFlyweight(type, @override, overrideIndex);
                    }
                    else
                    {
                        tc = m_template.OverrideComponent(type, @override, overrideIndex);
                    }
                }
                else
                {
                    #if UNITY_2017_1_OR_NEWER
                        UnityEngine.Debug.LogError($"Unable to create component {name} in template {m_template.Name}, component type not found");
                    #endif
                }
            }

            if (tc != null)
            { 
                if (tc.component == null)
                {
                    tc.component = serializer.Deserialize<IGameComponent>(jarray.CreateReader());
                }
                else
                {
                    serializer.Populate(jarray[1].CreateReader(), tc.component);
                }
            }

            return null;
        }

        public TemplateComponent ReadJsonObj(JsonReader reader, Type objectType, TemplateComponent existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject jobj = serializer.Deserialize<JObject>(reader);
            TemplateComponent tc = null;

            if (hasExistingValue)
            {
                tc = existingValue;
            }
            else
            {
                if (TemplateUtil.ParseComponentName((string)jobj["component"][0], out string name, out bool flyweight, out var @override, out int overrideIndex))
                {
                    if (GameComponentUtil.TryGetComponent(name, out Type type))
                    {
                        if (flyweight)
                        {
                            tc = m_template.OverrideFlyweight(type, @override, overrideIndex);
                        }
                        else
                        {
                            tc = m_template.OverrideComponent(type, @override, overrideIndex);
                        }
                    }
                }
            }

            if (tc != null)
            { 
                serializer.Populate(jobj.CreateReader(), tc);
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, TemplateComponent value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
