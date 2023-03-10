using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Rogue.Coe
{
    /// <summary>
    /// Defines a template component.
    /// </summary>
    public class TemplateComponent
    {
        /// <summary>
        /// Flag indicating whether the component was inherited from other template or not.
        /// </summary>
        [JsonIgnore]
        public bool Inherited { get; set; } = false;

        /// <summary>
        /// Flag indicating whether the component is a fly weight or not.
        /// </summary>
        [JsonProperty(PropertyName = "flyweight")]
        public bool Flyweight { get; set; } = false;

        /// <summary>
        /// Flag indicating whether the component overwrites the values of a component in a base template or not.
        /// </summary>
        [JsonProperty(PropertyName = "override")]
        public TemplateOverride Override { get; set; } = TemplateOverride.None;

        /// <summary>
        /// Index of the component of the same type to overwrite or less than zero to overwrite the first one.
        /// </summary>
        [JsonProperty(PropertyName = "overwriteIndex")]
        public int OverrideIndex{ get; set; } = -1;

        /// <summary>
        /// Component.
        /// </summary>
        [JsonProperty(Order = 0)]
        public IGameComponent component = null;

        public bool IsOverride => Override != TemplateOverride.None;

        public bool IsReplace => Override == TemplateOverride.Replace;

        public bool IsRemove => Override == TemplateOverride.Remove;

        /// <summary>
        /// Creates a new template component.
        /// </summary>
        /// <param name="component">Component.</param>
        /// <returns>Template component.</returns>
        public static TemplateComponent CreateNew(IGameComponent component) => new ()
        {
            component     = component,
            Inherited     = false,
            Flyweight     = false,
            Override      = TemplateOverride.None,
            OverrideIndex = -1
        };

        /// <summary>
        /// Creates an inherited template component.
        /// </summary>
        /// <param name="component">Component.</param>
        /// <returns>Template component.</returns>
        public static TemplateComponent CreateInherited(IGameComponent component) => new ()
        {
            component     = component,
            Inherited     = true,
            Flyweight     = false,
            Override      = TemplateOverride.None,
            OverrideIndex = -1
        };

        public static TemplateComponent CreateFlyweight(IGameComponent component, bool inherited) => new ()
        {
            component     = component,
            Inherited     = inherited,
            Flyweight     = true,
            Override      = TemplateOverride.None,
            OverrideIndex = -1,
        };

        /// <summary>
        /// Creates an overwrite template component.
        /// </summary>
        /// <param name="component">Componet.</param>
        /// <param name="index">Index of the component of the same type to overwrite or less than zero to
        /// overwrite the first one.</param>
        /// <returns>Template component.</returns>
        public static TemplateComponent CreateOverwrite(IGameComponent component, int index)  => new ()
        {
            component     = component,
            Inherited     = false,
            Flyweight     = false,
            Override      = TemplateOverride.Replace,
            OverrideIndex = index
        };

        /// <summary>
        /// Clones the template component as an inherited component.
        /// </summary>
        /// <returns>Template component.</returns>
        public TemplateComponent CloneAsInherited() => CreateInherited(component.Clone());

        #region @@@ SERIALIZATION @@@

        public bool ShouldSerializeFlyweight() => Flyweight;

        public bool ShouldSerializeOverwrite() => Override != TemplateOverride.None;

        public bool ShouldSerializeOverwriteIndex() => Override != TemplateOverride.None && OverrideIndex >= 0;

        #endregion
    }
}
