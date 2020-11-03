using System.Collections.Generic;
using MediatR;
using Northwind.Domain.Models;

namespace Northwind.Core.UseCases.Categories.GetAll
{
    public class GetAllCategoriesQuery : IRequest<IList<Category>>
    {
    }
}
