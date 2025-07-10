using FluentValidation;
using Gym.Api.Abstractions.Consts;

namespace Gym.Api.Contracts.Trainers;

public class RegisterTrainerRequestValidator : AbstractValidator<RegisterTrainerRequest>
{
    public RegisterTrainerRequestValidator()
    {
        RuleFor(x => x.Info.Email)
         .NotEmpty()
         .EmailAddress();


        RuleFor(x => x.Info.Password)
            .NotEmpty()
            .MinimumLength(8);



        RuleFor(x => x.Info.PhoneNumber)
            .NotEmpty()
            .Matches(RegexPatterns.PhoneNumber)
            .WithMessage("Phone number must be in the format 01XXXXXXXXX where X is a digit.");


        RuleFor(x => x.Info.FirstName)
            .NotEmpty()
            .Length(3, 50);


        RuleFor(x =>   x.Info.LastName)
        .NotEmpty()
        .Length(3, 50);

        RuleFor(x => x.Info.Password)
            .NotEmpty()
            .Matches(RegexPatterns.Password)
            .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");

        RuleFor(x => x.HireDate)
        .NotEmpty()
        .Must(CheckHireDate)
        .WithMessage("Hire date must be today or befor.");

    }
    public bool CheckHireDate(DateOnly hireDate)
    {
        return hireDate <= DateOnly.FromDateTime(DateTime.Today);
    }


}
