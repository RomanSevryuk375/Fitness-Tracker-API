using FitnessTracker.Core.Abstractions;
using FitnessTracker.Core.AggregateRoots.Workouts;
using FitnessTracker.Core.AggregateRoots.Workouts.ValueObjects;
using MediatR;
using Shared.Result;

namespace FitnessTracker.Business.CQRS.Workouts.Commands.UpdateSet;

public sealed class UpdateSetHandler(
    IWorkoutRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateSetCommand, Result>
{
    public async Task<Result> Handle(UpdateSetCommand request, CancellationToken cancellationToken)
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

        var targetSet = exercise.Sets.FirstOrDefault(s => s.Id == request.SetId);
        if (targetSet is null)
        {
            return Result.Failure(Error.NotFound<Set>(
                $"Set {request.SetId} not found."));
        }

        Repetitions? newReps = request.Reps.HasValue
            ? Repetitions.Create(request.Reps.Value).Value
            : null;

        Weight? newWeight = request.Weight.HasValue
            ? Weight.Create(request.Weight.Value).Value
            : null;

        var updateResult = targetSet.Update(newReps, newWeight);
        if (updateResult.IsFailure)
        {
            return Result.Failure(updateResult.Error);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}