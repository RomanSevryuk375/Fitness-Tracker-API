using FluentValidation;

namespace FitnessTracker.Business.CQRS.Workouts.Commands.RemovePhotoFromWorkout;

public sealed class RemovePhotoFromWorkoutValidator 
    : AbstractValidator<RemovePhotoFromWorkoutCommand>
{
    public RemovePhotoFromWorkoutValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();

        RuleFor(x => x.WorkoutId).NotEmpty();

        RuleFor(x => x.PhotoId).NotEmpty();
    }
}