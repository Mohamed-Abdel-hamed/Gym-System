using FluentValidation;

namespace Gym.Api.Contracts.Roles;

public class RoleRequestValidator : AbstractValidator<RoleRequest>
{
    public RoleRequestValidator()
    {
        RuleFor(x=>x.Name)
            .NotEmpty()
            .Length(3,100);
    }
}
