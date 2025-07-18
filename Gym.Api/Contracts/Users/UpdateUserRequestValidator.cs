using FluentValidation;

namespace Gym.Api.Contracts.Users;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.FirstName) 
            .NotEmpty()
            .Length(3, 50);


        RuleFor(x => x.LastName)
        .NotEmpty()
        .Length(3, 50);
    }
}
