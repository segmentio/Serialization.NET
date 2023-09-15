using System;
using System.Collections.Generic;
using Segment.Serialization;
using Xunit;

namespace Tests
{
    public class JsonArrayTest
    {
        [Fact]
        public void TestJsonArrayConcurrentModified()
        {
            var json = new JsonArray
            {
                "a", "b", "c", "d", "e"
            };

            Exception exception = null;
            try
            {
                int i = 0;
                foreach (JsonElement element in json)
                {
                    json[i++] = "test";
                }
            }
            catch (Exception e)
            {
                exception = e;
            }

            Assert.Null(exception);
            Assert.Equal(5, json.Count);
            foreach (JsonElement element in json)
            {
                Assert.Equal("test", element);
            }
        }

        [Fact]
        public void TestJsonArrayConcurrentAccess()
        {
            var json = new JsonArray
            {
                "a", "b", "c", "d", "e"
            };

            Exception exception = null;
            try
            {
                foreach (JsonElement element in json)
                {
                    json.Add("test");
                    json.Remove(element);
                }
            }
            catch (Exception e)
            {
                exception = e;
            }

            Assert.Null(exception);
            Assert.Equal(5, json.Count);
            foreach (JsonElement element in json)
            {
                Assert.Equal("test", element);
            }
        }
        [Fact]
        public void TestJsonArrayWithCollectionConcurrentModified()
        {
            var list = new List<JsonElement>
            {
                "a",
                "b",
                "c",
                "d",
                "e"
            };
            var json = new JsonArray(list);

            Exception exception = null;
            try
            {
                int i = 0;
                foreach (JsonElement element in json)
                {
                    json[i++] = "test";
                }
            }
            catch (Exception e)
            {
                exception = e;
            }

            Assert.Null(exception);
            Assert.Equal(5, json.Count);
            foreach (JsonElement element in json)
            {
                Assert.Equal("test", element);
            }
        }

        [Fact]
        public void TestJsonArrayWithCollectionConcurrentAccess()
        {
            var list = new List<JsonElement>
            {
                "a",
                "b",
                "c",
                "d",
                "e"
            };
            var json = new JsonArray(list);

            Exception exception = null;
            try
            {
                foreach (JsonElement element in json)
                {
                    json.Add("test");
                    json.Remove(element);
                }
            }
            catch (Exception e)
            {
                exception = e;
            }

            Assert.Null(exception);
            Assert.Equal(5, json.Count);
            foreach (JsonElement element in json)
            {
                Assert.Equal("test", element);
            }
        }
    }
}
