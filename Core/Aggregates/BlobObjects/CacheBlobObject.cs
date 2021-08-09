using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geex.Common.BlobStorage.Api.Aggregates.BlobObjects;

namespace Geex.Common.BlobStorage.Core.Aggregates.BlobObjects
{
    /// <summary>
    /// 基于redis的临时文件存储
    /// </summary>
    public class CacheBlobObject : IBlobObject
    {
        public string FileName { get; set; }
        public string Md5 { get; set; }
        public long FileSize { get; set; }
        public string MimeType { get; set; }
    }
}
