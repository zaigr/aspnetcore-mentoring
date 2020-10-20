using AutoMapper;
using Northwind.Api.Models.Categories;
using Northwind.Api.Models.Products;
using Northwind.Domain.Models;

namespace Northwind.Api.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Category, CategoryItemModel>();

            CreateMap<Product, ProductItemModel>()
                .ForMember(d => d.SupplierName, opt => opt.MapFrom(s => s.Supplier.CompanyName))
                .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Category.CategoryName));
        }
    }
}
