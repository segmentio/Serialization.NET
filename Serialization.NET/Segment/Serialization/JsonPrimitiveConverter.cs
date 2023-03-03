using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Segment.Serialization
{
    public class JsonPrimitiveConverter : JsonConverter<JsonPrimitive>
    {
        public override void Write(Utf8JsonWriter writer, JsonPrimitive value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                return;
            }

            writer.WriteRawValue(value.ToString());
        }

        public override JsonPrimitive Read(ref Utf8JsonReader reader, Type objectType, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Null:
                    return JsonNull.Instance;
                case JsonTokenType.True:
                    return true;
                case JsonTokenType.False:
                    return false;
                case JsonTokenType.String:
                    return JsonPrimitive.Create(reader.GetString(), true);
                default:
                    return JsonPrimitive.Create(reader.GetDouble(), false);
            }
        }
    }

    public class JsonObjectConverter : JsonConverter<JsonObject>
    {
        public override void Write(Utf8JsonWriter writer, JsonObject value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                return;
            }
            writer.WriteRawValue(value.ToString());
        }

        public override JsonObject Read(ref Utf8JsonReader reader, Type objectType, JsonSerializerOptions options)
        {
            return reader.TokenType == JsonTokenType.StartObject ? ReadObject(ref reader, options) : null;
        }

        private JsonObject ReadObject(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            var result = new JsonObject();

            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                string key = reader.GetString();
                if (key == null || reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException("Unexpected token!");
                }

                reader.Read();
                JsonElement value;
                switch (reader.TokenType)
                {
                    case JsonTokenType.StartObject:
                        value = JsonSerializer.Deserialize<JsonObject>(ref reader, options);
                        break;
                    case JsonTokenType.StartArray:
                        value = JsonSerializer.Deserialize<JsonArray>(ref reader, options);
                        break;
                    default:
                        value = JsonSerializer.Deserialize<JsonPrimitive>(ref reader, options);
                        break;
                };

                result.Add(key, value);
            }

            return result;
        }
    }

    public class JsonArrayConverter : JsonConverter<JsonArray>
    {
        public override void Write(Utf8JsonWriter writer, JsonArray value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                return;
            }
            writer.WriteRawValue(value.ToString());
        }

        public override JsonArray Read(ref Utf8JsonReader reader, Type objectType, JsonSerializerOptions options)
        {
            return reader.TokenType == JsonTokenType.StartArray ? ReadArray(ref reader, options) : null;
        }

        private JsonArray ReadArray(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            var result = new JsonArray();


            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
                JsonElement value;
                switch (reader.TokenType)
                {
                    case JsonTokenType.StartObject:
                        value = JsonSerializer.Deserialize<JsonObject>(ref reader, options);
                        break;
                    case JsonTokenType.StartArray:
                        value = JsonSerializer.Deserialize<JsonArray>(ref reader, options);
                        break;
                    default:
                        value = JsonSerializer.Deserialize<JsonPrimitive>(ref reader, options);
                        break;
                };

                result.Add(value);
            }

            return result;
        }
    }
}
