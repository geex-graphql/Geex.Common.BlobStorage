using Geex.Common.Abstractions;
using Geex.Common.BlobStorage.Api;

using HotChocolate.Types;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Modularity;

namespace Geex.Common.BlobStorage.Core
{
    [DependsOn(typeof(BlobStorageApiModule))]
    public class BlobStorageCoreModule : GeexModule<BlobStorageCoreModule>
    {
        public override void PostConfigureServices(ServiceConfigurationContext context)
        {
            SchemaBuilder.AddType<UploadType>();
            base.PostConfigureServices(context);
        }

        /// <inheritdoc />
        public override void OnPostApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            app.UseFileDownload();
            base.OnPostApplicationInitialization(context);
        }
    }
}
