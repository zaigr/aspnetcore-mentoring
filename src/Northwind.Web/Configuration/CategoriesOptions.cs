namespace Northwind.Web.Configuration
{
    public class CategoriesOptions
    {
        public const string Categories = "Categories";

        public int ImageSizeLimitBytes { get; set; }

        public string AllowedImageType { get; set; }
    }
}
