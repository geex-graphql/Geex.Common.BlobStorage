
using System.Collections.Generic;
using Geex.Common.Abstractions;
using Kuanfang.Ims.DataFileObjects.External;
using MediatR;

namespace Geex.Common.BlobStorage.Api.Aggregates.BlobObjects.Inputs
{
    public class DeleteBlobObjectRequest : IRequest<Unit>
    {
        public List<string> Ids { get; set; }
        public BlobStorageType StorageType { get; set; }
    }
}
