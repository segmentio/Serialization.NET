using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

#if NETSTANDARD2_0
using System.Text.Json.Serialization;
#else
using Newtonsoft.Json;
#endif

namespace Segment.Serialization
{
    public abstract class JsonElement
    {
        public static implicit operator JsonElement(bool value) => new JsonLiteral(value.ToString().ToLower(), false);

        public static implicit operator JsonElement(string value)
        {
            if (value == null) return JsonNull.Instance;
            return new JsonLiteral(value, true);
        }

        public static implicit operator JsonElement(int value) => new JsonLiteral(value, false);

        public static implicit operator JsonElement(long value) => new JsonLiteral(value, false);

        public static implicit operator JsonElement(short value) => new JsonLiteral(value, false);

        public static implicit operator JsonElement(double value) => new JsonLiteral(value, false);

        public static implicit operator JsonElement(float value) => new JsonLiteral(value, false);
    }

    [JsonConverter(typeof(JsonPrimitiveConverter))]
    public abstract class JsonPrimitive : JsonElement
    {
        public abstract bool IsString { get; }

        public abstract string Content { get; }

        public override string ToString() => Content;

        public static implicit operator JsonPrimitive(bool value) => new JsonLiteral(value.ToString().ToLower(), false);

        public static implicit operator JsonPrimitive(string value)
        {
            if (value == null) return JsonNull.Instance;
            return new JsonLiteral(value, true);
        }

        public static implicit operator JsonPrimitive(int value) => new JsonLiteral(value, false);

        public static implicit operator JsonPrimitive(long value) => new JsonLiteral(value, false);

        public static implicit operator JsonPrimitive(short value) => new JsonLiteral(value, false);

        public static implicit operator JsonPrimitive(double value) => new JsonLiteral(value, false);

        public static implicit operator JsonPrimitive(float value) => new JsonLiteral(value, false);

        public static JsonPrimitive Create(object value, bool isString)
        {
            return new JsonLiteral(value, isString);
        }
    }

    internal class JsonLiteral : JsonPrimitive
    {
        public override bool IsString { get; }

        public override string Content { get; }

        internal JsonLiteral(object body, bool isString)
        {
            IsString = isString;
            Content = body.ToString();
        }

        public override string ToString()
        {
            return IsString ? JsonUtility.PrintQuoted(Content) : Content;
        }

        public override bool Equals(object other)
        {
            if (this == other)
            {
                return true;
            }

            if (other == null || GetType() != other.GetType())
            {
                return false;
            }

            var literal = other as JsonLiteral;
            return IsString == literal.IsString && Content.Equals(literal.Content);
        }

        public override int GetHashCode()
        {
            int result = IsString.GetHashCode();
            // 31 is an arbitrary number and shouldn't be changed.
            result = 31 * result + Content.GetHashCode();
            return result;
        }
    }

    public sealed class JsonNull : JsonPrimitive
    {
        public override bool IsString => false;
        public override string Content => "null";

        private static JsonNull s_instance;

        public static JsonNull Instance
        {
            get
            {
                if (s_instance == null)
                {
                    s_instance = new JsonNull();
                }

                return s_instance;
            }
        }

        private JsonNull()
        {

        }
    }

    [JsonConverter(typeof(JsonObjectConverter))]
    public class JsonObject : JsonElement, IEnumerable
    {
        public IDictionary<string, JsonElement> Content { get; }

        public ICollection<string> Keys => Content.Keys;

        public ICollection<JsonElement> Values => Content.Values;

        public int Count => Content.Count;

        public JsonObject()
        {
            Content = new ConcurrentDictionary<string, JsonElement>();
        }

        public JsonObject(IDictionary<string, JsonElement> content)
        {
            Content = content == null ?
                new ConcurrentDictionary<string, JsonElement>() :
                new ConcurrentDictionary<string, JsonElement>(content);
        }

        public override string ToString()
        {
            IEnumerable<string> entries = Content.Select(d =>
                $"{JsonUtility.PrintQuoted(d.Key)}: {d.Value}");
            return "{" + string.Join(",", entries) + "}";
        }

        public IEnumerator<KeyValuePair<string, JsonElement>> GetEnumerator() => Content.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<string, JsonElement> item) => Content.Add(item);

        public void Clear() => Content.Clear();

        public bool Contains(KeyValuePair<string, JsonElement> item) => Content.Contains(item);

        public void Add(string key, JsonElement value) => Content.Add(key, value);

        public bool ContainsKey(string key) => Content.ContainsKey(key);

        public bool Remove(string key) => Content.Remove(key);

        public bool TryGetValue(string key, out JsonElement value) => Content.TryGetValue(key, out value);

        public JsonElement this[string key]
        {
            get => Content[key];
            set => Content[key] = value;
        }
    }

    [JsonConverter(typeof(JsonArrayConverter))]
    public class JsonArray : JsonElement, IEnumerable
    {
        public IList<JsonElement> Content { get; }

        public int Count => Content.Count;

        public bool IsReadOnly => Content.IsReadOnly;

        public JsonArray()
        {
            Content = new ConcurrentList<JsonElement>();
        }

        public JsonArray(IList<JsonElement> content)
        {
            Content = new ConcurrentList<JsonElement>(content);
        }

        public IEnumerator<JsonElement> GetEnumerator() => Content.GetEnumerator();

        public override string ToString()
        {
            return "[" + string.Join(",", Content) + "]";
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(JsonElement item) => Content.Add(item);

        public void Clear() => Content.Clear();

        public bool Contains(JsonElement item) => Content.Contains(item);

        public void CopyTo(JsonElement[] array, int arrayIndex) => Content.CopyTo(array, arrayIndex);

        public bool Remove(JsonElement item) => Content.Remove(item);
        public int IndexOf(JsonElement item) => Content.IndexOf(item);

        public void Insert(int index, JsonElement item) => Content.Insert(index, item);

        public void RemoveAt(int index) => Content.RemoveAt(index);

        public JsonElement this[int index]
        {
            get => Content[index];
            set => Content[index] = value;
        }
    }

}
