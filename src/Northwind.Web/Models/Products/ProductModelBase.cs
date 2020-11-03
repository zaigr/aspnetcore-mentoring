using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Northwind.Web.Models.Products
{
    public abstract class ProductModelBase
    {
        [Required]
        [MaxLength(40)]
        [DisplayName("Name")]
        public string ProductName { get; set; }

        [DisplayName("Supplier")]
        public int SupplierId { get; set; }

        [DisplayName("Category")]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(20)]
        [DisplayName("Quantity per Unit")]
        public string QuantityPerUnit { get; set; }

        [Required]
        [Range(0, 999)]
        [DisplayName("Unit Price")]
        public decimal? UnitPrice { get; set; }

        [Range(0, 999)]
        [DisplayName("Units in Stock")]
        public short? UnitsInStock { get; set; }

        [Range(0, 999)]
        [DisplayName("Units on Order")]
        public short? UnitsOnOrder { get; set; }

        [Range(0, 999)]
        [DisplayName("Reorder Level")]
        public short? ReorderLevel { get; set; }

        public bool Discontinued { get; set; }
    }
}
