using FluentValidation;

namespace Gym.Api.Contracts.SubscriptionPlans
{
    public class SubscriptionPlanRequestValidator : AbstractValidator<SubscriptionPlanRequest>
    {
        public SubscriptionPlanRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be zero or positive.");

            RuleFor(x => x.DurationInDays)
                .GreaterThan(0).WithMessage("Duration must be greater than 0 days.");

            RuleFor(x => x.MaxClassBookingsPerDay)
                .GreaterThanOrEqualTo(0).WithMessage("Max class bookings per day cannot be negative.");

            RuleFor(x => x.MaxClassBookingsInFuture)
                .GreaterThanOrEqualTo(0).WithMessage("Max class bookings in future cannot be negative.");

            RuleFor(x => x.MaxFreezesPerYear)
                .GreaterThanOrEqualTo(0).WithMessage("Max freezes per year cannot be negative.");

            RuleFor(x => x.MaxFreezeDays)
                .GreaterThanOrEqualTo(0).WithMessage("Max freeze days cannot be negative.");

            // Example of a custom rule if you want logical consistency
            RuleFor(x => x.MaxFreezeDays)
                .LessThanOrEqualTo(x => x.DurationInDays)
                .WithMessage("Max freeze days cannot exceed the plan duration.");
        }
    }
}
