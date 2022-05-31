using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.StaticFiles;
using IContentTypeProvider = Pillow.ApplicationCore.Interfaces.IContentTypeProvider;

namespace Pillow.Infrastructure.Services
{
    public class ContentTypeProvider : IContentTypeProvider
    {
        private static readonly Lazy<FileExtensionContentTypeProvider> FileExtensionContentTypeProvider =
            new(() =>
            {
                var result = new FileExtensionContentTypeProvider();
                result.Mappings.TryAdd(".ac3", "audio/ac3");
                return result;
            });

        public bool TryGetContentType(string subpath, out string contenType)
        {
            return FileExtensionContentTypeProvider.Value.TryGetContentType(subpath, out contenType);
        }
    }
}