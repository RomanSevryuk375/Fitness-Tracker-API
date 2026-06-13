using FitnessTracker.Business.Abstractions;
using FitnessTracker.Core.Abstractions;
using FitnessTracker.Core.AggregateRoots.Workouts;
using MediatR;
using Shared.Result;

namespace FitnessTracker.Business.CQRS.Behaviors;

public sealed class SecureBehavior<TRequest, TResponse>(IWorkoutRepository repository)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>, IUserBoundWorkout
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var workout = await repository
            .GetByIdAsync(request.WorkoutId, cancellationToken);

        if (workout is null)
        {
            BehaviorHelpers.CreateFailedResult<TResponse>(
                Error.NotFound<Workout>($"Workout {request.WorkoutId} not found."));
        }

        if (workout!.UserId != request.UserId)
        {
            BehaviorHelpers.CreateFailedResult<TResponse>(
                Error.Conflict<TRequest>("Access denied"));
        }

        return await next();
    }
}
