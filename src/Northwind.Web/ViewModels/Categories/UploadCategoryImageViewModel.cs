using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Northwind.Web.ViewModels.Categories
{
    public class UploadCategoryImageViewModel
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public int FileSizeLimitMb { get; set; }

        public string AllowedFileExtension { get; set; }

        [Required]
        public IFormFile File { get; set; }
    }
}
