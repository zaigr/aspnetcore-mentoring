using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Northwind.Core.Exceptions;
using Northwind.Data;

namespace Northwind.Core.UseCases.Products.Delete
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly NorthwindContext _context;

        public DeleteProductCommandHandler(NorthwindContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == request.ProductId, cancellationToken);
            if (product == null)
            {
                throw new EntityNotFoundException();
            }

            _context.Products.Remove(product);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
