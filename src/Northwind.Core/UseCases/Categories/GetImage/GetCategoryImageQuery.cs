using System.IO;
using MediatR;

namespace Northwind.Core.UseCases.Categories.GetImage
{
    public class GetCategoryImageQuery : IRequest<Stream>
    {
        public GetCategoryImageQuery(int categoryId)
        {
            CategoryId = categoryId;
        }

        public int CategoryId { get; }
    }
}
