using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Geex.Common.BlobStorage.Api.Aggregates.BlobObjects;
using Geex.Common.BlobStorage.Api.Aggregates.BlobObjects.Inputs;
using Geex.Common.BlobStorage.Core.Aggregates;
using Geex.Common.BlobStorage.Core.Aggregates.BlobObjects;

using Geex.Common.Abstraction.Gql.Inputs;
using Kuanfang.Ims.DataFileObjects;
using Kuanfang.Ims.DataFileObjects.External;
using MediatR;
using MimeKit;
using MongoDB.Bson;
using MongoDB.Entities;

namespace Geex.Common.BlobStorage.Core.Handlers
{
    public class BlobObjectHandler :
        IRequestHandler<QueryInput<IBlobObject>, IQueryable<IBlobObject>>,
        IRequestHandler<CreateBlobObjectRequest, IBlobObject>,
        IRequestHandler<DeleteBlobObjectRequest, Unit>
    {
        public DbContext DbContext { get; }

        public BlobObjectHandler(DbContext dbContext)
        {
            DbContext = dbContext;
        }
        public async Task<IQueryable<IBlobObject>> Handle(QueryInput<IBlobObject> input,
            CancellationToken cancellationToken)
        {
            // todo: 区分存储类型
            return DbContext.Queryable<DbBlobObject>();
        }

        public async Task<IBlobObject> Handle(CreateBlobObjectRequest request, CancellationToken cancellationToken)
        {
            if (request.StorageType == BlobStorageType.Db)
            {
                var entity = new DbBlobObject(request.File.Name, request.Md5, MimeTypes.GetMimeType(request.File.Name));
                DbContext.Attach(entity);
                await entity.Data.UploadAsync(request.File.OpenReadStream(), cancellation: cancellationToken);
                await entity.SaveAsync(cancellation: cancellationToken);
                return entity;
            }
            throw new NotImplementedException();
        }

        public async Task<Unit> Handle(DeleteBlobObjectRequest request, CancellationToken cancellationToken)
        {
            if (request.StorageType == BlobStorageType.Db)
            {
                var blobObjects = await DbContext.Find<DbBlobObject>().Match(x => request.Ids.Contains(x.Id)).ExecuteAsync(cancellationToken);
                foreach (var blobObject in blobObjects)
                {
                    await blobObject.Data.ClearAsync(cancellationToken);
                }
                await blobObjects.DeleteAllAsync();
                return Unit.Value;
            }
            throw new NotImplementedException();
        }

        public Task<DbBlobObject> GetOrNullAsync(string id)
        {
            return DbContext.Find<DbBlobObject>().OneAsync(id);
        }
    }
}
