using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Geex.Common.BlobStorage.Api.Aggregates.BlobObjects;
using Geex.Common.BlobStorage.Api.Aggregates.BlobObjects.Inputs;
using Geex.Common.BlobStorage.Core.Aggregates;
using Geex.Common.BlobStorage.Core.Aggregates.BlobObjects;

using Geex.Common.Abstraction.Gql.Inputs;
using Geex.Common.BlobStorage.Api.Abstractions;
using Kuanfang.Ims.DataFileObjects;
using Kuanfang.Ims.DataFileObjects.External;
using MediatR;
using Microsoft.AspNetCore.Http;
using MimeKit;
using MongoDB.Bson;
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
            await entity.SaveAsync(cancellation: cancellationToken);
            if (request.StorageType == BlobStorageType.Db)
            {
                var dbFile = await DbContext.Find<DbFile>().Match(x => x.Md5 == request.Md5).ExecuteFirstAsync(cancellationToken);
                if (dbFile == null)
                {
                    dbFile = new DbFile(entity.Md5);
                    await dbFile.SaveAsync(cancellationToken);
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
                var blobObjects = await DbContext.Find<BlobObject>().Match(x => request.Ids.Contains(x.Id)).ExecuteAsync(cancellationToken);
                foreach (var blobObject in blobObjects)
                {
                    var duplicateCount = await DbContext.CountAsync<BlobObject>(x => x.Md5 == blobObject.Md5, cancellationToken);
                    if (duplicateCount <= 1)
                    {
                        var dbFile = await DbContext.Find<DbFile>().Match(x => x.Md5 == blobObject.Md5).ExecuteSingleAsync(cancellationToken);
                        await dbFile.Data.ClearAsync(cancellationToken);
                    }
                }
                await blobObjects.DeleteAllAsync();
                return Unit.Value;
            }
            throw new NotImplementedException();
        }

        public Task<BlobObject> GetOrNullAsync(string id)
        {
            return DbContext.Find<BlobObject>().OneAsync(id);
        }

        /// <summary>Handles a request</summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response from the request</returns>
        public async Task<(IBlobObject blob, DbFile dbFile)> Handle(DownloadFileRequest request, CancellationToken cancellationToken)
        {
            var blob = await DbContext.Find<BlobObject>().OneAsync(request.FileId, cancellationToken);
            var dbFile = await DbContext.Find<DbFile>().Match(x => x.Md5 == blob.Md5).ExecuteFirstAsync(cancellationToken);
            return (blob, dbFile);
        }
    }
}
