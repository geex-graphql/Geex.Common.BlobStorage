using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Geex.Common.Abstractions;
using Geex.Common.BlobStorage.Api;
using Geex.Common.BlobStorage.Api.Abstractions;

using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Voyager;

using Kuanfang.Ims.DataFileObjects.External;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders.Physical;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder
{
    internal static class MicrosoftAspNetCoreBuilderExtension
    {
        public static void UseFileDownload(this IEndpointRouteBuilder endpoints)
        {
            endpoints.Map(endpoints.ServiceProvider.GetService<BlobStorageModuleOptions>().FileDownloadPath, async (context) =>
            {
                if (context.Request.Query.TryGetValue("storageType", out var storageType) && context.Request.Query.TryGetValue("fileId", out var fileId))
                {
                    var (blobObject, dbFile) = await context.RequestServices.GetService<IMediator>().Send(new DownloadFileRequest(fileId, BlobStorageType.FromValue(storageType)));
                    context.Response.ContentType = blobObject.MimeType;
                    await dbFile.Data.DownloadAsync(context.Response.Body);
                    await context.Response.CompleteAsync();
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            });
        }
    }
}
