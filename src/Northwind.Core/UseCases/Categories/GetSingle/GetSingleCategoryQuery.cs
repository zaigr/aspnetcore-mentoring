using MediatR;
using Northwind.Domain.Models;

namespace Northwind.Core.UseCases.Categories.GetSingle
{
    public class GetSingleCategoryQuery : IRequest<Category>
    {
        public GetSingleCategoryQuery(int categoryId)
        {
            CategoryId = categoryId;
        }

        public int CategoryId { get; }
    }
}
