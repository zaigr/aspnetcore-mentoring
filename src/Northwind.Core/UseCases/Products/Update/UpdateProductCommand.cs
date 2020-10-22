using MediatR;

namespace Northwind.Core.UseCases.Products.Update
{
    public class UpdateProductCommand : IRequest
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int CategoryId { get; set; }

        public decimal? UnitPrice { get; set; }

        public short? UnitsInStock { get; set; }

        public short? UnitsInOrder { get; set; }

        public short? ReorderLevel { get; set; }

        public bool Discontinued { get; set; }
    }
}
