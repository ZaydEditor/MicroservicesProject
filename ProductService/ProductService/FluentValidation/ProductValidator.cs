using FluentValidation;
using ProductService.Dtos;
using ProductService.Entities;

namespace ProductService.FluentValidation
{
    public class ProductValidator : AbstractValidator<CreateProductDto>
    {
        public ProductValidator()
        {
            RuleFor(r => r.Name).NotEmpty();

            RuleFor(r => r.Price).GreaterThan(0);

            RuleFor(r => r.Stock).GreaterThanOrEqualTo(0);
        }
    }
}
