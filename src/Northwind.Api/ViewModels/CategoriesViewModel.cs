using System.Collections.Generic;
using Northwind.Api.Models.Categories;

namespace Northwind.Api.ViewModels
{
    public class CategoriesViewModel
    {
        public IList<CategoryItemModel> CategoryItemModels { get; set; }
    }
}
