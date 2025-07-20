using FluentValidation;

namespace Gym.Api.Contracts.Authentications;

public class ForgetPasswordRequestValidator : AbstractValidator<ForgetPasswordRequest>
{
    public ForgetPasswordRequestValidator()
    {
        RuleFor(x => x.Email)
   .NotEmpty()
   .EmailAddress();
    }
}
