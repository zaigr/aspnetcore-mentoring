using MediatR;
using Northwind.Domain.Models;

namespace Northwind.Core.UseCases.Products.GetSingle
{
    public class GetSingleProductQuery : IRequest<Product>
    {
        public GetSingleProductQuery(int productId)
        {
            ProductId = productId;
        }

        public int ProductId { get; }
    }
}
