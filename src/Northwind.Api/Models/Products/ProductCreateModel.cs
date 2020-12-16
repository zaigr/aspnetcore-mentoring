using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Northwind.Api.Models.Products
{
    public class ProductCreateModel
    {
        [Required]
        [MaxLength(40)]
        [JsonPropertyName("name")]
        public string ProductName { get; set; }

        [JsonPropertyName("supplier_id")]
        public int SupplierId { get; set; }

        [JsonPropertyName("category_id")]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(20)]
        [JsonPropertyName("quantity_per_unit")]
        public string QuantityPerUnit { get; set; }

        [Required]
        [Range(0, 999)]
        [JsonPropertyName("unit_price")]
        public decimal? UnitPrice { get; set; }

        [Range(0, 999)]
        [JsonPropertyName("units_in_stock")]
        public short? UnitsInStock { get; set; }

        [Range(0, 999)]
        [JsonPropertyName("units_on_order")]
        public short? UnitsOnOrder { get; set; }

        [Range(0, 999)]
        [JsonPropertyName("reorder_level")]
        public short? ReorderLevel { get; set; }

        [JsonPropertyName("discontinued")]
        public bool Discontinued { get; set; }
    }
}
