using FluentValidation;

namespace FitnessTracker.Business.CQRS.Workouts.Commands.AttachPhotoToWorkout;

public sealed class AttachPhotoToWorkoutValidator : AbstractValidator<AttachPhotoToWorkoutCommand>
{
    public AttachPhotoToWorkoutValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.WorkoutId).NotEmpty();

        RuleFor(x => x.FileName).NotEmpty();
        RuleFor(x => x.ContentType).NotEmpty();

        RuleFor(x => x.Content)
            .NotNull()
            .Must(stream => stream.Length > 0)
            .WithMessage("File content cannot be empty.");
    }
}