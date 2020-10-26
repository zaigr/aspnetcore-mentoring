using System.ComponentModel.DataAnnotations;

namespace Northwind.Web.Models.Products
{
    public class ProductCreateModel
    {
        [Required]
        [MaxLength(40)]
        public string ProductName { get; set; }

        public int SupplierId { get; set; }

        public int CategoryId { get; set; }

        [Required]
        [MaxLength(20)]
        public string QuantityPerUnit { get; set; }

        [Required]
        [Range(0, 999)]
        public decimal? UnitPrice { get; set; }

        [Range(0, 999)]
        public short? UnitsInStock { get; set; }

        [Range(0, 999)]
        public short? UnitsOnOrder { get; set; }

        [Range(0, 999)]
        public short? ReorderLevel { get; set; }

        public bool Discontinued { get; set; }
    }
}
