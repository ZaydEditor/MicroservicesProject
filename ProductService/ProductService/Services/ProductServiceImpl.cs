using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using ProductService.Dtos;
using ProductService.Entities;
using ProductService.Exceptions;
using ProductService.Repositories.Interfaces;
using ProductService.Services.Interfaces;

namespace ProductService.Services
{
    public class ProductServiceImpl(IProductRepository productRepo, IMapper mapper) : IProductService
    {
        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await productRepo.GetAllAsync();
            if(products == null || !products.Any())
                throw new NotFoundException("No products found");

            return mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await productRepo.GetByIdAsync(id);
            if (product == null) throw new NotFoundException("Product not found");

            return mapper.Map<ProductDto>(product);
        }

        public async Task CreateAsync(CreateProductDto createProductDto)
        {
            await productRepo.CreateAsync(mapper.Map<Product>(createProductDto));
        }

        public async Task UpdateStockAsync(int id, UpdateStockDto updateStockDto)
        {
            var product = await productRepo.GetByIdAsync(id);
            if (product == null) throw new NotFoundException("Product not found");

            product.Stock -= updateStockDto.Quantity;
            await productRepo.UpdateAsync(product);
        }

        public async Task DeleteAsync(int id)
        {
            var product = await productRepo.GetByIdAsync(id);
            if (product == null) throw new NotFoundException("Product not found");

            await productRepo.DeleteAsync(product);
        }
    }
}
