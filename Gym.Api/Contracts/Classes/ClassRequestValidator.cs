using FluentValidation;

namespace Gym.Api.Contracts.Classes;

public class ClassRequestValidator : AbstractValidator<ClassRequest>
{
    public ClassRequestValidator()
    {
        RuleFor(x=>x.Title)
            .NotEmpty()
            .Length(3,100);

        RuleFor(c => c.Duration)
                   .GreaterThanOrEqualTo(15)
                   .WithMessage("Gym class must be at least 15 minutes long.")
                   .LessThanOrEqualTo(120)
                   .WithMessage("Gym class cannot be longer than 2 hours.");

        RuleFor(x => x.Capacity)
            .GreaterThanOrEqualTo(10)
           .LessThanOrEqualTo(25);
    }
}
