using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Northwind.Data;
using Northwind.Domain.Models;

namespace Northwind.Core.UseCases.Products.GetAll
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IList<Product>>
    {
        private readonly NorthwindContext _context;

        public GetAllProductsQueryHandler(NorthwindContext context)
        {
            _context = context;
        }

        public async Task<IList<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Products
                .Include(p => p.Supplier)
                .Include(p => p.Category)
                .AsQueryable();

            if (request.Top.HasValue)
            {
                query = query.Take(request.Top.Value);
            }

            if (request.Skip.HasValue)
            {
                query = query.Skip(request.Skip.Value);
            }

            return await query.ToListAsync(cancellationToken);
        }
    }
}
