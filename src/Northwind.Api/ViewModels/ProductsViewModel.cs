using System.Collections.Generic;
using Northwind.Api.Models.Products;

namespace Northwind.Api.ViewModels
{
    public class ProductsViewModel
    {
        public IList<ProductItemModel> ProductItemModels { get; set; }
    }
}
