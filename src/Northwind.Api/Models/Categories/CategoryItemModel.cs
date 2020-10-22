using System.ComponentModel;

namespace Northwind.Api.Models.Categories
{
    public class CategoryItemModel
    {
        [DisplayName("Id")]
        public int CategoryId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
