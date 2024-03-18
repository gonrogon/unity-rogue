using Newtonsoft.Json;

namespace Rogue.Coe
{
    public class TemplateMember
    {
        /// <summary>
        /// Flags.
        /// </summary>
        [JsonIgnore]
        public TemplateFlag Flags { get; set; } = TemplateFlag.None;

        /// <summary>
        /// Index of the component of the same type to overwrite or less than zero to overwrite the first one.
        /// </summary>
        [JsonIgnore]
        public int OverrideIndex{ get; set; } = -1;

        /// <summary>
        /// Checks whether the member was inherited from a base template or not.
        /// </summary>
        public bool IsInherited => (Flags & TemplateFlag.Inherited) != 0;

        /// <summary>
        /// Checks whether the member overrides a member in a base template or not.
        /// </summary>
        public bool IsOverrided => (Flags & TemplateFlag.Overrided) != 0;

        /// <summary>
        /// Checks whether the member has been marked as removed or not.
        /// </summary>
        public bool IsRemoved => (Flags & TemplateFlag.Removed) != 0;

        public bool IsFlyweight => (Flags & TemplateFlag.Flyweight) != 0;

        public bool IsOverrideReplace => (Flags & TemplateFlag.OverrideReplace) != 0;

        public bool IsOverrideRemove => (Flags & TemplateFlag.OverrideRemove) != 0;

        public TemplateMember() {}

        public TemplateMember(TemplateFlag flags) 
        {
            Flags = flags;
        }

        public void MarkAsInherited() => Flags |= TemplateFlag.Inherited;

        public void MarkAsOverrided() => Flags |= TemplateFlag.Overrided;

        public void MarkAsRemoved() =>  Flags |= TemplateFlag.Removed;

        public void ClearInherited() => Flags &= ~TemplateFlag.Inherited;

        public void ClearOverrided() => Flags &= ~TemplateFlag.Overrided;

        public void ClearRmeoved() => Flags &= ~TemplateFlag.Removed;
    }
}
