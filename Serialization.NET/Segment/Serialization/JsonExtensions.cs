using System;

namespace Segment.Serialization
{
    public static class JsonExtensions
    {
        #region JsonElement Extensions

        public static JsonPrimitive ToJsonPrimitive(this JsonElement element)
        {
            if (element is JsonPrimitive primitive)
            {
                return primitive;
            }

            throw element.Error("JsonPrimitive");
        }

        public static JsonObject ToJsonObject(this JsonElement element)
        {
            if (element is JsonObject obj)
            {
                return obj;
            }

            throw element.Error("JsonObject");
        }

        public static JsonArray ToJsonArray(this JsonElement element)
        {
            if (element is JsonArray array)
            {
                return array;
            }

            throw element.Error("JsonArray");
        }

        public static JsonNull ToJsonNull(this JsonElement element)
        {
            if (element is JsonNull nil)
            {
                return nil;
            }

            throw element.Error("JsonNull");
        }

        private static Exception Error(this JsonElement element, string type)
        {
            return new Exception($"Element {element.GetType()} is not a {type}");
        }

        #endregion

        #region JsonPrimitive Extensions

        public static int ToInt(this JsonPrimitive element)
        {
            return int.Parse(element.Content);
        }

        public static int? ToIntOrNull(this JsonPrimitive element)
        {
            if (int.TryParse(element.Content, out int ret))
            {
                return ret;
            }

            return null;
        }

        public static long ToLong(this JsonPrimitive element)
        {
            return long.Parse(element.Content);
        }

        public static long? ToLongOrNull(this JsonPrimitive element)
        {
            if (long.TryParse(element.Content, out long ret))
            {
                return ret;
            }

            return null;
        }

        public static double ToDouble(this JsonPrimitive element)
        {
            return double.Parse(element.Content);
        }

        public static double? ToDoubleOrNull(this JsonPrimitive element)
        {
            if (double.TryParse(element.Content, out double ret))
            {
                return ret;
            }

            return null;
        }

        public static float ToFloat(this JsonPrimitive element)
        {
            return float.Parse(element.Content);
        }

        public static float? ToFloatOrNull(this JsonPrimitive element)
        {
            if (float.TryParse(element.Content, out float ret))
            {
                return ret;
            }

            return null;
        }

        public static bool ToBool(this JsonPrimitive element)
        {
            return bool.Parse(element.Content);
        }

        public static bool? ToBoolOrNull(this JsonPrimitive element)
        {
            if (bool.TryParse(element.Content, out bool ret))
            {
                return ret;
            }

            return null;
        }

        public static string ToContentOrNull(this JsonPrimitive element)
        {
            return element is JsonNull ? null : element.Content;
        }

        #endregion

        #region JsonObject Extensions

        public static int GetInt(this JsonObject jsonObject, string key, int defaultValue = default)
        {
            return jsonObject.ContainsKey(key) ? jsonObject[key].ToJsonPrimitive().ToInt() : defaultValue;
        }

        public static long GetLong(this JsonObject jsonObject, string key, long defaultValue = default)
        {
            return jsonObject.ContainsKey(key) ? jsonObject[key].ToJsonPrimitive().ToLong() : defaultValue;
        }

        public static double GetDouble(this JsonObject jsonObject, string key, double defaultValue = default)
        {
            return jsonObject.ContainsKey(key) ? jsonObject[key].ToJsonPrimitive().ToDouble() : defaultValue;
        }

        public static float GetInt(this JsonObject jsonObject, string key, float defaultValue = default)
        {
            return jsonObject.ContainsKey(key) ? jsonObject[key].ToJsonPrimitive().ToFloat() : defaultValue;
        }

        public static bool GetBool(this JsonObject jsonObject, string key, bool defaultValue = default)
        {
            return jsonObject.ContainsKey(key) ? jsonObject[key].ToJsonPrimitive().ToBool() : defaultValue;
        }

        public static string GetString(this JsonObject jsonObject, string key, string defaultValue = default)
        {
            return jsonObject.ContainsKey(key) ? jsonObject[key].ToJsonPrimitive().Content : defaultValue;
        }

        public static JsonObject GetJsonObject(this JsonObject jsonObject, string key,
            JsonObject defaultValue = default)
        {
            return jsonObject.ContainsKey(key) ? jsonObject[key].ToJsonObject() : defaultValue;
        }

        public static JsonArray GetJsonArray(this JsonObject jsonObject, string key,
            JsonArray defaultValue = default)
        {
            return jsonObject.ContainsKey(key) ? jsonObject[key].ToJsonArray() : defaultValue;
        }

        #endregion
    }
}
