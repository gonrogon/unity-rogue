using System;
using Rogue.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace Rogue.Game
{
    public class BodyMember
    {
        [Flags]
        public enum Flags
        {
            None  = 0,
            Wield = 1,
            Armor = 2
        }

        public enum Type { Head, UpperBody, LowerBody, Arm, Hand, Leg, Foot }

        [JsonProperty(PropertyName = "type"), JsonConverter(typeof(StringEnumConverter))]
        public Type type = Type.Head;

        [JsonIgnore]
        public Flags flags = Flags.None;

        [JsonProperty(PropertyName = "name")]
        public string name = "";

        [JsonProperty(PropertyName = "id")]
        public string id = "";

        [JsonIgnore]
        public Ident wield = Ident.Zero;

        [JsonIgnore]
        public Ident armor = Ident.Zero;

        [JsonIgnore]
        public bool AllowWield => (flags & Flags.Wield) != Flags.None;

        [JsonIgnore]
        public bool AllowArmor => (flags & Flags.Armor) != Flags.None;

        [JsonIgnore]
        public bool IsHolding => !wield.IsZero;

        [JsonIgnore]
        public bool IsArmored => !armor.IsZero;

        public BodyMember() {}

        public BodyMember(Type type, string name, string id) : this(type, name, id, Ident.Zero, Ident.Zero) {}

        public BodyMember(Type type, string name, string id, Ident wield, Ident armor)
        {
            this.type  = type;
            this.name  = name;
            this.id    = id;
            this.flags = Flags.Armor | ((type == Type.Hand) ? Flags.Wield : Flags.None);
            this.wield = wield;
            this.armor = armor;
        }

        public BodyMember(BodyMember member)
        {
            type  = member.type;
            name  = member.name;
            id    = member.id;
            flags = member.flags;
            wield = Ident.Zero;
            armor = Ident.Zero;
        }
    }
}
