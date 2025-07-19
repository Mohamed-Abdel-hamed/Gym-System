using FluentValidation;

namespace Gym.Api.Contracts.Users;

public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidator()
    {
        RuleFor(x => x.Email)
    .NotEmpty()
    .EmailAddress();

        RuleFor(x => x.Token)
    .NotEmpty();


        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .MinimumLength(8);
    }
}
