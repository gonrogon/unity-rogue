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
        /// Behavior type.
        /// </summary>
        public string type = null;

        /// <summary>
        /// Creates a new template behaviour.
        /// </summary>
        /// <param name="component">Behavior type.</param>
        /// <returns>Template behaviour.</returns>
        public static TemplateBehaviour CreateNew(string type) => new ()
        {
            type      = type,
            Inherited = false
        };

        /// <summary>
        /// Creates an inherited template behaviour.
        /// </summary>
        /// <param name="component">Behavior type.</param>
        /// <returns>Template behaviour.</returns>
        public static TemplateBehaviour CreateInherited(string type) => new ()
        {
            type      = type,
            Inherited = true
        };

        /// <summary>
        /// Clones the template behaviour as an inherited behaviour.
        /// </summary>
        /// <returns>Template behaviour.</returns>
        public TemplateBehaviour CloneAsInherited() => CreateInherited(type);
    }
}
