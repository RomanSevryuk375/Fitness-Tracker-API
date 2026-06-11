using FitnessTracker.Core.Abstractions;
using FitnessTracker.Core.AggregateRoots.Workouts;
using FitnessTracker.Core.AggregateRoots.Workouts.ValueObjects;
using MediatR;
using Shared.Result;

namespace FitnessTracker.Business.CQRS.Workouts.Commands.RenameExercise;

public sealed class RenameExerciseHandler(
    IWorkoutRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<RenameExerciseCommand, Result>
{
    public async Task<Result> Handle(
        RenameExerciseCommand request, 
        CancellationToken cancellationToken)
    {
        var workout = await repository.GetByIdAsync(request.WorkoutId, cancellationToken);
        if (workout is null)
        {
            return Result.Failure(Error.NotFound<Workout>(
                $"Workout {request.WorkoutId} not found."));
        }

        var exercise = workout.Exercises.FirstOrDefault(e => e.Id == request.ExerciseId);
        if (exercise is null)
        {
            return Result.Failure(Error.NotFound<Exercise>(
                $"Exercise {request.ExerciseId} not found."));
        }

        var newNameResult = ExerciseName.Create(request.NewName);
        if (newNameResult.IsFailure)
        {
            return Result.Failure(newNameResult.Error);
        }

        exercise.Rename(newNameResult.Value);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}