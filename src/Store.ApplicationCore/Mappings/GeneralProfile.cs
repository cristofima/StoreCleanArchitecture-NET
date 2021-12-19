using AutoMapper;
using Store.ApplicationCore.DTOs;
using Store.ApplicationCore.Entities;

namespace Store.ApplicationCore.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<CreateProductRequest, Product>()
                .ForMember(dest =>
                    dest.Id,
                    opt => opt.Ignore()
                )
                .ForMember(dest =>
                    dest.Stock,
                    opt => opt.Ignore()
                )
                .ForMember(dest =>
                    dest.CreatedAt,
                    opt => opt.Ignore()
                )
                .ForMember(dest =>
                    dest.UpdatedAt,
                    opt => opt.Ignore()
                );

            CreateMap<Product, ProductResponse>();
            CreateMap<Product, SingleProductResponse>();
        }
    }
}