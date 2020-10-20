namespace Northwind.Api.Models.Products
{
    public class ProductItemModel
    {
        public int ProductId { get; set; }

        public string Name { get; set; }

        public string SupplierName { get; set; }

        public string CategoryName { get; set; }

        public string QuantityPerUnit { get; set; }

        public decimal UnitPrice { get; set; }

        public int UnitsInStock { get; set; }

        public int UnitsInOrder { get; set; }

        public int RecorderLevel { get; set; }

        public bool Discontinued { get; set; }
    }
}
