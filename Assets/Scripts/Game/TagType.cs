using System;
using Newtonsoft.Json;

namespace Rogue.Game
{
    [Flags]
    public enum TagType
    {
        None        = 0,
        Player      = 1,
        Enemy       = 1 << 1,
        Door        = 1 << 2,
        Trap        = 1 << 3,
        MeleeWeapon = 1 << 4,
        RangeWeapon = 1 << 5,
        Item        = 1 << 6,
        All         = int.MaxValue
    }
}
