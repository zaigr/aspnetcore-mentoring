using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Path = System.IO.Path;

namespace Northwind.Web.Common.Utilities
{
    public static class FormFileHelper
    {
        public static async Task<byte[]> DumpToMemoryBufferAsync(
            this IFormFile formFile,
            ModelStateDictionary modelState,
            int fileSizeLimitBytes,
            IList<string> allowedFileExtensions,
            string fileModelKey = "File")
        {
            if (formFile == null)
            {
                modelState.AddModelError(fileModelKey, "File was not chosen.");

                return new byte[0];
            }

            // Don't trust the file name sent by the client. To display
            // the file name, HTML-encode the value.
            var trustedFileName = WebUtility.HtmlEncode(formFile.FileName);

            if (formFile.Length == 0)
            {
                modelState.AddModelError(fileModelKey, $"{trustedFileName} is empty.");

                return new byte[0];
            }

            if (formFile.Length > fileSizeLimitBytes)
            {
                var megabyteSizeLimit = fileSizeLimitBytes / 10000000;
                modelState.AddModelError(fileModelKey, $"{trustedFileName} exceeds {megabyteSizeLimit:N1} MB.");

                return new byte[0];
            }

            if (!IsValidFileExtension(formFile.FileName, allowedFileExtensions))
            {
                modelState.AddModelError(fileModelKey, $"{trustedFileName} extension is not permitted.");

                return new byte[0];
            }

            await using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);

            return memoryStream.ToArray();
        }

        private static bool IsValidFileExtension(string fileName, IList<string> allowedFileExtensions)
        {
            if (allowedFileExtensions == null)
            {
                return true;
            }

            if (string.IsNullOrEmpty(fileName))
            {
                return false;
            }

            var extension = Path.GetExtension(fileName).TrimStart('.').ToLowerInvariant();

            return allowedFileExtensions.Contains(extension);
        }
    }
}
