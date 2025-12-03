using Segment.Serialization;
using Xunit;

namespace Tests
{
    public class NullablePrimitivesTest
    {
        private class TestModel
        {
            public bool? nullableBool;
            public bool? nullableBoolNull;
            public int? nullableInt;
            public int? nullableIntNull;
            public long? nullableLong;
            public long? nullableLongNull;
            public short? nullableShort;
            public short? nullableShortNull;
            public double? nullableDouble;
            public double? nullableDoubleNull;
            public float? nullableFloat;
            public float? nullableFloatNull;
        }

        [Fact]
        public void TestNullablePrimitivesRoundTrip()
        {
            TestModel data = new TestModel
            {
                nullableBool = true,
                nullableBoolNull = null,
                nullableInt = 42,
                nullableIntNull = null,
                nullableLong = 9223372036854775807L,
                nullableLongNull = null,
                nullableShort = 32767,
                nullableShortNull = null,
                nullableDouble = 3.14159,
                nullableDoubleNull = null,
                nullableFloat = 2.71828f,
                nullableFloatNull = null
            };

            JsonObject json = new JsonObject
            {
                { "nullableBool", data.nullableBool },
                { "nullableBoolNull", data.nullableBoolNull },
                { "nullableInt", data.nullableInt },
                { "nullableIntNull", data.nullableIntNull },
                { "nullableLong", data.nullableLong },
                { "nullableLongNull", data.nullableLongNull },
                { "nullableShort", data.nullableShort },
                { "nullableShortNull", data.nullableShortNull },
                { "nullableDouble", data.nullableDouble },
                { "nullableDoubleNull", data.nullableDoubleNull },
                { "nullableFloat", data.nullableFloat },
                { "nullableFloatNull", data.nullableFloatNull }
            };

            string stringified = JsonUtility.ToJson(json);

            // Verify serialization
            Assert.Contains("\"nullableBool\": true", stringified);
            Assert.Contains("\"nullableInt\": 42", stringified);
            Assert.Contains("\"nullableLong\": 9223372036854775807", stringified);
            Assert.Contains("\"nullableShort\": 32767", stringified);
            Assert.Contains("\"nullableDouble\": 3.14159", stringified);
            Assert.Contains("\"nullableFloat\": 2.71828", stringified);
            Assert.Contains("\"nullableBoolNull\": null", stringified);
            Assert.Contains("\"nullableIntNull\": null", stringified);
            Assert.Contains("\"nullableLongNull\": null", stringified);
            Assert.Contains("\"nullableShortNull\": null", stringified);
            Assert.Contains("\"nullableDoubleNull\": null", stringified);
            Assert.Contains("\"nullableFloatNull\": null", stringified);

            // Verify deserialization round-trip
            TestModel deserialized = JsonUtility.FromJson<TestModel>(stringified);

            Assert.NotNull(deserialized);
            Assert.Equal(data.nullableBool, deserialized.nullableBool);
            Assert.Null(deserialized.nullableBoolNull);
            Assert.Equal(data.nullableInt, deserialized.nullableInt);
            Assert.Null(deserialized.nullableIntNull);
            Assert.Equal(data.nullableLong, deserialized.nullableLong);
            Assert.Null(deserialized.nullableLongNull);
            Assert.Equal(data.nullableShort, deserialized.nullableShort);
            Assert.Null(deserialized.nullableShortNull);
            Assert.Equal(data.nullableDouble, deserialized.nullableDouble);
            Assert.Null(deserialized.nullableDoubleNull);
            Assert.Equal(data.nullableFloat, deserialized.nullableFloat);
            Assert.Null(deserialized.nullableFloatNull);
        }

