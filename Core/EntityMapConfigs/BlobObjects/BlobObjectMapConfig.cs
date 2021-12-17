using Geex.Common.BlobStorage.Core.Aggregates.BlobObjects;
using Geex.Common.Abstraction;
using Geex.Common.Abstraction.Bson;
using Kuanfang.Ims.DataFileObjects.External;
using MongoDB.Bson.Serialization;

namespace Geex.Common.BlobStorage.Core.EntityMapConfigs.BlobObjects
{
    public class BlobObjectMapConfig : IEntityMapConfig<BlobObject>
    {
        public void Map(BsonClassMap<BlobObject> map)
        {
            map.AutoMap();
            map.MapMember(x => x.StorageType).SetSerializer(new EnumerationSerializer<BlobStorageType, string>());
        }
    }

    public class SettingMapConfig : IEntityMapConfig<DbFile>
    {
        public void Map(BsonClassMap<DbFile> map)
        {
            map.AutoMap();
        }
    }
}
