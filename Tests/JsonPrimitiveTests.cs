using System.Collections.Generic;
using Segment.Serialization;
using Xunit;

namespace Tests
{
    public class JsonPrimitiveTests
    {
        [Fact]
        public void EnsureIntAreNotSerializedWithScientificNotation()
        {
            var dict =  new Dictionary<string, object>
            {
                {"IntNumber", int.MaxValue.ToString()}
            };

            var serialized = JsonUtility.ToJson(dict); // This call exist inside Segment.Analytics.Analytics::Track<T>
            var deserialized = JsonUtility.FromJson<JsonObject>(serialized); // This call exist inside Segment.Analytics.Analytics::Track<T>
            Assert.Equal(expected: int.MaxValue.ToString(), actual: deserialized.GetString("IntNumber"));
        }

        [Fact]
        public void EnsureLongAreNotSerializedWithScientificNotation()
        {
            var dict =  new Dictionary<string, object>
            {
                {"LongNumber", long.MaxValue.ToString()}
            };

            var serialized = JsonUtility.ToJson(dict); // This call exist inside Segment.Analytics.Analytics::Track<T>
            var deserialized = JsonUtility.FromJson<JsonObject>(serialized); // This call exist inside Segment.Analytics.Analytics::Track<T>
            Assert.Equal(expected: long.MaxValue.ToString(), actual: deserialized.GetString("LongNumber"));
        }
    }
}
