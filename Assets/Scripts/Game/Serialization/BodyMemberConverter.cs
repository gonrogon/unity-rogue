using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Rogue.Coe;
using Rogue.Core;
using System;
using System.Reflection;

namespace Rogue.Game.Serialization
{
    public class BodyMemberConverter : JsonConverter<BodyMember>
    {
        public override BodyMember ReadJson(JsonReader reader, Type objectType, BodyMember existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            serializer.Converters.Add(new StringEnumConverter());

            JObject    jobj = serializer.Deserialize<JObject>(reader);
            BodyMember member;

            if (hasExistingValue)
            {
                member = existingValue;
            }
            else
            {
                member = new ();
            }

            member.name = jobj.Value<string>("name");
            member.id   = jobj.Value<string>("id");
            member.type = serializer.Deserialize<BodyMember.Type>(jobj.GetValue("type").CreateReader());
            
            if (jobj.ContainsKey("armor") && jobj.Value<bool>("armor"))
            {
                member.flags |= BodyMember.Flags.Armor;

                if (jobj.ContainsKey("armorId"))
                {
                    member.armor = Ident.CreateFromRaw(jobj.Value<uint>("armorId"));
                }
            }
            else
            {
                member.flags &= ~BodyMember.Flags.Armor;
            }

            if (jobj.ContainsKey("wield") && jobj.Value<bool>("wield"))
            {
                member.flags |= BodyMember.Flags.Wield;

                if (jobj.ContainsKey("wieldId"))
                {
                    member.wield = Ident.CreateFromRaw(jobj.Value<uint>("armorId"));
                }
            }
            else
            {
                member.flags &= ~BodyMember.Flags.Wield;
            }

            return member;
        }

        public override void WriteJson(JsonWriter writer, BodyMember value, JsonSerializer serializer)
        {
            serializer.Converters.Add(new StringEnumConverter());

            writer.WriteStartObject();

            writer.WritePropertyName("name");
            writer.WriteValue(value.name);

            writer.WritePropertyName("id");
            writer.WriteValue(value.id);

            writer.WritePropertyName("type");
            serializer.Serialize(writer, value.type);

            writer.WritePropertyName("armor");
            writer.WriteValue(value.AllowArmor);

            if (!value.armor.IsZero)
            {
                writer.WritePropertyName("armorId");
                writer.WriteValue(value.armor.Raw);
            }
            
            writer.WritePropertyName("wield");
            writer.WriteValue(value.AllowWield);

            if (!value.wield.IsZero)
            {
                writer.WritePropertyName("wieldId");
                writer.WriteValue(value.wield.Raw);
            }

            writer.WriteEndObject();
        }
    }
}
