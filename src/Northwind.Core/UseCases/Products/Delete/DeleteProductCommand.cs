using MediatR;

namespace Northwind.Core.UseCases.Products.Delete
{
    public class DeleteProductCommand : IRequest
    {
        public DeleteProductCommand(int productId)
        {
            ProductId = productId;
        }

        public int ProductId { get; }
    }
}
