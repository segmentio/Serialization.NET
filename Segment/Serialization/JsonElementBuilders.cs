using System;
using System.Collections.Generic;

namespace Segment.Serialization
{
    public static class Builders
    {
        public static JsonObject BuildJsonObject(Action<JsonObjectBuilder> action)
        {
            var builder = new JsonObjectBuilder();
            action.Invoke(builder);
            return builder.Build();
        }

        public static JsonArray BuildJsonArray(Action<JsonArrayBuilder> action)
        {
            var builder = new JsonArrayBuilder();
            action.Invoke(builder);
            return builder.Build();
        }

        #region JsonObjectBuilder Extensions

        public static JsonObjectBuilder PutJsonObject(this JsonObjectBuilder builder, string key,
            Action<JsonObjectBuilder> action) => builder.Put(key, BuildJsonObject(action));

        public static JsonObjectBuilder PutJsonArray(this JsonObjectBuilder builder, string key, Action<JsonArrayBuilder> action) =>
            builder.Put(key, BuildJsonArray(action));

        public static JsonObjectBuilder Put(this JsonObjectBuilder builder, string key, JsonElement value) =>
            builder.Put(key, value);

        #endregion

        #region JsonArrayBuilder Extensions

        public static JsonArrayBuilder AddJsonObject(this JsonArrayBuilder builder, Action<JsonObjectBuilder> action) =>
            builder.Add(BuildJsonObject(action));

        public static JsonArrayBuilder AddJsonArray(this JsonArrayBuilder builder, Action<JsonArrayBuilder> action) =>
            builder.Add(BuildJsonArray(action));

        public static JsonArrayBuilder Add(this JsonArrayBuilder builder, JsonElement value) =>
            builder.Add(value);

        #endregion
    }

    public class JsonObjectBuilder
    {
        private readonly IDictionary<string, JsonElement> _content;

        internal JsonObjectBuilder()
        {
            _content = new Dictionary<string, JsonElement>();
        }

        public JsonObjectBuilder Put(string key, JsonElement element)
        {
            _content.Add(key, element);
            return this;
        }

        internal JsonObject Build() => new JsonObject(_content);
    }

    public class JsonArrayBuilder
    {
        private readonly List<JsonElement> _content;

        internal JsonArrayBuilder()
        {
            _content = new List<JsonElement>();
        }

        public JsonArrayBuilder Add(JsonElement element)
        {
            _content.Add(element);
            return this;
        }

        internal JsonArray Build() => new JsonArray(_content);
    }
}