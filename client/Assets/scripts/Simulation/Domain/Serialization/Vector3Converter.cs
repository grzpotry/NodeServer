using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Simulation.Domain.Serialization
{
    /// <summary>
    /// Converter for <see cref="Vector3"/>
    /// </summary>
    public class Vector3Converter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Vector3)
                   || objectType == typeof(Vector3?);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            if (reader.TokenType != JsonToken.StartArray)
            {
                throw new JsonReaderException("Expected StartArray or null, got: " + reader.TokenType);
            }

            var v = new Vector3();
            var x = reader.ReadAsDouble();
            v.x = (float)x.Value;

            var y = reader.ReadAsDouble();
            v.y = (float)y.Value;

            var z = reader.ReadAsDouble();
            v.z = (float)z.Value;

            reader.Read();

            if (reader.TokenType != JsonToken.EndArray)
            {
                throw new JsonReaderException("Expected EndArray");
            }

            return v;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var v = (Vector3)value;

            var prevFormatter = writer.Formatting;
            writer.Formatting = Formatting.None;

            writer.WriteStartArray();
            writer.WriteValue(v.x);
            writer.WriteValue(v.y);
            writer.WriteValue(v.z);
            writer.WriteEndArray();

            writer.Formatting = prevFormatter;
        }
    }
}