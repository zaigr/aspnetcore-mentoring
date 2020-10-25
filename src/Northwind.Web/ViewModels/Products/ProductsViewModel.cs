using System.Collections.Generic;
using Northwind.Web.Models.Products;

namespace Northwind.Web.ViewModels
{
    public class ProductsViewModel
    {
        public IList<ProductItemModel> ProductItemModels { get; set; }
    }
}
