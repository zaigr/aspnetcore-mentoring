using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Northwind.Api.Models.Products;
using Northwind.Core.UseCases.Products.GetAll;

namespace Northwind.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        private readonly IMapper _mapper;

        public ProductsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IList<ProductReadModel>> GetAll()
        {
            var result = await _mediator.Send(new GetAllProductsQuery());

            return _mapper.Map<IList<ProductReadModel>>(result);
        }
    }
}
