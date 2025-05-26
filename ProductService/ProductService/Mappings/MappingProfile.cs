using AutoMapper;
using ProductService.Dtos;
using ProductService.Entities;

namespace ProductService.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>();

            CreateMap<CreateProductDto, Product>();
            CreateMap<ProductDto, Product>();
        }
    }
}
