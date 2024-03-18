using System;

namespace Rogue.Coe
{
    [Flags]
    public enum TemplateFlag
    {
        None            = 0,  // No flags.
        Inherited       = 1,  // Member is inherited from a base template.
        Overrided       = 2,  // Member is overrided with new data.
        Removed         = 4,  // Member is marked for removal.
        Flyweight       = 8 , // Member is a flyweight.
        OverrideReplace = 16, // Member should replace a member in the base template.
        OverrideRemove  = 32, // Member should remove a member in the base template.
    }
}