        [Fact]
        public void TestNullableBoolRoundTrip()
        {
            bool? trueValue = true;
            bool? falseValue = false;
            bool? nullValue = null;

            JsonObject json = new JsonObject
            {
                { "trueValue", trueValue },
                { "falseValue", falseValue },
                { "nullValue", nullValue }
            };

            string stringified = JsonUtility.ToJson(json);

            // Verify serialization
            Assert.Contains("\"trueValue\": true", stringified);
            Assert.Contains("\"falseValue\": false", stringified);
            Assert.Contains("\"nullValue\": null", stringified);

            // Deserialize and verify round-trip
            var deserialized = JsonUtility.FromJson<BoolModel>(stringified);
            Assert.NotNull(deserialized);
            Assert.Equal(true, deserialized.trueValue);
            Assert.Equal(false, deserialized.falseValue);
            Assert.Null(deserialized.nullValue);
        }

        private class BoolModel
        {
            public bool? trueValue;
            public bool? falseValue;
            public bool? nullValue;
        }

        [Fact]
        public void TestNullableIntRoundTrip()
        {
            int? value = 42;
            int? nullValue = null;
            int? negativeValue = -123;

            JsonObject json = new JsonObject
            {
                { "value", value },
                { "nullValue", nullValue },
                { "negativeValue", negativeValue }
            };

            string stringified = JsonUtility.ToJson(json);

            Assert.Contains("\"value\": 42", stringified);
            Assert.Contains("\"nullValue\": null", stringified);
            Assert.Contains("\"negativeValue\": -123", stringified);

            // Deserialize and verify
            var deserialized = JsonUtility.FromJson<IntModel>(stringified);
            Assert.Equal(42, deserialized.value);
            Assert.Null(deserialized.nullValue);
            Assert.Equal(-123, deserialized.negativeValue);
        }

        private class IntModel
        {
            public int? value;
            public int? nullValue;
            public int? negativeValue;
        }

        [Fact]
        public void TestNullableLongRoundTrip()
        {
            long? maxValue = long.MaxValue;
            long? minValue = long.MinValue;
            long? nullValue = null;

            JsonObject json = new JsonObject
            {
                { "maxValue", maxValue },
                { "minValue", minValue },
                { "nullValue", nullValue }
            };

            string stringified = JsonUtility.ToJson(json);

            Assert.Contains($"\"maxValue\": {long.MaxValue}", stringified);
            Assert.Contains($"\"minValue\": {long.MinValue}", stringified);
            Assert.Contains("\"nullValue\": null", stringified);

            // Deserialize and verify
            var deserialized = JsonUtility.FromJson<LongModel>(stringified);
            Assert.Equal(long.MaxValue, deserialized.maxValue);
            Assert.Equal(long.MinValue, deserialized.minValue);
            Assert.Null(deserialized.nullValue);
        }

        private class LongModel
        {
            public long? maxValue;
            public long? minValue;
            public long? nullValue;
        }

        [Fact]
        public void TestNullableShortRoundTrip()
        {
            short? maxValue = short.MaxValue;
            short? minValue = short.MinValue;
            short? nullValue = null;

            JsonObject json = new JsonObject
            {
                { "maxValue", maxValue },
                { "minValue", minValue },
                { "nullValue", nullValue }
            };

            string stringified = JsonUtility.ToJson(json);

            Assert.Contains($"\"maxValue\": {short.MaxValue}", stringified);
            Assert.Contains($"\"minValue\": {short.MinValue}", stringified);
            Assert.Contains("\"nullValue\": null", stringified);

            // Deserialize and verify
            var deserialized = JsonUtility.FromJson<ShortModel>(stringified);
            Assert.Equal(short.MaxValue, deserialized.maxValue);
            Assert.Equal(short.MinValue, deserialized.minValue);
            Assert.Null(deserialized.nullValue);
        }

        private class ShortModel
        {
            public short? maxValue;
            public short? minValue;
            public short? nullValue;
        }

