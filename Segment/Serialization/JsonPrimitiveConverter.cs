using System;
using Newtonsoft.Json;

namespace Segment.Serialization
{
    public class JsonPrimitiveConverter : JsonConverter<JsonPrimitive>
    {
        public override void WriteJson(JsonWriter writer, JsonPrimitive? value, JsonSerializer serializer)
        {
            if (value == null)
            {
                return;
            }

            writer.WriteRawValue(value.ToString());
        }

        public override JsonPrimitive? ReadJson(JsonReader reader, Type objectType, JsonPrimitive? existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return JsonNull.instance;
            }

            var str = reader.Value.ToString();

            return bool.TryParse(str, out var v) ? v : JsonPrimitive.Create(str, !double.TryParse(str, out _));
        }
    }

    public class JsonObjectConverter : JsonConverter<JsonObject>
    {
        public override void WriteJson(JsonWriter writer, JsonObject? value, JsonSerializer serializer)
        {
            if (value == null)
            {
                return;
            }
            writer.WriteRawValue(value.ToString());
        }

        public override JsonObject? ReadJson(JsonReader reader, Type objectType, JsonObject? existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            return reader.TokenType == JsonToken.StartObject ? ReadJsonObject(reader, serializer) : null;
        }

        private JsonObject ReadJsonObject(JsonReader reader, JsonSerializer serializer)
        {
            var result = new JsonObject();

            reader.Read();
            while (reader.TokenType != JsonToken.EndObject)
            {
                var key = reader.Value?.ToString();
                if (key == null || reader.TokenType != JsonToken.PropertyName)
                {
                    throw new JsonSerializationException("Unexpected token!");
                }

                reader.Read();
                JsonElement value = reader.TokenType switch
                {
                    JsonToken.StartObject => serializer.Deserialize<JsonObject>(reader),
                    JsonToken.StartArray => serializer.Deserialize<JsonArray>(reader),
                    _ => serializer.Deserialize<JsonPrimitive>(reader)
                };

                result.Add(key, value);
                reader.Read();
            }

            return result;
        }
    }

    public class JsonArrayConverter : JsonConverter<JsonArray>
    {
        public override void WriteJson(JsonWriter writer, JsonArray? value, JsonSerializer serializer)
        {
            if (value == null)
            {
                return;
            }
            writer.WriteRawValue(value.ToString());
        }

        public override JsonArray? ReadJson(JsonReader reader, Type objectType, JsonArray? existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            return reader.TokenType == JsonToken.StartArray ? ReadJsonArray(reader, serializer) : null;
        }

        private JsonArray ReadJsonArray(JsonReader reader, JsonSerializer serializer)
        {
            var result = new JsonArray();

            reader.Read();
            while (reader.TokenType != JsonToken.EndArray)
            {
                JsonElement value = reader.TokenType switch
                {
                    JsonToken.StartObject => serializer.Deserialize<JsonObject>(reader),
                    JsonToken.StartArray => serializer.Deserialize<JsonArray>(reader),
                    _ => serializer.Deserialize<JsonPrimitive>(reader)
                };

                result.Add(value);
                reader.Read();
            }

            return result;
        }
    }
}