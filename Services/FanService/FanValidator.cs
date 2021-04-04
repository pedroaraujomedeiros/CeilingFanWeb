using Domain.Entities;
using FluentValidation;
using System;

namespace Services.FanService
{
    public class FanValidator : AbstractValidator<Fan>
    {
        public FanValidator()
        {
            RuleFor(x => x)
                .NotNull()
                .OnAnyFailure(x => throw new ArgumentNullException($"The object cannot be null."));

            RuleFor(x => x.Description)
                .NotNull().WithMessage("Description is required.")
                .MinimumLength(1).WithMessage("Description is required.")
                .MaximumLength(50).WithMessage("Description cannot be longer than 50 characters.");

        }
    }
}
