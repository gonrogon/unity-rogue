using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Rogue.Coe
{
    /// <summary>
    /// Defines a template view.
    /// </summary>
    public class TemplateView
    {
        /// <summary>
        /// Flag indicating whether the view was inherited from other template or not.
        /// </summary>
        [JsonIgnore]
        public bool Inherited { get; set; } = false;

        /// <summary>
        /// View type.
        /// </summary>
        public string type = null;

        /// <summary>
        /// View name.
        /// </summary>
        public string name = null;

        /// <summary>
        /// Creates a new template view.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="name">Name.</param>
        /// <returns>Template view.</returns>
        public static TemplateView CreateNew(string type, string name) => new ()
        {
            type      = type,
            name      = name,
            Inherited = false
        };

        /// <summary>
        /// Creates an inherited template view.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="name">Name.</param>
        /// <returns>Template view.</returns>
        public static TemplateView CreateInherited(string type, string name) => new ()
        {
            type      = type,
            name      = name,
            Inherited = true
        };

        /// <summary>
        /// Clones the template view as an inherited view.
        /// </summary>
        /// <returns>Template view.</returns>
        public TemplateView CloneAsInherited() => CreateInherited(type, name);
    }
}
