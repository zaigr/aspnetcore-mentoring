using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Northwind.Data;
using Northwind.Domain.Models;

namespace Northwind.Core.UseCases.Suppliers.GetAll
{
    public class GetAllSuppliersQueryHandler : IRequestHandler<GetAllSuppliersQuery, IList<Supplier>>
    {
        private readonly NorthwindContext _context;

        public GetAllSuppliersQueryHandler(NorthwindContext context)
        {
            _context = context;
        }

        public async Task<IList<Supplier>> Handle(GetAllSuppliersQuery request, CancellationToken cancellationToken)
        {
            return await _context.Suppliers.AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}
