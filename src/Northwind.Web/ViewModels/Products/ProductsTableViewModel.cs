using System.Collections.Generic;
using Northwind.Web.Models.Products;

namespace Northwind.Web.ViewModels.Products
{
    public class ProductsTableViewModel
    {
        public IList<ProductTableItemModel> ProductItemModels { get; set; }
    }
}
