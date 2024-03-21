using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Rogue.Coe
{
    /// <summary>
    /// Defines a template behavior.
    /// </summary>
    public class TemplateBehaviour : TemplateMember
    {
        /// <summary>
        /// Behavior type.
        /// </summary>
        public string behaviour = null;

        /// <summary>
        /// Creates a template behaviour for a behaviour.
        /// </summary>
        /// <param name="behaviour">Type of behaviour.</param>
        /// <param name="inherited">Flag indicating whether the component was inherited or not.</param>
        /// <returns>Template behaviour.</returns>
        public static TemplateBehaviour Create(string behaviour, bool inherited)
        {
            return new ()
            {
                Flags     = inherited ? TemplateFlag.Inherited : TemplateFlag.None,
                behaviour = behaviour
            };
        }

        /// <summary>
        /// Clones the template behaviour as an inherited template behaviour.
        /// </summary>
        /// <returns>Clone.</returns>
        public TemplateBehaviour CloneAsInherited()
        {
            return new ()
            {
                Flags         = TemplateFlag.Inherited,
                OverrideIndex = OverrideIndex,
                behaviour     = behaviour
            };
        }
    }
}
