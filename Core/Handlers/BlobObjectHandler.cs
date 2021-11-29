using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Geex.Common.BlobStorage.Api.Aggregates.BlobObjects;
using Geex.Common.BlobStorage.Api.Aggregates.BlobObjects.Inputs;
using Geex.Common.BlobStorage.Core.Aggregates.BlobObjects;

using Geex.Common.Abstraction.Gql.Inputs;
using Geex.Common.BlobStorage.Api.Abstractions;
using Kuanfang.Ims.DataFileObjects.External;
using MediatR;
using MimeKit;
using MongoDB.Entities;

namespace Geex.Common.BlobStorage.Core.Handlers
{
    public class BlobObjectHandler :
        IRequestHandler<QueryInput<IBlobObject>, IQueryable<IBlobObject>>,
        IRequestHandler<CreateBlobObjectRequest, IBlobObject>,
        IRequestHandler<DeleteBlobObjectRequest, Unit>,
        IRequestHandler<DownloadFileRequest, (IBlobObject blob, DbFile dbFile)>
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
            return DbContext.Queryable<BlobObject>();
        }

        public async Task<IBlobObject> Handle(CreateBlobObjectRequest request, CancellationToken cancellationToken)
        {
            var entity = new BlobObject(request.File.Name, request.Md5, request.StorageType, MimeTypes.GetMimeType(request.File.Name), request.File.Length.GetValueOrDefault());
            DbContext.Attach(entity);
            if (request.StorageType == BlobStorageType.Db)
            {
                var dbFile = DbContext.Queryable<DbFile>().FirstOrDefault(x => x.Md5 == request.Md5);
                if (dbFile == null)
                {
                    dbFile = new DbFile(entity.Md5);
                    await DbContext.SaveChanges(cancellationToken);
                    await dbFile.Data.UploadAsync(request.File.OpenReadStream(), cancellation: cancellationToken);
                }
            }
            else
            {
                throw new NotImplementedException();
            }
            return entity;
        }

        public async Task<Unit> Handle(DeleteBlobObjectRequest request, CancellationToken cancellationToken)
        {
            if (request.StorageType == BlobStorageType.Db)
            {
                var blobObjects = await Task.FromResult(DbContext.Queryable<DbFile>().Where(x => request.Ids.Contains(x.Id)));
                foreach (var blobObject in blobObjects)
                {
                    var duplicateCount = await DbContext.CountAsync<BlobObject>(x => x.Md5 == blobObject.Md5, cancellationToken);
                    if (duplicateCount <= 1)
                    {
                        var dbFile = await Task.FromResult(DbContext.Queryable<DbFile>().Single(x => x.Md5 == blobObject.Md5));
                        await dbFile.Data.ClearAsync(cancellationToken);
                    }
                }
                await DbContext.DeleteAsync<BlobObject>(blobObjects.Select(x => x.Id), cancellationToken);
                return Unit.Value;
            }
            throw new NotImplementedException();
        }

        public Task<BlobObject> GetOrNullAsync(string id)
        {
            return Task.FromResult(DbContext.Queryable<BlobObject>().First(x => x.Id == id));
        }

        /// <summary>Handles a request</summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response from the request</returns>
        public async Task<(IBlobObject blob, DbFile dbFile)> Handle(DownloadFileRequest request, CancellationToken cancellationToken)
        {
            var blob = await Task.FromResult(DbContext.Queryable<BlobObject>().First(x => x.Id == request.FileId));
            var dbFile = await Task.FromResult(DbContext.Queryable<DbFile>().First(x => x.Md5 == blob.Md5));
            return (blob, dbFile);
        }
    }
}
