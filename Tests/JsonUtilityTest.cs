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
            "{\"int\": 1,\"float\": 1,\"long\": 1,\"double\": 1,\"string\": \"1\",\"bool\": true,\"datetime\": \"2015-12-10T04:08:31.905Z\",\"date\": \"" + s_date + "\",\"object\": {\"another object\": \"obj\"},\"array\": [1,1,1,1,\"1\",true,{\"object in array\": \"obj\"}]}";

        [Fact]
        public void Test_ToJson()
        {
            string actual = JsonUtility.ToJson(_jsonObject);
            Assert.Equal(_jsonStr, actual);
        }

        [Fact]
        public void Test_FromJson()
        {
            JsonObject actual = JsonUtility.FromJson<JsonObject>(_jsonStr);
            Assert.Equal(_jsonObject.ToString(), actual.ToString());
        }
    }
}
