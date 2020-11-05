using MediatR;

namespace Northwind.Core.UseCases.Categories.UpdateImage
{
    public class UpdateCategoryImageCommand : IRequest
    {
        public int CategoryId { get; set; }

        public byte[] ImageBytes { get; set; }
    }
}
