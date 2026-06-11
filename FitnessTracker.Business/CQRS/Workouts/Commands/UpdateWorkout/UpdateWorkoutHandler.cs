using FitnessTracker.Core.Abstractions;
using FitnessTracker.Core.AggregateRoots.Workouts;
using FitnessTracker.Core.AggregateRoots.Workouts.ValueObjects;
using MediatR;
using Shared.Result;

namespace FitnessTracker.Business.CQRS.Workouts.Commands.UpdateWorkout;

public sealed class UpdateWorkoutHandler(
    IWorkoutRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateWorkoutCommand, Result>
{
    public async Task<Result> Handle(
        UpdateWorkoutCommand request, 
        CancellationToken cancellationToken)
    {
        var workout = await repository.GetByIdAsync(request.WorkoutId, cancellationToken);
        if (workout is null)
        {
            return Result.Failure(Error.NotFound<Workout>(
                $"Workout {request.WorkoutId} not found."));
        }

        WorkoutTitle? newTitle = request.Title is not null 
            ? WorkoutTitle.Create(request.Title).Value 
            : null;

        WorkoutDuration? newDuration = request.Duration.HasValue 
            ? WorkoutDuration.Create(request.Duration.Value).Value
            : null;

        Calories? newCalories = request.CaloriesBurned.HasValue 
            ? Calories.Create(request.CaloriesBurned.Value).Value 
            : null;

        WorkoutDate? newDate = request.WorkoutDate.HasValue 
            ? WorkoutDate.Create(request.WorkoutDate.Value, DateTime.UtcNow).Value 
            : null;

        var updateResult = workout.Update(newTitle, request.Type, newDuration, newCalories, newDate);
        if (updateResult.IsFailure)
        {
            return Result.Failure(updateResult.Error);
        }

        await repository.Update(workout); 
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}