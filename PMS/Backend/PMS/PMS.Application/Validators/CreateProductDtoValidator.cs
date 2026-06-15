using FluentValidation;
using PMS.Application.DTOs.Product;

namespace PMS.Application.Validators
{
    public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Product Name is required").MaximumLength(100);

            RuleFor(x => x.Description).NotEmpty().WithMessage("Description required");

            RuleFor(x => x.Price).GreaterThan(10).WithMessage("Price must be greater than zero");
        }
    }
}
