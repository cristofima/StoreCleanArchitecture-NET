using AutoMapper;
using Store.ApplicationCore.DTOs;
using Store.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.ApplicationCore.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<CreateProductRequest, Product>();
            CreateMap<Product, ProductResponse>();
        }
    }
}
