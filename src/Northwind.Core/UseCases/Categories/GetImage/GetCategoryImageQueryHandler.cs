using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Northwind.Core.Exceptions;
using Northwind.Data;

namespace Northwind.Core.UseCases.Categories.GetImage
{
    public class GetCategoryImageQueryHandler : IRequestHandler<GetCategoryImageQuery, Stream>
    {
        private readonly NorthwindContext _context;

        public GetCategoryImageQueryHandler(NorthwindContext context)
        {
            _context = context;
        }

        public async Task<Stream> Handle(GetCategoryImageQuery request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == request.CategoryId, cancellationToken);
            if (category == null)
            {
                throw new EntityNotFoundException();
            }

            var pictureBytes = category
                .Picture
                .Skip(78) // First 78 bytes of image is garbage
                .ToArray();

            return new MemoryStream(pictureBytes);
        }
    }
}
