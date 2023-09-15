using System;
using System.Collections.Generic;
using Segment.Serialization;
using Xunit;

namespace Tests
{
    public class JsonObjectTest
    {
        [Fact]
        public void TestJsonObjectConcurrentAccess()
        {
            var json = new JsonObject
            {
                ["a"] = "a",
                ["b"] = "a",
                ["c"] = "a",
                ["d"] = "a",
                ["e"] = "a",
            };

            Exception exception = null;
            try
            {
                foreach (KeyValuePair<string, JsonElement> pair in json)
                {
                    json.Remove(pair.Key);
                    json["test"] = "test";
                }
            }
            catch (Exception e)
            {
                exception = e;
            }

            Assert.Null(exception);
            Assert.Single(json);
            Assert.True(json.ContainsKey("test"));
        }

        [Fact]
        public void TestJsonObjectConcurrentAccessKeys()
        {
            var json = new JsonObject
            {
                ["a"] = "a",
                ["b"] = "a",
                ["c"] = "a",
                ["d"] = "a",
                ["e"] = "a",
            };

            Exception exception = null;
            try
            {
                foreach (string key in json.Keys)
                {
                    json.Remove(key);
                    json["test"] = "test";
                }
            }
            catch (Exception e)
            {
                exception = e;
            }

            Assert.Null(exception);
            Assert.Single(json);
            Assert.True(json.ContainsKey("test"));
        }

        [Fact]
        public void TestJsonObjectConcurrentAccessValues()
        {
            var json = new JsonObject
            {
                ["a"] = "a",
                ["b"] = "a",
                ["c"] = "a",
                ["d"] = "a",
                ["e"] = "a",
            };

            Exception exception = null;
            try
            {
                foreach (string key in json.Keys)
                {
                    json[key] = "test";
                }
            }
            catch (Exception e)
            {
                exception = e;
            }

            Assert.Null(exception);
        }

        [Fact]
        public void TestJsonObjectWithCollectionConcurrentAccess()
        {
            var dict = new Dictionary<string, JsonElement>
            {
                ["a"] = "a",
                ["b"] = "a",
                ["c"] = "a",
                ["d"] = "a",
                ["e"] = "a",
            };
            var json = new JsonObject(dict);

            Exception exception = null;
            try
            {
                foreach (KeyValuePair<string, JsonElement> pair in json)
                {
                    json.Remove(pair.Key);
                    json["test"] = "test";
                }
            }
            catch (Exception e)
            {
                exception = e;
            }

            Assert.Null(exception);
            Assert.Single(json);
            Assert.True(json.ContainsKey("test"));
        }

        [Fact]
        public void TestJsonObjectWithCollectionConcurrentAccessKeys()
        {
            var dict = new Dictionary<string, JsonElement>
            {
                ["a"] = "a",
                ["b"] = "a",
                ["c"] = "a",
                ["d"] = "a",
                ["e"] = "a",
            };
            var json = new JsonObject(dict);

            Exception exception = null;
            try
            {
                foreach (string key in json.Keys)
                {
                    json.Remove(key);
                    json["test"] = "test";
                }
            }
            catch (Exception e)
            {
                exception = e;
            }

            Assert.Null(exception);
            Assert.Single(json);
            Assert.True(json.ContainsKey("test"));
        }

        [Fact]
        public void TestJsonObjectWithCollectionConcurrentAccessValues()
        {
            var dict = new Dictionary<string, JsonElement>
            {
                ["a"] = "a",
                ["b"] = "a",
                ["c"] = "a",
                ["d"] = "a",
                ["e"] = "a",
            };
            var json = new JsonObject(dict);

            Exception exception = null;
            try
            {
                foreach (string key in json.Keys)
                {
                    json[key] = "test";
                }
            }
            catch (Exception e)
            {
                exception = e;
            }

            Assert.Null(exception);
        }
    }
}
