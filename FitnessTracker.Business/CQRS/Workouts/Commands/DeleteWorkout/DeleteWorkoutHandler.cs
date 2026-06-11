using FitnessTracker.Business.Abstractions;
using FitnessTracker.Core.Abstractions;
using FitnessTracker.Core.AggregateRoots.Workouts;
using MediatR;
using Shared.Result;

namespace FitnessTracker.Business.CQRS.Workouts.Commands.DeleteWorkout;

public sealed class DeleteWorkoutHandler(
    IWorkoutRepository repository,
    IFileService fileService,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteWorkoutCommand, Result>
{
    public async Task<Result> Handle(DeleteWorkoutCommand request, CancellationToken cancellationToken)
    {
        var workout = await repository.GetByIdAsync(request.WorkoutId, cancellationToken);
        if (workout is null)
        {
            return Result.Failure(Error.NotFound<Workout>(
                $"Workout {request.WorkoutId} not found."));
        }

        foreach (var photo in workout.Photos)
        {
            var deleteFileResult = await fileService.DeleteFileAsync(photo.FilePath.Value, cancellationToken);
            if (deleteFileResult.IsFailure)
            {
                return Result.Failure(deleteFileResult.Error);
            }
        }

        await repository.Delete(workout);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}