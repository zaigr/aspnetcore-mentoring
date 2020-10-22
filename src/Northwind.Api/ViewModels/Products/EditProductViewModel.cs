using Microsoft.AspNetCore.Mvc.Rendering;
using Northwind.Api.Models.Products;

namespace Northwind.Api.ViewModels.Products
{
    public class EditProductViewModel
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public SelectList Categories { get; set; }

        public ProductEditModel EditModel { get; set; }
    }
}
