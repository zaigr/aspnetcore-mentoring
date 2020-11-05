using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Northwind.Core.Exceptions;
using Northwind.Data;

namespace Northwind.Core.UseCases.Categories.UpdateImage
{
    public class UpdateCategoryImageCommandHandler : IRequestHandler<UpdateCategoryImageCommand>
    {
        private readonly NorthwindContext _context;

        public UpdateCategoryImageCommandHandler(NorthwindContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateCategoryImageCommand request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == request.CategoryId, cancellationToken);
            if (category == null)
            {
                throw new EntityNotFoundException();
            }

            await using var memoryBuffer = new MemoryStream();

            // write first 78 bytes of trash
            await memoryBuffer.WriteAsync(new byte[78], cancellationToken);

            await memoryBuffer.WriteAsync(request.ImageBytes, cancellationToken);

            category.Picture = memoryBuffer.ToArray();

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
