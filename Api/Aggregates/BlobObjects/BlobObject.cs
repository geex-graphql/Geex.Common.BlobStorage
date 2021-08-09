namespace Geex.Common.BlobStorage.Api.Aggregates.BlobObjects
{
    /// <summary>
    /// this is a aggregate root of this module, we name it the same as the module feel free to change it to its real name
    /// </summary>
    public interface IBlobObject
    {
        public string FileName { get; set; }
        public string Md5 { get; set; }
        public long FileSize { get; }
        public string MimeType { get; }
    }
}
