using System.Collections.Generic;
using System.Globalization;
using Segment.Serialization;
using Xunit;

namespace Tests
{
    public class VersionSerializationTests
    {
        [Fact]
        public static void TestLongVersionNumberSerializationForAllCultures()
        {
            const string longVersion = "1.2.3.02601051535";
            var eventParameters = new Dictionary<string, object> { { "Version", longVersion } };

            foreach (var cultureInfo in CultureInfo.GetCultures(CultureTypes.AllCultures))
            {
                using (new CurrentCultureScope(cultureInfo))
                {
                    var json = JsonUtility.ToJson(eventParameters);
                    var jsonObject = JsonUtility.FromJson<JsonObject>(json);
                    Assert.Equal(expected: longVersion, actual: jsonObject.GetString("Version"));
                }
            }
        }

        [Fact]
        public static void TestShortVersionNumberSerializationForAllCultures()
        {
            const string shortVersion = "1.2.3.4"; // The same as 'new Version(1, 2, 3, 4).ToString()'
            var eventParameters = new Dictionary<string, object> { { "Version", shortVersion } };

            foreach (var cultureInfo in CultureInfo.GetCultures(CultureTypes.AllCultures))
            {
                using (new CurrentCultureScope(cultureInfo))
                {
                    var json = JsonUtility.ToJson(eventParameters);
                    var jsonObject = JsonUtility.FromJson<JsonObject>(json);
                    Assert.Equal(expected: shortVersion, actual: jsonObject.GetString("Version"));
                }
            }
        }
    }
}
