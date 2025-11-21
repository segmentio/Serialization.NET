using System;
using Newtonsoft.Json;
using Segment.Serialization;
using Xunit;

namespace Tests
{
    public class JsonNullableTest
    {
        [Serializable]
        public struct DataModel
        {
            public string old_parcel;
            public string new_parcel;
            public string scene_hash;
            public bool? is_empty_scene;
        }

        private static DataModel NewModelWithNullableStruct() =>
            new DataModel
            {
                old_parcel = "RedRock",
                new_parcel = string.Empty,
                scene_hash = Guid.NewGuid().ToString(),
                is_empty_scene = null
            };

        [Fact]
        public void JsonValidityCrash()
        {
            DataModel data = NewModelWithNullableStruct();

            JsonObject json = new JsonObject
            {
                { "old_parcel", data.old_parcel },
                { "new_parcel", data.new_parcel },
                { "scene_hash", data.scene_hash },
                { "is_empty_scene", data.is_empty_scene }
            };

            string stringified = json.ToString();
            JsonConvert.DeserializeObject<DataModel>(stringified);
        }
    }
}
