using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using OrderService.Dtos;
using OrderService.Entities;
using OrderService.Exceptions;
using OrderService.Repositories.Interfaces;
using OrderService.Services;
using OrderService.Services.Interfaces;

namespace Microservices.UnitTests.OrderServiceTests
{
    public class GetOrderByIdTest
    {
        private readonly AutoMocker _mocker = new AutoMocker();
        private readonly Fixture _fixture = new Fixture();
        private readonly IOrderService _orderService;

        public GetOrderByIdTest()
        {
            _orderService = _mocker.CreateInstance<OrderServiceImpl>();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnOrder_WhenIdIsValid()
        {
            // Arrange
            var order = _fixture.Create<Order>();
            var orderDto = _fixture.Create<OrderDto>();

            _mocker.GetMock<IOrderRepository>()
                .Setup(r => r.GetByIdAsync(order.Id))
                .ReturnsAsync(order);

            _mocker.GetMock<IMapper>()
                .Setup(r => r.Map<OrderDto>(order))
                .Returns(orderDto);

            // Act
            var result = await _orderService.GetByIdAsync(order.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(orderDto);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowNotFoundException_WhenOrderDoesNotExist()
        {
            // Arrange

            _mocker.GetMock<IOrderRepository>()
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Order)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() =>
                _orderService.GetByIdAsync(It.IsAny<int>()));
        }
    }
}
