using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Northwind.Core.Exceptions;
using Northwind.Data;
using Northwind.Domain.Models;

namespace Northwind.Core.UseCases.Products.Update
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly NorthwindContext _context;

        public UpdateProductCommandHandler(NorthwindContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == request.ProductId, cancellationToken);
            if (product == null)
            {
                throw new EntityNotFoundException();
            }

            UpdateProduct(product, request);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private void UpdateProduct(Product product, UpdateProductCommand request)
        {
            product.ProductName = request.ProductName;
            product.CategoryId = request.CategoryId;
            product.Discontinued = request.Discontinued;

            product.UnitPrice = request.UnitPrice ?? product.UnitPrice;
            product.UnitsInStock = request.UnitsInStock ?? product.UnitsInStock;
            product.UnitsOnOrder = request.UnitsInOrder ?? product.UnitsOnOrder;
            product.ReorderLevel = request.ReorderLevel ?? product.ReorderLevel;
        }
    }
}
