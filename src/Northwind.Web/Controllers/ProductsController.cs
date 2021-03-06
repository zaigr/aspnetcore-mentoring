﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Northwind.Core.UseCases.Categories.GetAll;
using Northwind.Core.UseCases.Products.Create;
using Northwind.Core.UseCases.Products.GetAll;
using Northwind.Core.UseCases.Products.GetSingle;
using Northwind.Core.UseCases.Products.Update;
using Northwind.Core.UseCases.Suppliers.GetAll;
using Northwind.Domain.Models;
using Northwind.Web.Configuration;
using Northwind.Web.Extensions;
using Northwind.Web.Models.Products;
using Northwind.Web.ViewModels.Products;

namespace Northwind.Web.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IMediator _mediator;

        private readonly IMapper _mapper;

        private readonly int? _defaultTablePageSize;

        public ProductsController(IMediator mediator, IMapper mapper, IOptions<ProductsOptions> productsOptions)
        {
            _mediator = mediator;
            _mapper = mapper;

            _defaultTablePageSize = productsOptions.Value?.DefaultPageSize;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? top)
        {
            var query = new GetAllProductsQuery
            {
                Top = top ?? _defaultTablePageSize,
            };
            var products = await _mediator.Send(query);

            var productItems = _mapper.Map<IList<ProductTableItemModel>>(products);
            var viewModel = new ProductsTableViewModel
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
                return RedirectToAction(nameof(Index));
            }

            var query = new GetSingleProductQuery(id.Value);
            var product = await _mediator.Send(query);

            var categories = await _mediator.Send(new GetAllCategoriesQuery());
            var suppliers = await _mediator.Send(new GetAllSuppliersQuery());
            var editProductModel = _mapper.Map<ProductEditModel>(product);

            var viewModel = new EditProductViewModel
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Categories = categories.ToSelectList(nameof(Category.CategoryId), nameof(Category.CategoryName)),
                Suppliers = suppliers.ToSelectList(nameof(Supplier.SupplierId), nameof(Supplier.CompanyName)),
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
            var suppliers = await _mediator.Send(new GetAllSuppliersQuery());

            var viewModel = new CreateProductViewModel
            {
                Categories = categories.ToSelectList(nameof(Category.CategoryId), nameof(Category.CategoryName)),
                Suppliers = suppliers.ToSelectList(nameof(Supplier.SupplierId), nameof(Supplier.CompanyName)),
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
