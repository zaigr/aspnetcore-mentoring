﻿using AutoMapper;
using Northwind.Api.Models.Categories;
using Northwind.Api.Models.Products;
using Northwind.Core.UseCases.Products.Create;
using Northwind.Core.UseCases.Products.Update;
using Northwind.Domain.Models;

namespace Northwind.Api.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Category, CategoryReadModel>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.CategoryId))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.CategoryName));

            CreateMap<Product, ProductReadModel>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.ProductId))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.ProductName))
                .ForMember(d => d.SupplierName, opt => opt.MapFrom(s => s.Supplier.CompanyName))
                .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Category.CategoryName));

            CreateMap<ProductCreateModel, CreateProductCommand>();

            CreateMap<ProductCreateModel, UpdateProductCommand>();
        }
    }
}
