using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Northwind.Core.UseCases.Categories.GetAll;
using Northwind.Core.UseCases.Categories.GetImage;
using Northwind.Web.Const;
using Northwind.Web.Models.Categories;
using Northwind.Web.ViewModels.Categories;

namespace Northwind.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IMediator _mediator;

        private readonly IMapper _mapper;

        public CategoriesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
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
    }
}
