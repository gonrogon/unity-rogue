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
        /// Flag indicating whether the component was inherited from other template or not.
        /// </summary>
        //[JsonIgnore]
        //public bool Inherited { get; set; } = false;

        /// <summary>
        /// Flag indicating whether the component is a fly weight or not.
        /// </summary>
        //[JsonProperty(PropertyName = "flyweight")]
        //public bool Flyweight { get; set; } = false;

        /// <summary>
        /// Flag indicating whether the component overwrites the values of a component in a base template or not.
        /// </summary>
        //[JsonProperty(PropertyName = "override")]
        //public TemplateOverride Override { get; set; } = TemplateOverride.None;

        /// <summary>
        /// Index of the component of the same type to overwrite or less than zero to overwrite the first one.
        /// </summary>
        //[JsonProperty(PropertyName = "overwriteIndex")]
        //public int OverrideIndex{ get; set; } = -1;

        //[JsonIgnore]
        //public int OverrideState { get; set; } = 0;

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

        //public bool IsOverride => Override != TemplateOverride.None;

        //public bool IsReplace => Override == TemplateOverride.Replace;

        //public bool IsRemove => Override == TemplateOverride.Remove;

        public TemplateComponent() {}

        public TemplateComponent(IGameComponent component)
        {
            this.component = component;
        }

        public TemplateComponent(IGameComponent component, TemplateFlag flags) : base(flags) 
        {
            this.component = component;
        }

        /// <summary>
        /// Creates a new template component.
        /// </summary>
        /// <param name="component">Component.</param>
        /// <returns>Template component.</returns>
        public static TemplateComponent CreateNew(IGameComponent component) => new (component)
        {
            /*
            component     = component,
            Inherited     = false,
            Flyweight     = false,
            Override      = TemplateOverride.None,
            OverrideIndex = -1
            */
        };

        /// <summary>
        /// Creates an inherited template component.
        /// </summary>
        /// <param name="component">Component.</param>
        /// <returns>Template component.</returns>
        public static TemplateComponent CreateInherited(IGameComponent component) => new (component, TemplateFlag.Inherited)
        {
            /*
            component     = component,
            Inherited     = true,
            Flyweight     = false,
            Override      = TemplateOverride.None,
            OverrideIndex = -1
            */
        };

        public static TemplateComponent CreateFlyweight(IGameComponent component, bool inherited) => new (component, TemplateFlag.Flyweight | (inherited ? TemplateFlag.Inherited : TemplateFlag.None))
        {
            /*
            component     = component,
            Inherited     = inherited,
            Flyweight     = true,
            Override      = TemplateOverride.None,
            OverrideIndex = -1,
            */
        };

        /// <summary>
        /// Creates an overwrite template component.
        /// </summary>
        /// <param name="component">Componet.</param>
        /// <param name="index">Index of the component of the same type to overwrite or less than zero to
        /// overwrite the first one.</param>
        /// <returns>Template component.</returns>
        /*
        public static TemplateComponent CreateOverwrite(IGameComponent component, int index)  => new ()
        {
            component     = component,
            Inherited     = false,
            Flyweight     = false,
            Override      = TemplateOverride.Replace,
            OverrideIndex = index
        };
        */

        #region @@@ SERIALIZATION @@@

        //public bool ShouldSerializeFlyweight() => Flyweight;

        //public bool ShouldSerializeOverwrite() => Override != TemplateOverride.None;

        //public bool ShouldSerializeOverwriteIndex() => Override != TemplateOverride.None && OverrideIndex >= 0;

        #endregion
    }
}
