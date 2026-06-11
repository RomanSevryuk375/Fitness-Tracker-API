using FitnessTracker.Core.Abstractions;
using FitnessTracker.Core.AggregateRoots.Workouts;
using MediatR;
using Shared.Result;

namespace FitnessTracker.Business.CQRS.Workouts.Commands.RemoveSetFromExercise;

public sealed class RemoveSetFromExerciseHandler(
    IWorkoutRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<RemoveSetFromExerciseCommand, Result>
{
    public async Task<Result> Handle(
        RemoveSetFromExerciseCommand request, 
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

        var removeResult = exercise.RemoveSet(request.SetId);
        if (removeResult.IsFailure)
        {
            return Result.Failure(removeResult.Error);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}