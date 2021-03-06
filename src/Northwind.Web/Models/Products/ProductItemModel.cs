﻿using System.ComponentModel;

namespace Northwind.Web.Models.Products
{
    public class ProductTableItemModel
    {
        [DisplayName("Id")]
        public int ProductId { get; set; }

        public string Name { get; set; }

        [DisplayName("Supplier")]
        public string SupplierName { get; set; }

        [DisplayName("Category")]
        public string CategoryName { get; set; }

        [DisplayName("Quantity per Unit")]
        public string QuantityPerUnit { get; set; }

        [DisplayName("Unit Price")]
        public decimal UnitPrice { get; set; }

        [DisplayName("Units in Stock")]
        public int UnitsInStock { get; set; }

        [DisplayName("Units in Order")]
        public int UnitsInOrder { get; set; }

        [DisplayName("Recorder lvl")]
        public int RecorderLevel { get; set; }

        public bool Discontinued { get; set; }
    }
}
