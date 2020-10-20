using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Northwind.Api.Models.Products;
using Northwind.Api.ViewModels;
using Northwind.Core.UseCases.Products.GetAll;

namespace Northwind.Api.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IMediator _mediator;

        private readonly IMapper _mapper;

        private readonly int? _defaultTablePageSize;

        public ProductsController(IMediator mediator, IMapper mapper, IConfiguration configuration)
        {
            _mediator = mediator;
            _mapper = mapper;

            _defaultTablePageSize = configuration.GetValue<int?>("Products:DefaultPageSize");
        }

        public async Task<IActionResult> Index()
        {
            var query = new GetAllProductsQuery
            {
                Top = _defaultTablePageSize != 0 ? _defaultTablePageSize : null,
            };
            var products = await _mediator.Send(query);

            var productItems = _mapper.Map<IList<ProductItemModel>>(products);
            var viewModel = new ProductsViewModel
            {
                ProductItemModels = productItems,
            };

            return View(viewModel);
        }
    }
}
