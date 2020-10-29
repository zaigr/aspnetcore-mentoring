using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Northwind.Core.UseCases.Categories.GetAll;
using Northwind.Domain.Models;
using Northwind.Web.Controllers;
using Northwind.Web.Mapping;
using Northwind.Web.ViewModels.Categories;
using Xunit;

namespace Northwind.Web.UnitTests.Controllers
{
    public class CategoriesControllerTests
    {
        private readonly IMapper _mapper = SetupMapper();

        private readonly Mock<IMediator> _mediatorMock;

        public CategoriesControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllCategoriesQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(GetCategories()));
        }

        private static IMapper SetupMapper()
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<MapperProfile>());

            return mapperConfig.CreateMapper();
        }

        private static IList<Category> GetCategories()
        {
            return new Fixture()
                .Build<Category>()
                .Without(c => c.Products)
                .CreateMany()
                .ToList();
        }

        private CategoriesController Subject()
        {
            return new CategoriesController(_mediatorMock.Object, _mapper);
        }

        [Fact]
        public async Task Index_ViewRequested_ReturnsView()
        {
            // Arrange
            var controller = Subject();

            // Act
            var result = await controller.Index();

            // Assert
            var view = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<CategoriesViewModel>(view.ViewData.Model);

            Assert.NotEmpty(model.CategoryItemModels);
        }
    }
}
