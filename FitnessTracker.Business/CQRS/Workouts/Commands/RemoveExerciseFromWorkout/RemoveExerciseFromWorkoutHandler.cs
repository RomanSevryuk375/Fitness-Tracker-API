using FitnessTracker.Core.Abstractions;
using FitnessTracker.Core.AggregateRoots.Workouts;
using MediatR;
using Shared.Result;

namespace FitnessTracker.Business.CQRS.Workouts.Commands.RemoveExerciseFromWorkout;

public sealed class RemoveExerciseFromWorkoutHandler(
    IWorkoutRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<RemoveExerciseFromWorkoutCommand, Result>
{
    public async Task<Result> Handle(
        RemoveExerciseFromWorkoutCommand request, 
        CancellationToken cancellationToken)
    {
        var workout = await repository.GetByIdAsync(request.WorkoutId, cancellationToken);
        if (workout is null)
        {
            return Result.Failure(Error.NotFound<Workout>(
                $"Workout {request.WorkoutId} not found."));
        }

        var removeResult = workout.RemoveExercise(request.ExerciseId);

        if (removeResult.IsFailure)
        {
            return Result.Failure(removeResult.Error);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}