using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Rogue.Coe
{
    /// <summary>
    /// Defines a template behavior.
    /// </summary>
    public class TemplateBehaviour
    {
        /// <summary>
        /// Flag indicating whether the behaviour was inherited from other template or not.
        /// </summary>
        [JsonIgnore]
        public bool Inherited { get; set; } = false;

        /// <summary>
        /// Flag indicating whether the behaviour overwrites the values of a component in a base template or not.
        /// </summary>
        public TemplateOverride Override { get; set; } = TemplateOverride.None;

        [JsonIgnore]
        public int OverrideState { get; set; } = 0;

        /// <summary>
        /// Behavior type.
        /// </summary>
        public string behaviour = null;

        /// <summary>
        /// Flag indicating whether template behaviour is a remove or not.
        /// </summary>
        public bool IsRemove => Override == TemplateOverride.Remove;

        /// <summary>
        /// Creates a new template behaviour.
        /// </summary>
        /// <param name="component">Type of behaviour.</param>
        /// <returns>Template behaviour.</returns>
        public static TemplateBehaviour CreateNew(string type) => new ()
        {
            behaviour = type,
            Inherited = false,
            Override  = TemplateOverride.None
        };

        /// <summary>
        /// Creates an inherited template behaviour.
        /// </summary>
        /// <param name="component">Type of behaviour.</param>
        /// <returns>Template behaviour.</returns>
        public static TemplateBehaviour CreateInherited(string type) => new ()
        {
            behaviour = type,
            Inherited = true,
            Override  = TemplateOverride.None
        };

        /// <summary>
        /// Creates a remove template behaviour.
        /// </summary>
        /// <param name="type">Type of behaviour.</param>
        /// <returns>Template behaviour.</returns>
        public static TemplateBehaviour CreateRemove(string type) => new ()
        {
            behaviour = type,
            Inherited = false,
            Override  = TemplateOverride.Remove
        };
    }
}
