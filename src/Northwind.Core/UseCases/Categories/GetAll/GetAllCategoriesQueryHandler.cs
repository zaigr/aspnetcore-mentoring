using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Northwind.Data;
using Northwind.Domain.Models;

namespace Northwind.Core.UseCases.Categories.GetAll
{
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, IList<Category>>
    {
        private readonly NorthwindContext _context;

        public GetAllCategoriesQueryHandler(NorthwindContext context)
        {
            _context = context;
        }

        public async Task<IList<Category>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Categories.AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}