        [Fact]
        public void TestNullableDoubleRoundTrip()
        {
            double? value = 3.14159;
            double? nullValue = null;
            double? negativeValue = -2.71828;

            JsonObject json = new JsonObject
            {
                { "value", value },
                { "nullValue", nullValue },
                { "negativeValue", negativeValue }
            };

            string stringified = JsonUtility.ToJson(json);

            Assert.Contains("\"value\": 3.14159", stringified);
            Assert.Contains("\"nullValue\": null", stringified);
            Assert.Contains("\"negativeValue\": -2.71828", stringified);

            // Deserialize and verify
            var deserialized = JsonUtility.FromJson<DoubleModel>(stringified);
            Assert.Equal(3.14159, deserialized.value);
            Assert.Null(deserialized.nullValue);
            Assert.Equal(-2.71828, deserialized.negativeValue);
        }

        private class DoubleModel
        {
            public double? value;
            public double? nullValue;
            public double? negativeValue;
        }

        [Fact]
        public void TestNullableFloatRoundTrip()
        {
            float? value = 2.71828f;
            float? nullValue = null;
            float? negativeValue = -1.41421f;

            JsonObject json = new JsonObject
            {
                { "value", value },
                { "nullValue", nullValue },
                { "negativeValue", negativeValue }
            };

            string stringified = JsonUtility.ToJson(json);

            Assert.Contains("\"value\": 2.71828", stringified);
            Assert.Contains("\"nullValue\": null", stringified);
            Assert.Contains("\"negativeValue\": -1.41421", stringified);

            // Deserialize and verify
            var deserialized = JsonUtility.FromJson<FloatModel>(stringified);
            Assert.Equal(2.71828f, deserialized.value);
            Assert.Null(deserialized.nullValue);
            Assert.Equal(-1.41421f, deserialized.negativeValue);
        }

        private class FloatModel
        {
            public float? value;
            public float? nullValue;
            public float? negativeValue;
        }

        [Fact]
        public void TestNullablePrimitivesInArray()
        {
            int? value1 = 1;
            int? nullValue = null;
            int? value3 = 3;

            var array = new JsonArray
            {
                value1,
                nullValue,
                value3
            };

            string stringified = JsonUtility.ToJson(array);

            Assert.Equal("[1,null,3]", stringified);
        }

        [Fact]
        public void TestMixedNullablePrimitivesRoundTrip()
        {
            bool? boolValue = true;
            int? intValue = 42;
            long? longValue = 9223372036854775807L;
            short? shortValue = 32767;
            double? doubleValue = 3.14159;
            float? floatValue = 2.71828f;
            bool? nullBool = null;
            int? nullInt = null;

            JsonObject json = new JsonObject
            {
                { "boolValue", boolValue },
                { "intValue", intValue },
                { "longValue", longValue },
                { "shortValue", shortValue },
                { "doubleValue", doubleValue },
                { "floatValue", floatValue },
                { "nullBool", nullBool },
                { "nullInt", nullInt }
            };

            string stringified = JsonUtility.ToJson(json);

            Assert.Contains("\"boolValue\": true", stringified);
            Assert.Contains("\"intValue\": 42", stringified);
            Assert.Contains("\"longValue\": 9223372036854775807", stringified);
            Assert.Contains("\"shortValue\": 32767", stringified);
            Assert.Contains("\"doubleValue\": 3.14159", stringified);
            Assert.Contains("\"floatValue\": 2.71828", stringified);
            Assert.Contains("\"nullBool\": null", stringified);
            Assert.Contains("\"nullInt\": null", stringified);

            // Deserialize and verify
            var deserialized = JsonUtility.FromJson<MixedModel>(stringified);
            Assert.Equal(true, deserialized.boolValue);
            Assert.Equal(42, deserialized.intValue);
            Assert.Equal(9223372036854775807L, deserialized.longValue);
            Assert.Equal((short)32767, deserialized.shortValue);
            Assert.Equal(3.14159, deserialized.doubleValue);
            Assert.Equal(2.71828f, deserialized.floatValue);
            Assert.Null(deserialized.nullBool);
            Assert.Null(deserialized.nullInt);
        }

        private class MixedModel
        {
            public bool? boolValue;
            public int? intValue;
            public long? longValue;
            public short? shortValue;
            public double? doubleValue;
            public float? floatValue;
            public bool? nullBool;
            public int? nullInt;
        }
    }
}
