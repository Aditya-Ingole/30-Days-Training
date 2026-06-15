using FluentValidation;
using PMS.Application.DTOs.Auth;

namespace PMS.Application.Validators;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage(
                "First Name is required.");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage(
                "Last Name is required.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(
                "Email is required.")
            .EmailAddress()
            .WithMessage(
                "Invalid email format.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(
                "Password is required.")
            .MinimumLength(6)
            .WithMessage(
                "Password must be at least 6 characters.");
    }
}