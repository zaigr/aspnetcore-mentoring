using System.ComponentModel.DataAnnotations;

namespace Northwind.Web.Models.Products
{
    public class ProductEditModel
    {
        public int ProductId { get; set; }

        [Required]
        [MaxLength(40)]
        public string ProductName { get; set; }

        public int CategoryId { get; set; }

        [Range(0, 999)]
        public decimal? UnitPrice { get; set; }

        [Range(0, 999)]
        public int? UnitsInStock { get; set; }

        [Range(0, 999)]
        public short? UnitsInOrder { get; set; }

        [Range(0, 999)]
        public short? ReorderLevel { get; set; }

        public bool Discontinued { get; set; }
    }
}
