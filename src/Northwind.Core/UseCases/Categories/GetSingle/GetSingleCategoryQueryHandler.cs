using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Northwind.Core.Exceptions;
using Northwind.Data;
using Northwind.Domain.Models;

namespace Northwind.Core.UseCases.Categories.GetSingle
{
    public class GetSingleCategoryQueryHandler : IRequestHandler<GetSingleCategoryQuery, Category>
    {
        private readonly NorthwindContext _context;

        public GetSingleCategoryQueryHandler(NorthwindContext context)
        {
            _context = context;
        }

        public async Task<Category> Handle(GetSingleCategoryQuery request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == request.CategoryId, cancellationToken);
            if (category == null)
            {
                throw new EntityNotFoundException();
            }

            return category;
        }
    }
}
