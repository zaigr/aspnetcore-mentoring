using Microsoft.AspNetCore.Mvc.Rendering;
using Northwind.Web.Models.Products;

namespace Northwind.Web.ViewModels.Products
{
    public class CreateProductViewModel
    {
        public SelectList Categories { get; set; }

        public SelectList Suppliers { get; set; }

        public ProductCreateModel CreateModel { get; set; }
    }
}
