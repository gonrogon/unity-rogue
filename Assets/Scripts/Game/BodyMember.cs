using System;
using Rogue.Core;

namespace Rogue.Game
{
    public class BodyMember
    {
        [Flags]
        public enum Flags
        {
            None  = 0,
            Hold  = 1,
            Armor = 2
        }

        public enum Type { Head, Chest, Arm, Hand, Leg, Foot }

        public Type type = Type.Head;

        public Flags flags = Flags.None;

        public string name = "";

        public string id = "";

        public Ident hold = Ident.Zero;

        public Ident armor = Ident.Zero;

        public bool AllowHold => (flags & Flags.Hold) != Flags.None;

        public bool AllowArmor => (flags & Flags.Armor) != Flags.None;

        public bool IsHolding => !hold.IsZero;

        public bool IsArmored => !armor.IsZero;

        public BodyMember() {}

        public BodyMember(Type type, string name, string id) : this(type, name, id, Ident.Zero, Ident.Zero) {}

        public BodyMember(Type type, string name, string id, Ident hold, Ident armor)
        {
            this.type  = type;
            this.name  = name;
            this.id    = id;
            this.hold  = hold;
            this.armor = armor;
            this.flags = Flags.Armor | ((type == Type.Hand) ? Flags.Hold : Flags.None);
        }
    }
}
