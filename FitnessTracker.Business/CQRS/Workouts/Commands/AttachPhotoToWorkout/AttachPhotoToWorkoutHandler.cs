using FitnessTracker.Business.Abstractions;
using FitnessTracker.Core.Abstractions;
using FitnessTracker.Core.AggregateRoots.Workouts;
using FitnessTracker.Core.AggregateRoots.Workouts.ValueObjects;
using MediatR;
using Shared.Result;

namespace FitnessTracker.Business.CQRS.Workouts.Commands.AttachPhotoToWorkout;

public sealed class AttachPhotoToWorkoutHandler(
    IWorkoutRepository repository,
    IFileService fileService,
    IUnitOfWork unitOfWork) : IRequestHandler<AttachPhotoToWorkoutCommand, Result>
{
    public async Task<Result> Handle(
        AttachPhotoToWorkoutCommand request, 
        CancellationToken cancellationToken)
    {
        var workout = await repository.GetByIdAsync(request.WorkoutId, cancellationToken);
        if (workout is null)
        {
            return Result.Failure(Error.NotFound<Workout>(
                $"Workout {request.WorkoutId} not found."));
        }

        if (workout.UserId.ToString() != request.UserId.ToString())
        {
            return Result.Failure(Error.Conflict<Workout>(
                "Access denied."));
        }

        var uniqueFileName = $"{Guid.NewGuid()}_{request.FileName}";
        var filePathResult = FilePath.Create(uniqueFileName);

        if (filePathResult.IsFailure)
        {
            return Result.Failure(filePathResult.Error);
        }

        var uploadResult = await fileService.UploadFileAsync(
            request.Content,
            uniqueFileName,
            request.ContentType,
            cancellationToken);
        if (uploadResult.IsFailure)
        {
            return Result.Failure(uploadResult.Error);
        }

        var photoResult = Photo.Create(workout.Id, filePathResult.Value);
        if (photoResult.IsFailure)
        {
            return Result.Failure(photoResult.Error);
        }

        var addResult = workout.AddPhoto(photoResult.Value);
        if (addResult.IsFailure)
        {
            await fileService.DeleteFileAsync(uniqueFileName, cancellationToken);
            return Result.Failure(addResult.Error);
        }

        try
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            await fileService.DeleteFileAsync(uniqueFileName, cancellationToken);
            throw; 
        }

        return Result.Success();
    }
}