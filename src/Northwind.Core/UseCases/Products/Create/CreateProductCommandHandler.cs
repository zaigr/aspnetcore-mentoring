using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Northwind.Data;
using Northwind.Domain.Models;

namespace Northwind.Core.UseCases.Products.Create
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly NorthwindContext _context;

        public CreateProductCommandHandler(NorthwindContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                ProductName = request.ProductName,
                SupplierId = request.SupplierId,
                CategoryId = request.CategoryId,
                QuantityPerUnit = request.QuantityPerUnit,
                UnitPrice = request.UnitPrice,
                UnitsInStock = request.UnitsInStock,
                UnitsOnOrder = request.UnitsOnOrder,
                ReorderLevel = request.ReorderLevel,
                Discontinued = request.Discontinued,
            };

            _context.Products.Add(product);

            await _context.SaveChangesAsync(cancellationToken);

            return product.ProductId;
        }
    }
}
