using Newtonsoft.Json.Linq;
using Segment.Serialization;
using Xunit;

namespace Tests
{
    public class JsonUtilityTest
    {
        private static readonly string s_date = System.DateTime.UtcNow.ToString();
        private readonly JsonObject _jsonObject = new JsonObject
        {
            ["int"] = 1,
            ["float"] = 1f,
            ["long"] = 1L,
            ["double"] = 1.0,
            ["string"] = "1",
            ["bool"] = true,
            ["datetime"] = "2015-12-10T04:08:31.905Z",
            ["date"] = s_date,
            ["null"] = null,
            ["object"] = new JsonObject
            {
                ["another object"] = "obj"
            },
            ["array"] = new JsonArray
                {
                    1, 1f, 1L, 1.0, "1", true, new JsonObject
                    {
                        ["object in array"] = "obj"
                    }
                }
        };

        private readonly string _jsonStr =
            "{\"int\": 1,\"float\": 1,\"long\": 1,\"double\": 1,\"string\": \"1\",\"bool\": true,\"datetime\": \"2015-12-10T04:08:31.905Z\",\"date\": \"" + s_date + "\",\"null\": null,\"object\": {\"another object\": \"obj\"},\"array\": [1,1,1,1,\"1\",true,{\"object in array\": \"obj\"}]}";

        [Fact]
        public void Test_ToJson()
        {
            string actual = JsonUtility.ToJson(_jsonObject);

            JsonObject expected = JsonUtility.FromJson<JsonObject>(_jsonStr);
            Assert.Equal(JsonUtility.ToJson(expected), actual);
        }

        [Fact]
        public void Test_FromJson()
        {
            JsonObject actual = JsonUtility.FromJson<JsonObject>(_jsonStr);
            Assert.Equal(_jsonObject.ToString(), actual.ToString());
        }

        [Fact]
        public void Test_ToJson_Camel_Case()
        {
            var foo = new Foo();
            string actual = JsonUtility.ToJson(foo);
            Assert.True(JsonEquals(foo.ToString(), actual));
        }

        [Fact]
        public void Test_ToJson_Polymorphic()
        {
            var bar = new Bar();
            Foo foo = bar;
            string actual = JsonUtility.ToJson(foo);
            Assert.True(JsonEquals(bar.ToString(), actual));
        }

        [Fact]
        public void Test_ToJson_Only_Property()
        {
            var foo = new Foo();
            string actual = JsonUtility.ToJson(foo);
            Assert.Contains(foo.PropertyFoo, actual);
            Assert.DoesNotContain("privateFieldFoo", actual);
        }

        [Fact]
        public void Test_FromJson_T()
        {
            var actual = new Foo();
            Foo expected = JsonUtility.FromJson<Foo>(actual.ToString());
            Assert.Equal(expected.ToString(), actual.ToString());
        }

        [Fact]
        public void Test_FromJson_Nested_Objects()
        {
            string settingsStr =
                "{\"integrations\":{\"Segment.io\":{\"apiKey\":\"qwerty\"}},\"plan\":{},\"edgeFunctions\":{}}";
            Settings settings = JsonUtility.FromJson<Settings>(settingsStr);
            Assert.NotNull(settings.Integrations);
            Assert.NotNull(settings.Integrations["Segment.io"]);
            Assert.Equal("qwerty", settings.Integrations.GetJsonObject("Segment.io").GetString("apiKey"));
            Assert.Equal(new JsonObject(), settings.Plan);
            Assert.Equal(new JsonObject(), settings.EdgeFunctions);
        }

        private bool JsonEquals(string expect, string actual)
        {
            var jsonExpect = JObject.Parse(expect);
            var jsonActual = JObject.Parse(actual);

            return JToken.DeepEquals(jsonExpect, jsonActual);
        }
    }
}
