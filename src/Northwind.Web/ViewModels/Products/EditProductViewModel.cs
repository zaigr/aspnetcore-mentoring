using Microsoft.AspNetCore.Mvc.Rendering;
using Northwind.Web.Models.Products;

namespace Northwind.Web.ViewModels.Products
{
    public class EditProductViewModel
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public SelectList Suppliers { get; set; }

        public SelectList Categories { get; set; }

        public ProductEditModel EditModel { get; set; }
    }
}
