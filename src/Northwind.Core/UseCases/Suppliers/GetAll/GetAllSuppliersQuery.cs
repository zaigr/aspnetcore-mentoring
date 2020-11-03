using System.Collections.Generic;
using MediatR;
using Northwind.Domain.Models;

namespace Northwind.Core.UseCases.Suppliers.GetAll
{
    public class GetAllSuppliersQuery : IRequest<IList<Supplier>>
    {
    }
}
