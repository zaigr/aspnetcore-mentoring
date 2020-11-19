using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Northwind.Api.Models.Categories;
using Northwind.Core.UseCases.Categories.GetAll;

namespace Northwind.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        private readonly IMapper _mapper;

        public CategoriesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IList<CategoryReadModel>> GetAll()
        {
            var queryResult = await _mediator.Send(new GetAllCategoriesQuery());

            return _mapper.Map<IList<CategoryReadModel>>(queryResult);
        }
    }
}
