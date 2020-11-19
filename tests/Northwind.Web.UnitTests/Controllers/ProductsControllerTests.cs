using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Northwind.Core.UseCases.Categories.GetAll;
using Northwind.Core.UseCases.Products.Create;
using Northwind.Core.UseCases.Products.GetAll;
using Northwind.Core.UseCases.Products.GetSingle;
using Northwind.Core.UseCases.Products.Update;
using Northwind.Core.UseCases.Suppliers.GetAll;
using Northwind.Domain.Models;
using Northwind.Web.Configuration;
using Northwind.Web.Controllers;
using Northwind.Web.Mapping;
using Northwind.Web.ViewModels.Products;
using Xunit;

namespace Northwind.Web.UnitTests.Controllers
{
    public class ProductsControllerTests
    {
        private const int DefaultProductsPageSize = 10;

        private readonly IMapper _mapper = SetupMapper();

        private readonly IOptions<ProductsOptions> _productOptions;

        private readonly Mock<IMediator> _mediatorMock;

        public ProductsControllerTests()
        {
            var productConfig = new ProductsOptions { DefaultPageSize = DefaultProductsPageSize };

            var optionsMock = new Mock<IOptions<ProductsOptions>>();
            optionsMock.Setup(o => o.Value).Returns(productConfig);

            _productOptions = optionsMock.Object;

            _mediatorMock = new Mock<IMediator>();
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllProductsQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(GetProducts()));

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllCategoriesQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(GetCategories()));

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllSuppliersQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(GetSuppliers()));
        }

        private static IMapper SetupMapper()
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<MapperProfile>());

            return mapperConfig.CreateMapper();
        }

        private static IList<Product> GetProducts()
        {
            var fixture = new Fixture();
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            return fixture
                .CreateMany<Product>()
                .ToList();
        }

        private static IList<Supplier> GetSuppliers()
        {
            return new Fixture()
                .Build<Supplier>()
                .Without(s => s.Products)
                .CreateMany()
                .ToList();
        }

        private static IList<Category> GetCategories()
        {
            return new Fixture()
                .Build<Category>()
                .Without(c => c.Products)
                .CreateMany()
                .ToList();
        }

        private ProductsController Subject()
        {
            var mediator = _mediatorMock.Object;

            return new ProductsController(mediator, _mapper, _productOptions);
        }

        public static IEnumerable<object[]> IndexMethodTestMemberData()
        {
            return new List<object[]>
            {
                new object[] { null },
                new object[] { 10 },
                new object[] { 15 },
            };
        }

        [Theory]
        [MemberData(nameof(IndexMethodTestMemberData))]
        public async Task Index_TopQueryParameter_ReturnsView(int? topParameter)
        {
            // Arrange
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllProductsQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(GetProducts()))
                .Callback((IRequest<IList<Product>> request, CancellationToken token) =>
                {
                    var command = (GetAllProductsQuery)request;

                    var expectedTop = topParameter ?? DefaultProductsPageSize;
                    Assert.Equal(expectedTop, command.Top);

                    Assert.Null(command.Skip);
                });

            var controller = Subject();

            // Act
            var result = await controller.Index(topParameter);

            // Assert
            var view = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ProductsTableViewModel>(view.ViewData.Model);

            Assert.NotEmpty(model.ProductItemModels);

            _mediatorMock.Verify();
        }

        [Fact]
        public async Task Edit_ProductById_ReturnsView()
        {
            // Arrange
            var product = new Fixture()
                .Build<Product>()
                .Without(p => p.Supplier)
                .Without(p => p.Category)
                .Create();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetSingleProductQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(product))
                .Callback((IRequest<Product> request, CancellationToken token) =>
                {
                    var query = (GetSingleProductQuery) request;

                    Assert.Equal(product.ProductId, query.ProductId);
                });

            var controller = Subject();

            // Act
            var result = await controller.Edit(product.ProductId);

            // Assert
            var view = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<EditProductViewModel>(view.ViewData.Model);

            Assert.Equal(product.ProductId, model.ProductId);
            Assert.Equal(product.ProductName, model.ProductName);
            Assert.NotNull(model.Categories);
            Assert.NotNull(model.Suppliers);
            Assert.NotNull(model.EditModel);

            var editModel = model.EditModel;
            Assert.Equal(product.ProductId, editModel.ProductId);
            Assert.Equal(product.ProductName, editModel.ProductName);
        }

        [Fact]
        public async Task Edit_NullProductId_ReturnsNotFound()
        {
            // Arrange
            var controller = Subject();

            // Act
            var result = await controller.Edit(id: null);

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirect.ControllerName);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async Task Edit_InvalidModelSent_ReturnsBackView()
        {
            // Arrange
            var viewModel = new EditProductViewModel();

            var controller = Subject();
            controller.ModelState.AddModelError(nameof(EditProductViewModel.EditModel.ProductName), "Required");

            // Act
            var result = await controller.Edit(id: 10, viewModel);

            // Assert
            var view = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<EditProductViewModel>(view.ViewData.Model);

            Assert.Equal(viewModel, model);

            _mediatorMock
                .Verify(m => m.Send(It.IsAny<UpdateProductCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Edit_ValidModelSent_RedirectsToIndex()
        {
            // Arrange
            var viewModel = new Fixture()
                .Build<EditProductViewModel>()
                .Without(v => v.Suppliers)
                .Without(v => v.Categories)
                .Create();

            var controller = Subject();

            // Act
            var result = await controller.Edit(viewModel.ProductId, viewModel);

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirect.ControllerName);
            Assert.Equal("Index", redirect.ActionName);

            _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateProductCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Create_ViewRequest_ReturnsView()
        {
            // Arrange
            var controller = Subject();

            // Act
            var result = await controller.Create();

            // Assert
            var view = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<CreateProductViewModel>(view.ViewData.Model);

            Assert.NotEmpty(model.Categories);
            Assert.NotEmpty(model.Suppliers);
        }

        [Fact]
        public async Task Create_InvalidModelProvided_ReturnsBackView()
        {
            // Arrange
            var viewModel = new CreateProductViewModel();

            var controller = Subject();
            controller.ModelState.AddModelError(nameof(CreateProductViewModel.CreateModel.ProductName), "Required");

            // Act
            var result = await controller.Create(viewModel);

            // Assert
            var view = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<CreateProductViewModel>(view.ViewData.Model);

            Assert.Equal(viewModel, model);

            _mediatorMock.Verify(m => m.Send(It.IsAny<CreateProductCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Create_ValidModelSent_RedirectsToIndex()
        {
            // Arrange
            var viewModel = new Fixture()
                .Build<CreateProductViewModel>()
                .Without(v => v.Categories)
                .Without(v => v.Suppliers)
                .Create();

            var controller = Subject();

            // Act
            var result = await controller.Create(viewModel);

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirect.ControllerName);
            Assert.Equal("Index", redirect.ActionName);

            _mediatorMock.Verify(m => m.Send(It.IsAny<CreateProductCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
