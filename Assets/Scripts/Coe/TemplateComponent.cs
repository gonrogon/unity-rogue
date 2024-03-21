using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace Rogue.Coe
{
    /// <summary>
    /// Defines a template component.
    /// </summary>
    public class TemplateComponent : TemplateMember
    {
        /// <summary>
        /// Component.
        /// </summary>
        [JsonProperty(Order = 0)]
        public IGameComponent component = null;

        /// <summary>
        /// List with the properties changed in the component.
        /// </summary>
        [JsonIgnore]
        public List<string> changes = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        protected TemplateComponent() {}

        /// <summary>
        /// Creates a template component for a component.
        /// </summary>
        /// <param name="component">Component.</param>
        /// <param name="inherited">Flag indicating whether the component was inherited or not.</param>
        /// <returns>Template component.</returns>
        public static TemplateComponent Create(IGameComponent component, bool inherited)
        {
            return new()
            {
                Flags     = inherited ? TemplateFlag.Inherited : TemplateFlag.None,
                component = component,
            };
        }

        /// <summary>
        /// Creates a template component for a flyweight component.
        /// </summary>
        /// <param name="component">Component.</param>
        /// <param name="inherited">Flag indicating whether the component was inherited or not.</param>
        /// <returns>Template component.</returns>
        public static TemplateComponent CreateFlyweight(IGameComponent component, bool inherited)
        {
            return new ()
            {
                Flags     = TemplateFlag.Flyweight | (inherited ? TemplateFlag.Inherited : TemplateFlag.None),
                component = component,
            };
        }

        /// <summary>
        /// Clones the template component as an inherited template component.
        /// </summary>
        /// <returns>Clone.</returns>
        public TemplateComponent CloneAsInherited()
        {
            return new()
            {
                Flags         = TemplateFlag.Inherited | (IsFlyweight ? TemplateFlag.Flyweight : TemplateFlag.None),
                OverrideIndex = OverrideIndex,
                component     = component.Clone()
            };
        }

        /// <summary>
        /// Records a field or property of the component that has been modified.
        /// </summary>
        /// <param name="name">Name of the field or property.</param>
        public void RecordChange(string name)
        {
            if (changes == null)
            {
                changes = new ();
            }

            changes.Add(name);
        }

        /// <summary>
        /// Clears the list of changes.
        /// </summary>
        public void ClearChanges() => changes.Clear();
    }
}
