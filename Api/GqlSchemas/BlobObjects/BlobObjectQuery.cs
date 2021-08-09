using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Geex.Common.BlobStorage.Api.Aggregates.BlobObjects;
using Geex.Common.BlobStorage.Api.Aggregates.BlobObjects.Inputs;
using Geex.Common.BlobStorage.Api.GqlSchemas.BlobObjects.Types;

using Geex.Common.Abstraction.Gql.Inputs;
using Geex.Common.Gql.Roots;

using HotChocolate;
using HotChocolate.Types;

using MediatR;

using MongoDB.Entities;

namespace Geex.Common.BlobStorage.Api.GqlSchemas.BlobObjects
{
    public class BlobObjectQuery : QueryTypeExtension<BlobObjectQuery>
    {
        protected override void Configure(IObjectTypeDescriptor<BlobObjectQuery> descriptor)
        {
            descriptor.ResolveMethod(x => x.BlobObjects(default))
            .UseOffsetPaging<BlobObjectGqlType>()
            .UseFiltering<IBlobObject>(x =>
            {
                x.Field(y => y.FileName);
            })
            ;
            base.Configure(descriptor);
        }

        /// <summary>
        /// 列表获取BlobObject
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IQueryable<IBlobObject>> BlobObjects(
            [Service] IMediator mediator)
        {
            var result = await mediator.Send(new QueryInput<IBlobObject>());
            return result;
        }

    }
}
