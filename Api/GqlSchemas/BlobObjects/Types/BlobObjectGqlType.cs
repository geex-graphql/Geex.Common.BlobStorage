using Geex.Common.BlobStorage.Api.Aggregates.BlobObjects;
using HotChocolate.Types;

namespace Geex.Common.BlobStorage.Api.GqlSchemas.BlobObjects.Types
{
    public class BlobObjectGqlType : ObjectType<IBlobObject>
    {
        protected override void Configure(IObjectTypeDescriptor<IBlobObject> descriptor)
        {
            // Implicitly binding all fields, if you want to bind fields explicitly, read more about hot chocolate
            descriptor.BindFieldsImplicitly();
            base.Configure(descriptor);
        }
    }
}
