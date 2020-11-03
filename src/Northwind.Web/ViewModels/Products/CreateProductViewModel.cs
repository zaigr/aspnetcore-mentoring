using Microsoft.AspNetCore.Mvc.Rendering;
using Northwind.Web.Models.Products;

namespace Northwind.Web.ViewModels.Products
{
    public class CreateProductViewModel
    {
        public CreateProductViewModel()
        {
            CreateModel = new ProductCreateModel();
        }

        public SelectList Suppliers { get; set; }

        public SelectList Categories { get; set; }

        public ProductCreateModel CreateModel { get; set; }
    }
}
