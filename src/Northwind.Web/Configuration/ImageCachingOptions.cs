using System.Collections.Generic;

namespace Northwind.Web.Configuration
{
    public class ImageCachingOptions
    {
        public const string ImageCaching = "ImageCaching";

        public string StoreLocation { get; set; }

        public int MaxCacheItems { get; set; }

        public int CacheExpirationMin { get; set; }

        public IList<string> AllowedImageTypes { get; set; }
    }
}
