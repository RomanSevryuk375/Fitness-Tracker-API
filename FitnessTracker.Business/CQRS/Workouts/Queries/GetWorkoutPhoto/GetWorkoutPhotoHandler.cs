using FitnessTracker.Business.Abstractions;
using FitnessTracker.Core.Abstractions;
using FitnessTracker.Core.AggregateRoots.Workouts;
using MediatR;
using Shared.Result;

namespace FitnessTracker.Business.CQRS.Workouts.Queries.GetWorkoutPhoto;

public sealed class GetWorkoutPhotoHandler(
    IPhotoRepository photoRepository,
    IFileService fileService) : IRequestHandler<GetWorkoutPhotoQuery, Result<FileStreamResponse>>
{
    public async Task<Result<FileStreamResponse>> Handle(
        GetWorkoutPhotoQuery request,
        CancellationToken cancellationToken)
    {
        var photo = await photoRepository.GetByFilePathAsync(request.FileName, cancellationToken);
        if (photo is null || photo.Workout?.UserId != request.UserId)
        {
            return Result<FileStreamResponse>.Failure(Error.NotFound<Photo>(
                "Photo not found or access denied."));
        }

        var streamResult = await fileService.GetFileAsync(request.FileName, cancellationToken);
        if (streamResult.IsFailure)
        {
            return Result<FileStreamResponse>.Failure(streamResult.Error);
        }

        return Result<FileStreamResponse>.Success(
            new FileStreamResponse
            {
                Stream = streamResult.Value,
                ContentType = "image/jpeg",
                FileName = request.FileName
            });
    }
}