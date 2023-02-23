using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

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
        private readonly TemplateDatabase m_database;

        /// <summary>
        /// Template that is being converted.
        /// </summary>
        private readonly Template m_template;

        public TemplateComponentConverter(TemplateDatabase database, Template template)
        {
            m_database = database;
            m_template = template;
        }

        public override TemplateComponent ReadJson(JsonReader reader, Type objectType, TemplateComponent existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject jobj = serializer.Deserialize<JObject>(reader);
            TemplateComponent comp;
            // Creates a new component if it is needed.
            if (hasExistingValue)
            {
                comp = existingValue;
            }
            else
            {
                // Get the main properties to establish the type of component.
                bool @override = GetOverride (jobj, serializer, out TemplateOverride overrideType, out int overrideIndex);
                bool flyweight = GetFlyweight(jobj);
                // If the template component is an overwrite, the component inherited from the base template has to be
                // found to overwrite its values.
                if (@override)
                {
                    string @class = GetClass(jobj);

                    if (GameComponentUtil.TryGetComponent(@class, out Type type))
                    {
                        if (flyweight)
                        {
                            comp = OverideFlyweight (m_template, type, overrideType, overrideIndex);
                        }
                        else
                        {
                            comp = OverrideComponent(m_template, type, overrideType, overrideIndex);
                        }
                    }
                    else
                    {
                        throw new JsonException($"Component class \"{@class}\" from template \"{m_template.Name}\" not found");
                    }
                }
                else
                {
                    comp = TemplateComponent.CreateNew(null);
                }
            }
            // Read the object.
            serializer.Populate(jobj.CreateReader(), comp);
            // Note that if the template component is an overwrite, it values are written over a component already in
            // the list, so a new one is not created. As a result, null values may be inserted into the list, a custom
            // list converter must be used to omit these values.
            if (comp.Override != TemplateOverride.None)
            {
                return null;
            }
            else
            {
                return comp;
            }
        }

        public override void WriteJson(JsonWriter writer, TemplateComponent value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        #region @@@ UTILITIES @@@

        private static bool GetFlyweight(JObject jobj)
        {
            return jobj.ContainsKey("flyweight") && (bool)jobj["flyweight"];
        }

        private static bool GetOverride(JObject jobj, JsonSerializer serializer, out TemplateOverride @override, out int index)
        {
            index = -1;
            if (jobj.ContainsKey("overrideIndex"))
            {   
                index = (int)jobj["overrideIndex"];
            }

            if (jobj.ContainsKey("override"))
            {
                @override = serializer.Deserialize<TemplateOverride>(jobj["override"].CreateReader());
            }
            else
            {
                @override = TemplateOverride.None;
            }

            return @override != TemplateOverride.None;
            //return jobj.ContainsKey("override") && ((string)jobj["override"]).Length > 0;
        }

        private static bool GetRemove(JObject jobj)
        {
            return jobj.ContainsKey("remove") && (bool)jobj["remove"];
        }

        private static string GetClass(JObject jobj)
        {
            return (string)jobj["component"][0];
        }

        private static TemplateComponent OverrideComponent(Template template, Type type, TemplateOverride overrideType, int overwriteIndex)
        {
            TemplateComponent result;
            // Try to find the template component to overwrite. If found, the template component is overwritten
            // with the new values; otherwise, a new template component is created.
            int index = template.FindComponentIndex(type, overwriteIndex < 0 ? 0 : overwriteIndex);
            if (index < 0)
            {
                result = TemplateComponent.CreateNew(null);
            }
            else
            {
                result = template.GetComponentInfo(index);
                // If the template component to overwrite is inherited, its data is cloned to avoid overriding the
                // base template.
                if (result.Inherited)
                {
                    result.component = result.component.Clone();
                }
            }

            result.Inherited     = false;
            result.Flyweight     = false;
            result.Override      = overrideType;
            result.OverrideIndex = overwriteIndex;

            return result;
        }

        private static TemplateComponent OverideFlyweight(Template template, Type type, TemplateOverride overrideType, int overwriteIndex)
        {
            TemplateComponent result;
            // Try to find the template component to overwrite. If found, the template component is overwritten
            // with the new values; otherwise, a new template component is created.
            int index = template.FindFlyweightIndex(type, overwriteIndex < 0 ? 0 : overwriteIndex);
            if (index < 0)
            {
                result = TemplateComponent.CreateFlyweight(null, false);
            }
            else
            {
                result = template.GetFlyweightInfo(index);
                // If the template component to overwrite is inherited, its data is cloned to avoid overriding the
                // base template.
                if (result.Inherited)
                {
                    result.component = result.component.Clone();
                }
            }

            result.Inherited     = false;
            result.Flyweight     = true;
            result.Override      = overrideType;
            result.OverrideIndex = overwriteIndex;

            return result;
        }

        #endregion
    }
}
