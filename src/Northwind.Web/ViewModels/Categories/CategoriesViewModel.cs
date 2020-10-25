using System.Collections.Generic;
using Northwind.Web.Models.Categories;

namespace Northwind.Web.ViewModels.Categories
{
    public class CategoriesViewModel
    {
        public IList<CategoryItemModel> CategoryItemModels { get; set; }
    }
}
