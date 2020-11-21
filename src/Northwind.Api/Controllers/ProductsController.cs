using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Northwind.Api.Models.Products;
using Northwind.Core.UseCases.Products.Create;
using Northwind.Core.UseCases.Products.Delete;
using Northwind.Core.UseCases.Products.GetAll;
using Northwind.Core.UseCases.Products.Update;

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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]ProductCreateModel model)
        {
            var command = _mapper.Map<CreateProductCommand>(model);

            var result = await _mediator.Send(command);

            return Ok(new { id = result });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody]ProductCreateModel model)
        {
            var command = _mapper.Map<UpdateProductCommand>(model);

            command.ProductId = id;

            await _mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteProductCommand(id);

            await _mediator.Send(command);

            return NoContent();
        }
    }
}
