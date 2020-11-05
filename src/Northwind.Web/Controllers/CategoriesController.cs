using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Northwind.Core.UseCases.Categories.GetAll;
using Northwind.Core.UseCases.Categories.GetImage;
using Northwind.Core.UseCases.Categories.GetSingle;
using Northwind.Core.UseCases.Categories.UpdateImage;
using Northwind.Web.Configuration;
using Northwind.Web.Const;
using Northwind.Web.Filters;
using Northwind.Web.Models.Categories;
using Northwind.Web.Utilities;
using Northwind.Web.ViewModels.Categories;

namespace Northwind.Web.Controllers
{
    [ServiceFilter(typeof(ActionLoggingFilter))]
    public class CategoriesController : Controller
    {
        private readonly IMediator _mediator;

        private readonly IMapper _mapper;

        private readonly int _categoryImageSizeLimit;

        private readonly string _allowedCategoryImageExtension;

        public CategoriesController(IMediator mediator, IMapper mapper, IOptions<CategoriesOptions> options)
        {
            _mediator = mediator;
            _mapper = mapper;

            _categoryImageSizeLimit = options.Value.ImageSizeLimitBytes;
            _allowedCategoryImageExtension = options.Value.AllowedImageType;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var query = new GetAllCategoriesQuery();
            var categories = await _mediator.Send(query);

            var categoryModels = _mapper.Map<IList<CategoryItemModel>>(categories);
            var viewModel = new CategoriesViewModel
            {
                CategoryItemModels = categoryModels,
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Image(int id)
        {
            var query = new GetCategoryImageQuery(id);

            var result = await _mediator.Send(query);

            return File(result, ContentTypes.BmpImage);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var query = new GetSingleCategoryQuery(id);
            var category = await _mediator.Send(query);

            var viewModel = new UploadCategoryImageViewModel
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                FileSizeLimitMb = _categoryImageSizeLimit / 10000000,
                AllowedFileExtension = _allowedCategoryImageExtension,
            };

            return View("UploadImage", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(UploadCategoryImageViewModel model)
        {
            var image = await model.File
                .DumpToMemoryBufferAsync(ModelState, _categoryImageSizeLimit, new[] { _allowedCategoryImageExtension });

            if (!ModelState.IsValid)
            {
                return View("UploadImage", model);
            }

            var command = new UpdateCategoryImageCommand
            {
                CategoryId = model.CategoryId,
                ImageBytes = image,
            };

            await _mediator.Send(command);

            return RedirectToAction(nameof(Index));
        }
    }
}
