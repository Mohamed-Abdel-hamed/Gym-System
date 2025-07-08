using FluentValidation;
using Gym.Api.Abstractions.Consts;

namespace Gym.Api.Contracts.Authentications;

public class RegisterRequestValidator: AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();


        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8);



        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Matches(RegexPatterns.PhoneNumber)
            .WithMessage("Phone number must be in the format +201XXXXXXXXX where X is a digit.");


        RuleFor(x => x.FirstName)
            .NotEmpty()
            .Length(3, 50);


            RuleFor(x => x.LastName)
            .NotEmpty()
            .Length(3, 50);

        RuleFor(x => x.Password)
            .NotEmpty()
            .Matches(RegexPatterns.Password)
            .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");
    }
}
