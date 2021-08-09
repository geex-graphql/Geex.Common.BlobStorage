using Geex.Common.BlobStorage.Core.Aggregates.BlobObjects;
using Geex.Common.Abstraction;
using MongoDB.Bson.Serialization;

namespace Geex.Common.BlobStorage.Core.EntityMapConfigs.BlobObjects
{
    public class BlobObjectMapConfig : EntityMapConfig<DbBlobObject>
    {
        public override void Map(BsonClassMap<DbBlobObject> map)
        {
            map.AutoMap();
        }
    }
}
