using System.Threading.Tasks;

using Geex.Common.BlobStorage.Api.Aggregates.BlobObjects;
using Geex.Common.BlobStorage.Api.Aggregates.BlobObjects.Inputs;

using Geex.Common.Gql.Roots;

using HotChocolate;
using HotChocolate.Types;

using MediatR;

using MongoDB.Entities;

namespace Geex.Common.BlobStorage.Api.GqlSchemas.BlobObjects
{
    public class BlobObjectMutation : MutationTypeExtension<BlobObjectMutation>
    {
        /// <summary>
        /// 创建BlobObject
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IBlobObject> CreateBlobObject(
            [Service] IMediator mediator,
            CreateBlobObjectRequest input)
        {
            var result = await mediator.Send(input);
            return result;
        }

        /// <summary>
        /// 删除BlobObject
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> DeleteBlobObject(
            [Service] IMediator mediator,
            DeleteBlobObjectRequest input)
        {
            var result = await mediator.Send(input);
            return true;
        }
    }
}
