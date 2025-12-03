using System;
using System.Collections.Generic;
using System.Globalization;
using Segment.Serialization;
using Xunit;

namespace Tests
{
    public class DecimalNumberSerializationTest
    {
        private class CurrentCultureScope : IDisposable
        {
            private readonly CultureInfo _originalCurrentCulture;

            public CurrentCultureScope(CultureInfo cultureInfo)
            {
                _originalCurrentCulture = CultureInfo.CurrentCulture;
                CultureInfo.CurrentCulture = cultureInfo;
            }

            public void Dispose()
            {
                CultureInfo.CurrentCulture = _originalCurrentCulture;
            }
        }

        [Fact]
        public void TestDecimalNumberSerializationForAllCultures()
        {
            var eventParameters = new Dictionary<string, object> { { "FloatNumber", 1.23f } };

            foreach (var cultureInfo in CultureInfo.GetCultures(CultureTypes.AllCultures))
            {
                using (new CurrentCultureScope(cultureInfo))
                {
                    var eventParametersJson = JsonUtility.ToJson(eventParameters); // This call exist inside Segment.Analytics.Analytics::Track<T>
                    var jsonObject = JsonUtility.FromJson<JsonObject>(eventParametersJson); // This call exist inside Segment.Analytics.Analytics::Track<T>
                    var payloadJson = JsonUtility.ToJson(jsonObject); // Later, when dispatching the event to the analytics server, jsonObject gets serialized as JSON. If any float was serialized under pt-BR culture (which uses comma as a decimal separator), the JSON will be malformed and rejected by server.
                    Assert.True(payloadJson != null);
                    var payloadHashSet = new HashSet<char>(payloadJson); // Straight character comparison without culture info
                    Assert.Contains('.', payloadHashSet);
                    Assert.DoesNotContain(',', payloadHashSet);
                }
            }
        }
    }
}
