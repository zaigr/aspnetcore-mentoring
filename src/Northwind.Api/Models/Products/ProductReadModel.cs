﻿using System.Text.Json.Serialization;

namespace Northwind.Api.Models.Products
{
    public class ProductReadModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("supplier")]
        public string SupplierName { get; set; }

        [JsonPropertyName("category")]
        public string CategoryName { get; set; }

        [JsonPropertyName("quantity_per_unit")]
        public string QuantityPerUnit { get; set; }

        [JsonPropertyName("unit_price")]
        public decimal UnitPrice { get; set; }

        [JsonPropertyName("units_in_stock")]
        public int UnitsInStock { get; set; }

        [JsonPropertyName("units_on_order")]
        public int UnitsOnOrder { get; set; }

        [JsonPropertyName("reorder_level")]
        public int ReorderLevel { get; set; }

        [JsonPropertyName("discontinued")]
        public bool Discontinued { get; set; }

    }
}
