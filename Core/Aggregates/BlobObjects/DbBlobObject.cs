using System;
using Geex.Common.BlobStorage.Api.Aggregates.BlobObjects;
using Geex.Common.Abstractions;
using MongoDB.Entities;

namespace Geex.Common.BlobStorage.Core.Aggregates.BlobObjects
{
    /// <summary>
    /// this is a aggregate root of this module, we name it the same as the module feel free to change it to its real name
    /// </summary>
    public class DbBlobObject : FileEntity, IBlobObject
    {
        public DbBlobObject(string fileName, string md5, string mimeType)
        {
            this.FileName = fileName;
            this.Md5 = md5;
            this.MimeType = mimeType;
        }
        [Obsolete("for internal use only.", false)]
        public DbBlobObject()
        {

        }

        public string FileName { get; set; }
        public string Md5 { get; set; }
        public string MimeType { get; set; }
    }
}
