using FluentValidation;

namespace Gym.Api.Contracts.Classes;

public class ClassRequestValidator : AbstractValidator<ClassRequest>
{
    public ClassRequestValidator()
    {
        RuleFor(x=>x.Name)
            .NotEmpty()
            .Length(3,100);

        RuleFor(c => c.Duration)
                   .GreaterThanOrEqualTo(TimeSpan.FromMinutes(15))
                   .WithMessage("Gym class must be at least 15 minutes long.")
                   .LessThanOrEqualTo(TimeSpan.FromHours(2))
                   .WithMessage("Gym class cannot be longer than 2 hours.");
    }
}
