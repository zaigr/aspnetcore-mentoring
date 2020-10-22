using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Northwind.Core.Exceptions;
using Northwind.Data;
using Northwind.Domain.Models;

namespace Northwind.Core.UseCases.Products.GetSingle
{
    public class GetSingleProductQueryHandler : IRequestHandler<GetSingleProductQuery, Product>
    {
        private readonly NorthwindContext _context;

        public GetSingleProductQueryHandler(NorthwindContext context)
        {
            _context = context;
        }

        public async Task<Product> Handle(GetSingleProductQuery request, CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProductId == request.ProductId, cancellationToken);

            if (product == null)
            {
                throw new EntityNotFoundException();
            }

            return product;
        }
    }
}
