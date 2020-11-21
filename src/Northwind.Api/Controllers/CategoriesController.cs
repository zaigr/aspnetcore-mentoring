using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Northwind.Api.Config;
using Northwind.Api.Models.Categories;
using Northwind.Core.UseCases.Categories.GetAll;
using Northwind.Core.UseCases.Categories.GetImage;
using Northwind.Core.UseCases.Categories.UpdateImage;
using Northwind.Web.Common.Const;
using Northwind.Web.Common.Utilities;

namespace Northwind.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        private readonly IMapper _mapper;

        private readonly ImageFileOptions _imageFileOptions;

        public CategoriesController(IMediator mediator, IMapper mapper, IOptions<ImageFileOptions> options)
        {
            _mediator = mediator;
            _mapper = mapper;
            _imageFileOptions = options.Value;
        }

        [HttpGet]
        public async Task<IList<CategoryReadModel>> GetAll()
        {
            var queryResult = await _mediator.Send(new GetAllCategoriesQuery());

            return _mapper.Map<IList<CategoryReadModel>>(queryResult);
        }

        [HttpGet("{id}/image")]
        public async Task<FileStreamResult> GetImage(int id)
        {
            var query = new GetCategoryImageQuery(id);

            var result = await _mediator.Send(query);

            return File(result, ContentTypes.BmpImage);
        }

        [HttpPut("{id}/image")]
        public async Task<IActionResult> UpdateImage(int id, IFormFile file)
        {
            var image = await file.DumpToMemoryBufferAsync(ModelState, _imageFileOptions.FileSizeLimitBytes, _imageFileOptions.AllowedImageTypes);

            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.FirstOrDefault().Value?.Errors.FirstOrDefault()?.ErrorMessage;

                return BadRequest(errorMessage);
            }

            var command = new UpdateCategoryImageCommand
            {
                CategoryId = id,
                ImageBytes = image,
            };

            await _mediator.Send(command);

            return NoContent();
        }
    }
}
