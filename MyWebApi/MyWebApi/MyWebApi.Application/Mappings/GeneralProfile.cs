using MyWebApi.Application.Features.Products.Commands.CreateProduct;
using MyWebApi.Application.Features.Products.Queries.GetAllProducts;
using AutoMapper;
using MyWebApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApi.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Product, GetAllProductsViewModel>().ReverseMap();
            CreateMap<CreateProductCommand, Product>();
            CreateMap<GetAllProductsQuery, GetAllProductsParameter>();
        }
    }
}
