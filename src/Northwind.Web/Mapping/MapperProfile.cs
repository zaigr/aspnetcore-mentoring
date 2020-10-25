using AutoMapper;
using Northwind.Core.UseCases.Products.Create;
using Northwind.Core.UseCases.Products.Update;
using Northwind.Domain.Models;
using Northwind.Web.Models.Categories;
using Northwind.Web.Models.Products;

namespace Northwind.Web.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Category, CategoryItemModel>()
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.CategoryName));

            CreateMap<Product, ProductItemModel>()
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.ProductName))
                .ForMember(d => d.SupplierName, opt => opt.MapFrom(s => s.Supplier.CompanyName))
                .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Category.CategoryName));

            CreateMap<Product, ProductEditModel>();

            CreateMap<ProductEditModel, UpdateProductCommand>();
            CreateMap<ProductCreateModel, CreateProductCommand>();
        }
    }
}
