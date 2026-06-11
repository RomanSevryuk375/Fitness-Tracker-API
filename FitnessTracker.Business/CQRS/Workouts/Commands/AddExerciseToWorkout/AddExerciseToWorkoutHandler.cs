using FitnessTracker.Core.Abstractions;
using FitnessTracker.Core.AggregateRoots.Workouts;
using FitnessTracker.Core.AggregateRoots.Workouts.ValueObjects;
using MediatR;
using Shared.Result;

namespace FitnessTracker.Business.CQRS.Workouts.Commands.AddExerciseToWorkout;

public sealed class AddExerciseToWorkoutHandler(
    IWorkoutRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<AddExerciseToWorkoutCommand, Result>
{
    public async Task<Result> Handle(
        AddExerciseToWorkoutCommand request, 
        CancellationToken cancellationToken)
    {
        var workout = await repository.GetByIdAsync(request.WorkoutId, cancellationToken);
        if (workout is null)
        {
            return Result.Failure(Error.NotFound<Workout>(
                $"Workout {request.WorkoutId} not found."));
        }

        var nameResult = ExerciseName.Create(request.Name);
        if(nameResult.IsFailure)
        {
            return Result.Failure(nameResult.Error);
        }

        var newExercise = Exercise.Create(request.WorkoutId, nameResult.Value);
        if (newExercise.IsFailure)
        {
            return Result.Failure(newExercise.Error);
        }

        var addResult = workout.AddExercise(newExercise.Value);
        if (addResult.IsFailure)
        {
            return Result.Failure(addResult.Error); 
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
