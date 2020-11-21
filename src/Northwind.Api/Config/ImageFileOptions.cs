using System.Collections.Generic;

namespace Northwind.Api.Config
{
    public class ImageFileOptions
    {
        public const string ImageFile = "ImageFile";

        public int FileSizeLimitBytes { get; set; }

        public IList<string> AllowedImageTypes { get; set; }
    }
}
