using FitnessTracker.Core.AggregateRoots.Workouts.ValueObjects;
using FluentValidation;

namespace FitnessTracker.Business.CQRS.Workouts.Commands.UpdateSet;

public sealed class UpdateSetValidator : AbstractValidator<UpdateSetCommand>
{
    private const int MinWeight = 0;

    public UpdateSetValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();

        RuleFor(x => x.WorkoutId).NotEmpty();

        RuleFor(x => x.ExerciseId).NotEmpty();

        RuleFor(x => x.SetId).NotEmpty();

        RuleFor(x => x.Reps)
            .GreaterThan(Repetitions.MinValue)
            .LessThanOrEqualTo(Repetitions.MaxValue)
            .When(x => x.Reps.HasValue);

        RuleFor(x => x.Weight)
            .GreaterThanOrEqualTo(MinWeight)
            .When(x => x.Weight.HasValue);
    }
}