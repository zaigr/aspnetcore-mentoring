using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Northwind.Api.Models.Products;
using Northwind.Api.ViewModels;
using Northwind.Api.ViewModels.Products;
using Northwind.Core.UseCases.Categories.GetAll;
using Northwind.Core.UseCases.Products.Create;
using Northwind.Core.UseCases.Products.GetAll;
using Northwind.Core.UseCases.Products.GetSingle;
using Northwind.Core.UseCases.Products.Update;
using Northwind.Core.UseCases.Suppliers.GetAll;
using Northwind.Domain.Models;

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

        [HttpGet]
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

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var query = new GetSingleProductQuery(id.Value);
            var product = await _mediator.Send(query);

            var categories = await _mediator.Send(new GetAllCategoriesQuery());

            var editProductModel = _mapper.Map<ProductEditModel>(product);
            var categorySelectList = new SelectList(categories, nameof(Category.CategoryId), nameof(Category.CategoryName), product.Category.CategoryName);

            var viewModel = new EditProductViewModel
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Categories = categorySelectList,
                EditModel = editProductModel,
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditProductViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var command = _mapper.Map<UpdateProductCommand>(viewModel.EditModel);
            await _mediator.Send(command);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _mediator.Send(new GetAllCategoriesQuery());
            var categorySelectList = new SelectList(categories, nameof(Category.CategoryId), nameof(Category.CategoryName));

            var suppliers = await _mediator.Send(new GetAllSuppliersQuery());
            var suppliersSelectList = new SelectList(suppliers, nameof(Supplier.SupplierId), nameof(Supplier.CompanyName));

            var viewModel = new CreateProductViewModel
            {
                Categories = categorySelectList,
                Suppliers = suppliersSelectList,
                CreateModel = new ProductCreateModel(),
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var command = _mapper.Map<CreateProductCommand>(viewModel.CreateModel);

            await _mediator.Send(command);

            return RedirectToAction(nameof(Index));
        }
    }
}
