using FluentValidation;

namespace Gym.Api.Contracts.Authentications;

public class ResendConfirmationEmailRequestValidator : AbstractValidator<ResendConfirmationEmailRequest>
{
    public ResendConfirmationEmailRequestValidator()
    {
        RuleFor(x=>x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}
