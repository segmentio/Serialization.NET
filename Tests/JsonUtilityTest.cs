using Segment.Serialization;
using Xunit;

namespace Tests
{
    public class JsonUtilityTest
    {
        private readonly JsonObject jsonObject = new JsonObject
            {
                ["int"] = 1,
                ["float"] = 1f,
                ["long"] = 1L,
                ["double"] = 1.0,
                ["string"] = "1",
                ["bool"] = true,
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
        
        private readonly string jsonStr = 
            "{\"int\": 1,\"float\": 1,\"long\": 1,\"double\": 1,\"string\": \"1\",\"bool\": true,\"object\": {\"another object\": \"obj\"},\"array\": [1,1,1,1,\"1\",true,{\"object in array\": \"obj\"}]}";
        
        [Fact]
        public void Test_ToJson()
        {
            var actual = JsonUtility.ToJson(jsonObject);
            Assert.Equal(jsonStr, actual);
        }

        [Fact]
        public void Test_FromJson()
        {
            var actual = JsonUtility.FromJson<JsonObject>(jsonStr);
            Assert.Equal(jsonObject.ToString(), actual.ToString());
        }
    }
}