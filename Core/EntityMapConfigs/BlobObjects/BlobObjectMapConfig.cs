using Geex.Common.BlobStorage.Core.Aggregates.BlobObjects;
using Geex.Common.Abstraction;
using Geex.Common.Abstraction.Bson;
using Kuanfang.Ims.DataFileObjects.External;
using MongoDB.Bson.Serialization;

namespace Geex.Common.BlobStorage.Core.EntityMapConfigs.BlobObjects
{
    public class BlobObjectMapConfig : IEntityMapConfig<BlobObject>, IEntityMapConfig<DbFile>
    {
        public void Map(BsonClassMap<BlobObject> map)
        {
            map.AutoMap();
        }

        public void Map(BsonClassMap<DbFile> map)
        {
            map.AutoMap();
        }
    }
}
