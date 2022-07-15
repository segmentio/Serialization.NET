using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

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
        public abstract bool isString { get; }

        public abstract string content { get; }

        public override string ToString() => content;

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
        public override bool isString { get; }

        public override string content { get; }

        internal JsonLiteral(object body, bool isString)
        {
            this.isString = isString;
            this.content = body.ToString();
        }

        public override string ToString()
        {
            return isString ? JsonUtility.PrintQuoted(content) : content;
        }

        public override bool Equals(object other)
        {
            if (this == other)
            {
                return true;
            }

            if (other == null || this.GetType() != other.GetType())
            {
                return false;
            }

            var literal = other as JsonLiteral;
            return isString == literal.isString && content.Equals(literal.content);
        }

        public override int GetHashCode()
        {
            var result = isString.GetHashCode();
            // 31 is an arbitrary number and shouldn't be changed.
            result = 31 * result + content.GetHashCode();
            return result;
        }
    }

    public sealed class JsonNull : JsonPrimitive
    {
        public override bool isString => false;
        public override string content => "null";

        private static JsonNull _instance;

        public static JsonNull Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new JsonNull();
                }

                return _instance;
            }
        }

        private JsonNull()
        {

        }
    }

    [JsonConverter(typeof(JsonObjectConverter))]
    public class JsonObject : JsonElement, IEnumerable
    {
        public IDictionary<string, JsonElement> content { get; }

        public ICollection<string> keys => content.Keys;

        public ICollection<JsonElement> values => content.Values;

        public int count => content.Count;

        public JsonObject()
        {
            content = new Dictionary<string, JsonElement>();
        }

        public JsonObject(IDictionary<string, JsonElement>? content)
        {
            this.content = content == null ?
                new Dictionary<string, JsonElement>() :
                new Dictionary<string, JsonElement>(content);
        }

        public override string ToString()
        {
            var entries = content.Select(d =>
                $"{JsonUtility.PrintQuoted(d.Key)}: {d.Value}");
            return "{" + string.Join(",", entries) + "}";
        }

        public IEnumerator<KeyValuePair<string, JsonElement>> GetEnumerator() => content.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<string, JsonElement> item) => content.Add(item);

        public void Clear() => content.Clear();

        public bool Contains(KeyValuePair<string, JsonElement> item) => content.Contains(item);

        public void Add(string key, JsonElement value) => content.Add(key, value);

        public bool ContainsKey(string key) => content.ContainsKey(key);

        public bool Remove(string key) => content.Remove(key);

        public bool TryGetValue(string key, out JsonElement value) => content.TryGetValue(key, out value);

        public JsonElement this[string key]
        {
            get => content[key];
            set => content[key] = value;
        }
    }

    [JsonConverter(typeof(JsonArrayConverter))]
    public class JsonArray : JsonElement, IEnumerable
    {
        public IList<JsonElement> content { get; }

        public int Count => content.Count;

        public bool IsReadOnly => content.IsReadOnly;

        public JsonArray()
        {
            content = new List<JsonElement>();
        }

        public JsonArray(IList<JsonElement> content)
        {
            this.content = content;
        }

        public IEnumerator<JsonElement> GetEnumerator() => content.GetEnumerator();

        public override string ToString()
        {
            return "[" + string.Join(",", content) + "]";
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(JsonElement item) => content.Add(item);

        public void Clear() => content.Clear();

        public bool Contains(JsonElement item) => content.Contains(item);

        public void CopyTo(JsonElement[] array, int arrayIndex) => content.CopyTo(array, arrayIndex);

        public bool Remove(JsonElement item) => content.Remove(item);
        public int IndexOf(JsonElement item) => content.IndexOf(item);

        public void Insert(int index, JsonElement item) => content.Insert(index, item);

        public void RemoveAt(int index) => content.RemoveAt(index);

        public JsonElement this[int index]
        {
            get => content[index];
            set => content[index] = value;
        }
    }

}