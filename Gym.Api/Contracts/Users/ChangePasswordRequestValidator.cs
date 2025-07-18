using FluentValidation;

namespace Gym.Api.Contracts.Users;

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty();

        RuleFor(x => x.NewPassword)
           .NotEmpty()
           .NotEqual(x => x.CurrentPassword)
           .WithMessage("new password can nt be same as current password");
    }
}
