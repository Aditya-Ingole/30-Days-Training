using FluentValidation;
using PMS.Application.DTOs.Product;

namespace PMS.Application.Validators
{
    public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductDtoValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

            RuleFor(x => x.Description)
                .NotEmpty();

            RuleFor(x => x.Price)
                .GreaterThan(0);
        }
    }
}
