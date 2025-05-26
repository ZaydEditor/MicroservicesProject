using AutoFixture;
using AutoMapper;
using Moq;
using Moq.AutoMock;
using ProductService.Dtos;
using ProductService.Entities;
using ProductService.FluentValidation;
using ProductService.Repositories.Interfaces;
using ProductService.Services;
using ProductService.Services.Interfaces;

namespace Microservices.UnitTests.ProductServiceTests
{
    public class CreateProductTest
    {
        private readonly AutoMocker _mocker = new AutoMocker();
        private readonly Fixture _fixture = new Fixture();
        private readonly IProductService _productService;
        private readonly ProductValidator _productValidator;

        public CreateProductTest()
        {
            _productValidator = new ProductValidator();
            _productService = _mocker.CreateInstance<ProductServiceImpl>();
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateProduct_WhenInputIsValid()
        {
            //Arrange
            var productDto = _fixture.Create<CreateProductDto>();
            var product = _fixture.Create<Product>();

            _mocker.GetMock<IProductRepository>()
                .Setup(r => r.CreateAsync(It.IsAny<Product>()))
                .Returns(Task.CompletedTask);

            _mocker.GetMock<IMapper>()
                .Setup(r => r.Map<Product>(It.IsAny<CreateProductDto>()))
                .Returns(product);

            //Act

            await _productService.CreateAsync(productDto);

            //Assert
            _mocker.GetMock<IProductRepository>()
                .Verify(r => r.CreateAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowValidationException_WhenInputIsInvalid()
        {
            //Arrange
            var productDto = _fixture.Create<CreateProductDto>();
            productDto.Name = "";
            var result = _productValidator.Validate(productDto);

            //Act & Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Name");
        }
    }
}
