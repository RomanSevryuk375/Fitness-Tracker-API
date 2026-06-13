using FitnessTracker.Business.Abstractions;
using FitnessTracker.Core.Abstractions;
using FitnessTracker.Core.AggregateRoots.Workouts;
using MediatR;
using Shared.Result;

namespace FitnessTracker.Business.CQRS.Workouts.Commands.RemovePhotoFromWorkout;

public sealed class RemovePhotoFromWorkoutHandler(
    IWorkoutRepository repository,
    IFileService fileService,
    IUnitOfWork unitOfWork) : IRequestHandler<RemovePhotoFromWorkoutCommand, Result>
{
    public async Task<Result> Handle(
        RemovePhotoFromWorkoutCommand request,
        CancellationToken cancellationToken)
    {
        var workout = await repository.GetByIdAsync(request.WorkoutId, cancellationToken);
        if (workout is null)
        {
            return Result.Failure(Error.NotFound<Workout>(
                $"Workout {request.WorkoutId} not found."));
        }

        var photo = workout.Photos.FirstOrDefault(p => p.Id == request.PhotoId);
        if (photo is null)
        {
            return Result.Failure(Error.NotFound<Workout>(
                "Photo not found in this workout."));
        }

        var fileNameInStorage = photo.FilePath.Value;

        var removeResult = workout.RemovePhoto(request.PhotoId);
        if (removeResult.IsFailure)
        {
            return Result.Failure(removeResult.Error);
        }

        var deleteFileResult = await fileService.DeleteFileAsync(
            fileNameInStorage, cancellationToken);
        if (deleteFileResult.IsFailure)
        {
            return Result.Failure(deleteFileResult.Error);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}