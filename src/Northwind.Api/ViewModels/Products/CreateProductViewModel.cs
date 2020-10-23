using Microsoft.AspNetCore.Mvc.Rendering;
using Northwind.Api.Models.Products;

namespace Northwind.Api.ViewModels.Products
{
    public class CreateProductViewModel
    {
        public SelectList Categories { get; set; }

        public SelectList Suppliers { get; set; }

        public ProductCreateModel CreateModel { get; set; }
    }
}
