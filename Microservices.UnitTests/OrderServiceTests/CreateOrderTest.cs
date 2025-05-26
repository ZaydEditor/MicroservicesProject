using AutoFixture;
using AutoMapper;
using Moq;
using Moq.AutoMock;
using OrderService.Dtos;
using OrderService.Entities;
using OrderService.Exceptions;
using OrderService.ProductClient;
using OrderService.Repositories.Interfaces;
using OrderService.Services;
using OrderService.Services.Interfaces;

namespace Microservices.UnitTests.OrderServiceTests
{
    public class CreateOrderTest
    {
        private readonly AutoMocker _mocker = new AutoMocker();
        private readonly Fixture _fixture = new Fixture();
        private readonly IOrderService _orderService;

        public CreateOrderTest()
        {
            _orderService = _mocker.CreateInstance<OrderServiceImpl>();
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateOrder_WhenInputIsValid()
        {
            //Arrange
            var createOrderDto = _fixture.Create<CreateOrderDto>();

            var productDto = _fixture.Build<ProductDto>()
                .With(r => r.Id, createOrderDto.ProductId)
                .With(r => r.Stock, createOrderDto.Quantity + 1)
                .Create();

            var order = _fixture.Create<Order>();

            _mocker.GetMock<IProductServiceClient>()
                .Setup(r => r.GetProductAsync(createOrderDto.ProductId))
                .ReturnsAsync(productDto);

            _mocker.GetMock<IProductServiceClient>()
                .Setup(r => r.UpdateStockAsync(createOrderDto.ProductId, createOrderDto.Quantity))
                .ReturnsAsync(true);

            _mocker.GetMock<IMapper>()
                .Setup(r => r.Map<Order>(It.IsAny<CreateOrderDto>()))
                .Returns(order);

            _mocker.GetMock<IOrderRepository>()
                .Setup(r => r.CreateAsync(It.IsAny<Order>()))
                .Returns(Task.CompletedTask);

            //Act
            await _orderService.CreateAsync(createOrderDto);

            //Assert
            _mocker.GetMock<IOrderRepository>()
                .Verify(r => r.CreateAsync(It.Is<Order>(o => o.TotalPrice == productDto.Price * order.Quantity)), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowException_WhenProductNotFound()
        {
            //Arrange
            var dto = _fixture.Create<CreateOrderDto>();

            _mocker.GetMock<IProductServiceClient>()
                .Setup(c => c.GetProductAsync(dto.ProductId))
                .ReturnsAsync((ProductDto?)null);

            //Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _orderService.CreateAsync(dto));
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowException_WhenStockIsInsufficient()
        {
            // Arrange
            var dto = _fixture.Create<CreateOrderDto>();
            var product = _fixture.Build<ProductDto>()
                                  .With(p => p.Stock, dto.Quantity - 1)
                                  .Create();

            _mocker.GetMock<IProductServiceClient>()
                   .Setup(c => c.GetProductAsync(dto.ProductId))
                   .ReturnsAsync(product);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _orderService.CreateAsync(dto));
        }
    }
}
