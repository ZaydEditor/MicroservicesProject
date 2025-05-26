using AutoMapper;
using OrderService.Dtos;
using OrderService.Entities;

namespace OrderService.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Order, OrderDto>();

            CreateMap<OrderDto, Order>();
            CreateMap<CreateOrderDto, Order>();
        }
    }
}
