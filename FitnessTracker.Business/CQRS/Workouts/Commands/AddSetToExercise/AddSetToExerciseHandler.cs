using FitnessTracker.Core.Abstractions;
using FitnessTracker.Core.AggregateRoots.Workouts;
using FitnessTracker.Core.AggregateRoots.Workouts.ValueObjects;
using MediatR;
using Shared.Result;

namespace FitnessTracker.Business.CQRS.Workouts.Commands.AddSetToExercise;

public sealed class AddSetToExerciseHandler(
    IWorkoutRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<AddSetToExerciseCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        AddSetToExerciseCommand request,
        CancellationToken cancellationToken)
    {
        var workout = await repository.GetByIdAsync(request.WorkoutId, cancellationToken);
        if (workout is null)
        {
            return Result<Guid>.Failure(Error.NotFound<Workout>(
                $"Workout {request.WorkoutId} not found."));
        }

        var exercise = workout.Exercises.FirstOrDefault(e => e.Id == request.ExerciseId);
        if (exercise is null)
        {
            return Result<Guid>.Failure(Error.NotFound<Exercise>(
                $"Exercise {request.ExerciseId} not found in workout {request.WorkoutId}."));
        }

        var repsResult = Repetitions.Create(request.Reps);
        var weightResult = Weight.Create(request.Weight);

        if (repsResult.IsFailure)
        {
            return Result<Guid>.Failure(repsResult.Error);
        }
        if (weightResult.IsFailure)
        {
            return Result<Guid>.Failure(weightResult.Error);
        }

        var newSetResult = Set.Create(request.ExerciseId, repsResult.Value, weightResult.Value);
        if (newSetResult.IsFailure)
        {
            return Result<Guid>.Failure(newSetResult.Error);
        }

        var addResult = exercise.AddSet(newSetResult.Value);
        if (addResult.IsFailure)
        {
            return Result<Guid>.Failure(addResult.Error);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(newSetResult.Value.Id);
    }
}