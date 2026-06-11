using FitnessTracker.Core.AggregateRoots.Workouts.ValueObjects;
using FluentValidation;

namespace FitnessTracker.Business.CQRS.Workouts.Commands.AddSetToExercise;

public class AddSetToExerciseValidator : AbstractValidator<AddSetToExerciseCommand>
{
    private const int MinWeight = 0;

    public AddSetToExerciseValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.WorkoutId)
            .NotEmpty();

        RuleFor(x => x.ExerciseId)
            .NotEmpty();

        RuleFor(x => x.Reps)
            .NotEmpty()
            .GreaterThan(Repetitions.MinValue)
            .LessThanOrEqualTo(Repetitions.MaxValue);

        RuleFor(x => x.Weight)
            .GreaterThanOrEqualTo(MinWeight);
    }
}