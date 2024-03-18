using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rogue.Coe;
using System;
using System.Reflection;

namespace Rogue.Game.Serialization
{
    public class BodyConverter : JsonConverter<Body>
    {
        public override Body ReadJson(JsonReader reader, Type objectType, Body existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject jobj = serializer.Deserialize<JObject>(reader);
            Body    body;

            if (hasExistingValue)
            {
                body = existingValue;
            }
            else
            {
                body = new ();
            }

            if (jobj.TryGetValue("members", out JToken value))
            {
                if (value is JArray array)
                {
                    for (int i = 0; i < array.Count; i++) 
                    {
                        body.Add(serializer.Deserialize<BodyMember>(array[i].CreateReader()));
                    }
                }
            }

            //serializer.Populate(reader, body);

            return body;
        }

        public override void WriteJson(JsonWriter writer, Body value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("members");
            writer.WriteStartArray();

            for (int i = 0; i < Body.MaxMembers; i++)
            {
                BodyMember member = value.At(i);
                if (member != null)
                {
                    serializer.Serialize(writer, member);
                }
            }

            writer.WriteEndArray();
            writer.WriteEndObject();
        }
    }
}
