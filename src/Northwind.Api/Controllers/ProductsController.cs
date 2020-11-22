using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Northwind.Api.Models.Products;
using Northwind.Api.Models.Response;
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

        /// <summary>
        /// Returns full list of products.
        /// </summary>
        /// <response code="200" />
        [HttpGet]
        public async Task<IList<ProductReadModel>> GetAll()
        {
            var result = await _mediator.Send(new GetAllProductsQuery());

            return _mapper.Map<IList<ProductReadModel>>(result);
        }

        /// <summary>
        /// Creates new product.
        /// </summary>
        /// <param name="model"></param>
        /// <response code="200" >Returns id of newly created product </response>
        /// <response code="400" >Returns list of validation errors.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ObjectIdResponse> Create([FromBody]ProductCreateModel model)
        {
            var command = _mapper.Map<CreateProductCommand>(model);

            var result = await _mediator.Send(command);

            return new ObjectIdResponse(result);
        }

        /// <summary>
        /// Update whole product entry.
        /// </summary>
        /// <param name="id">ID of product.</param>
        /// <param name="model"></param>
        /// <response code="204" >Product updated successfully.</response>
        /// <response code="400" >Returns list of validation errors.</response>
        /// <response code="404" >Product with provided ID not found.</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody]ProductCreateModel model)
        {
            var command = _mapper.Map<UpdateProductCommand>(model);

            command.ProductId = id;

            await _mediator.Send(command);

            return NoContent();
        }

        /// <summary>
        /// Removes product entry.
        /// </summary>
        /// <param name="id">ID of product.</param>
        /// <response code="204" >Product updated successfully.</response>
        /// <response code="404" >Product with provided ID not found.</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteProductCommand(id);

            await _mediator.Send(command);

            return NoContent();
        }
    }
}
