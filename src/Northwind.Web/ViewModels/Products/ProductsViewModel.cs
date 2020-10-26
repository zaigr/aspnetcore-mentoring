using System.Collections.Generic;
using Northwind.Web.Models.Products;

namespace Northwind.Web.ViewModels.Products
{
    public class ProductsViewModel
    {
        public IList<ProductItemModel> ProductItemModels { get; set; }
    }
}
