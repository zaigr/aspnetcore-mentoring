using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Northwind.Web.Configuration;

namespace Northwind.Web.Middleware
{
    public class ImageCachingMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ImageCachingOptions _options;

        private readonly ILogger _logger;

        private static MemoryCache _memoryCache;

        public ImageCachingMiddleware(
            RequestDelegate next,
            IOptions<ImageCachingOptions> options,
            ILogger<ImageCachingMiddleware> logger)
        {
            _next = next;
            _options = options.Value;
            _logger = logger;

            _memoryCache ??= new MemoryCache(new MemoryCacheOptions
            {
                SizeLimit = _options.MaxCacheItems,
            });
        }

        public async Task Invoke(HttpContext context)
        {
            var requestPath = context.Request.Path.Value;
            if (!_memoryCache.TryGetValue(requestPath, out var imagePath))
            {
                _logger.LogDebug($"No cached image found for request '{requestPath}'");

                await _next(context);

                await TryCacheNewRequest(context);

                return;
            }

            await WriteCachedResponseAsync(context, (string)imagePath);
        }

        private async Task TryCacheNewRequest(HttpContext context)
        {
            if (IsRequestForImage(context))
            {
                var buffer = await GetResponseBufferAsync(context);

                if (!Directory.Exists(_options.StoreLocation))
                {
                    Directory.CreateDirectory(_options.StoreLocation);
                }

                var filePath = Path.Join(_options.StoreLocation, Guid.NewGuid().ToString());
                await File.WriteAllBytesAsync(filePath, buffer.ToArray());

                AddFileToCache(context.Request.Path.Value, filePath);
            }
        }

        private bool IsRequestForImage(HttpContext context)
        {
            var contentType = context.Response.ContentType;
            if (string.IsNullOrEmpty(contentType))
            {
                return false;
            }

            return contentType.StartsWith("image/") &&
                   _options.AllowedImageTypes.Any(type => contentType.EndsWith(type));
        }

        private async Task<MemoryStream> GetResponseBufferAsync(HttpContext context)
        {
            var buffer = new MemoryStream();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            await context.Response.Body.CopyToAsync(buffer);

            return buffer;
        }

        private void AddFileToCache(string fileKey, string filePath)
        {
            using var entry = _memoryCache.CreateEntry(fileKey);

            entry.SetSize(1);
            entry.SetValue(filePath);
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(_options.CacheExpirationMin));

            entry.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                _logger.LogDebug($"Image cache item '{key}':'{value}' expired.");

                File.Delete((string)value);
            });
        }

        private async Task WriteCachedResponseAsync(HttpContext context, string imagePath)
        {
            await context.Response.BodyWriter.WriteAsync(await File.ReadAllBytesAsync(imagePath));
        }
    }
}
