using System.Collections.Generic;
using MediatR;
using Northwind.Domain.Models;

namespace Northwind.Core.UseCases.Products.GetAll
{
    public class GetAllProductsQuery : IRequest<IList<Product>>
    {
        public int? Top { get; set; }

        public int? Skip { get; set; }
    }
}
