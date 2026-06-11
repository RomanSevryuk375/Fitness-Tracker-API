using FitnessTracker.Core.Abstractions;
using FitnessTracker.Core.AggregateRoots.Workouts;
using FitnessTracker.Core.AggregateRoots.Workouts.ValueObjects;
using MediatR;
using Shared.Result;

namespace FitnessTracker.Business.CQRS.Workouts.Commands.CreateWorkout;

public sealed class CreateWorkoutHandler(
    IWorkoutRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateWorkoutCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateWorkoutCommand request, CancellationToken cancellationToken)
    {
        var titleResult = WorkoutTitle.Create(request.Title);
        var durationResult = WorkoutDuration.Create(request.Duration);
        var caloriesResult = Calories.Create(request.CaloriesBurned);
        var dateResult = WorkoutDate.Create(request.WorkoutDate, DateTime.UtcNow);

        if (titleResult.IsFailure)
        {
            return Result<Guid>.Failure(titleResult.Error);
        }

        if (durationResult.IsFailure)
        {
            return Result<Guid>.Failure(durationResult.Error);
        }

        if (caloriesResult.IsFailure)
        {
            return Result<Guid>.Failure(caloriesResult.Error);
        }

        if (dateResult.IsFailure)
        {
            return Result<Guid>.Failure(dateResult.Error);
        }

        var workoutResult = Workout.Create(
            request.UserId, titleResult.Value, request.Type,
            durationResult.Value, caloriesResult.Value, dateResult.Value);
        if (workoutResult.IsFailure)
        {
            return Result<Guid>.Failure(workoutResult.Error);
        }

        var workout = workoutResult.Value;

        await repository.AddAsync(workout, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(workout.Id);
    }
}