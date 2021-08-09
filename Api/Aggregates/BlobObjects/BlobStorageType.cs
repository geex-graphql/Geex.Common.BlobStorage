using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Geex.Common.Abstractions;

namespace Kuanfang.Ims.DataFileObjects.External
{
    public class BlobStorageType : Enumeration<BlobStorageType, string>
    {
        public BlobStorageType(string name, string value) : base(name, value)
        {
        }
        public static BlobStorageType AliyunOss = new BlobStorageType(nameof(AliyunOss), nameof(AliyunOss));
        public static BlobStorageType RedisCache = new BlobStorageType(nameof(RedisCache), nameof(RedisCache));
        public static BlobStorageType Db = new BlobStorageType(nameof(Db), nameof(Db));

    }
}
