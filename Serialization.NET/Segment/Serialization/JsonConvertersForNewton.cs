#if NETSTANDARD1_3

using System;
using Newtonsoft.Json;

namespace Segment.Serialization
{
    public class JsonPrimitiveConverter : JsonConverter<JsonPrimitive>
    {
        public override void WriteJson(JsonWriter writer, JsonPrimitive value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            writer.WriteRawValue(value.ToString());
        }

        public override JsonPrimitive ReadJson(JsonReader reader, Type objectType, JsonPrimitive existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return JsonNull.Instance;
            }

            var str = reader.Value.ToString();

            if (reader.Value is bool)
            {
                bool.TryParse(str, out var result);
                return result;
            }

            if (reader.Value is string)
            {
                return JsonPrimitive.Create(str, true);
            }

            return JsonPrimitive.Create(str, false);
        }
    }

    public class JsonObjectConverter : JsonConverter<JsonObject>
    {
        public override void WriteJson(JsonWriter writer, JsonObject value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            writer.WriteRawValue(value.ToString());
        }

        public override JsonObject ReadJson(JsonReader reader, Type objectType, JsonObject existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return reader.TokenType == JsonToken.StartObject ? ReadJsonObject(reader, serializer) : null;
        }

        private JsonObject ReadJsonObject(JsonReader reader, JsonSerializer serializer)
        {
            var result = new JsonObject();

            while (reader.Read() && reader.TokenType != JsonToken.EndObject)
            {
                string key = reader.Value?.ToString();
                if (key == null || reader.TokenType != JsonToken.PropertyName)
                {
                    throw new JsonException("Unexpected token!");
                }

                reader.Read();
                JsonElement value;
                switch (reader.TokenType)
                {
                    case JsonToken.StartObject:
                        value = serializer.Deserialize<JsonObject>(reader);
                        break;
                    case JsonToken.StartArray:
                        value = serializer.Deserialize<JsonArray>(reader);
                        break;
                    default:
                        value = serializer.Deserialize<JsonPrimitive>(reader);
                        break;
                };

                result.Add(key, value);
            }

            return result;
        }
    }

    public class JsonArrayConverter : JsonConverter<JsonArray>
    {
        public override void WriteJson(JsonWriter writer, JsonArray value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            writer.WriteRawValue(value.ToString());
        }

        public override JsonArray ReadJson(JsonReader reader, Type objectType, JsonArray existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return reader.TokenType == JsonToken.StartArray ? ReadJsonArray(reader, serializer) : null;
        }

        private JsonArray ReadJsonArray(JsonReader reader, JsonSerializer serializer)
        {
            var result = new JsonArray();


            while (reader.Read() && reader.TokenType != JsonToken.EndArray)
            {
                JsonElement value;
                switch (reader.TokenType)
                {
                    case JsonToken.StartObject:
                        value = serializer.Deserialize<JsonObject>(reader);
                        break;
                    case JsonToken.StartArray:
                        value = serializer.Deserialize<JsonArray>(reader);
                        break;
                    default:
                        value = serializer.Deserialize<JsonPrimitive>(reader);
                        break;
                };

                result.Add(value);
            }

            return result;
        }
    }
}

#endif
