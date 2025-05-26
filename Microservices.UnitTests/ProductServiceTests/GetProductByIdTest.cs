using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using ProductService.Exceptions;
using ProductService.Entities;
using ProductService.Repositories.Interfaces;
using ProductService.Services;
using ProductService.Services.Interfaces;
using ProductService.Dtos;

namespace Microservices.UnitTests.ProductServiceTests
{
    public class GetProductByIdTest
    {
        private readonly AutoMocker _mocker = new AutoMocker();
        private readonly Fixture _fixture = new Fixture();
        private readonly IProductService _productService;

        public GetProductByIdTest()
        {
            _productService = _mocker.CreateInstance<ProductServiceImpl>();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnProduct_WhenIdIsValid()
        {
            //Arrange
            var product = _fixture.Create<Product>();
            var productDto = _fixture.Create<ProductDto>();

            _mocker.GetMock<IProductRepository>()
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(product);

            _mocker.GetMock<IMapper>()
                .Setup(r => r.Map<ProductDto>(It.IsAny<Product>()))
                .Returns(productDto);

            //Act
            var result = await _productService.GetByIdAsync(product.Id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(productDto);

        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            _mocker.GetMock<IProductRepository>()
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Product)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() =>
                _productService.GetByIdAsync(It.IsAny<int>()));
        }
    }
}
