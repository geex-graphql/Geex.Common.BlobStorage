using HotChocolate.Types;

using Kuanfang.Ims.DataFileObjects.External;

using MediatR;

namespace Geex.Common.BlobStorage.Api.Aggregates.BlobObjects.Inputs
{
    public class CreateBlobObjectRequest : IRequest<IBlobObject>
    {
        public IFile File { get; set; }
        public string Md5 { get; set; }
        public BlobStorageType StorageType { get; set; }
    }
}
