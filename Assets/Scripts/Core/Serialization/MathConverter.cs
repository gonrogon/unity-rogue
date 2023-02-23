using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using GG.Mathe;

namespace Rogue.Core.Serialization
{
    public class MathConverter : JsonConverter
    {
        private Stack<Formatting> mFormattingBackup = new Stack<Formatting>();

        public override bool CanConvert(Type type)
        {
            if (type == typeof(Vec2i))    { return true; }
            if (type == typeof(Rect2i))   { return true; }
            if (type == typeof(Circle2i)) { return true; }

            return false;
        }

        public override object ReadJson(JsonReader reader, Type type, object existingValue, JsonSerializer serializer)
        {
            if (type == typeof(Vec2i))    { return ReadCoord (reader, serializer); }
            if (type == typeof(Rect2i))   { return ReadRect  (reader, serializer); }
            if (type == typeof(Circle2i)) { return ReadCircle(reader, serializer); }

            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            switch (value)
            {
                case Vec2i    obj: { WriteCoord (writer, serializer, obj); } break;
                case Rect2i   obj: { WriteRect  (writer, serializer, obj); } break;
                case Circle2i obj: { WriteCircle(writer, serializer, obj); } break;
            }
        }

        public object ReadCoord(JsonReader reader, JsonSerializer serializer)
        {
            var jobj = JToken.ReadFrom(reader);

            return new Vec2i(
                (int)jobj[0],
                (int)jobj[1]
            );
        }

        public void WriteCoord(JsonWriter writer, JsonSerializer serializer, Vec2i value)
        {
            writer.WriteStartArray();

            PushFormatting(writer, Formatting.None);

            writer.WriteValue(value.x);
            writer.WriteValue(value.y);
            writer.WriteEndArray();

            PopFormatting(writer);
        }

        public object ReadRect(JsonReader reader, JsonSerializer serializer)
        {
            var jobj = JToken.ReadFrom(reader);
            
            return new Rect2i(
                jobj[0].ToObject<Vec2i>(serializer),
                jobj[1].ToObject<Vec2i>(serializer)
            );
        }

        public void WriteRect(JsonWriter writer, JsonSerializer serializer, Rect2i value)
        {
            PushFormatting(writer, Formatting.None);

            writer.WriteStartArray();
            serializer.Serialize(writer, value.Min);
            serializer.Serialize(writer, value.Max);
            writer.WriteEndArray();

            PopFormatting(writer);
        }

        public object ReadCircle(JsonReader reader, JsonSerializer serializer)
        {
            var jobj = JToken.ReadFrom(reader);

            return new Circle2i(
                jobj["center"].ToObject<Vec2i>(serializer),
                jobj["radius"].ToObject<int>(serializer)
            );
        }

        public void WriteCircle(JsonWriter writer, JsonSerializer serializer, Circle2i value)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("center");
            serializer.Serialize(writer, value.center);
            writer.WritePropertyName("radius");
            writer.WriteValue(value.radius);
            writer.WriteEndObject();
        }

        // ---------
        // UTILITIES
        // ---------

        private void PushFormatting(JsonWriter writer, Formatting formatting)
        {
            mFormattingBackup.Push(writer.Formatting);
            writer.Formatting = formatting;
        }

        private void PopFormatting(JsonWriter writer)
        {
            writer.Formatting = mFormattingBackup.Pop();
        }
    }
}
