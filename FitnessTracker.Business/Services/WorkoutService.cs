using AutoMapper;
using FitnessTracker.Business.Abstractions;
using FitnessTracker.Business.DTOs;
using FitnessTracker.Core.Abstractions;
using FitnessTracker.Core.AggregateRoots.MediaAttachments;
using FitnessTracker.Core.AggregateRoots.Workouts;
using FitnessTracker.Core.Enums;
using FitnessTracker.Core.Exceptions;
using FitnessTracker.Core.Models;
using FluentValidation;

namespace FitnessTracker.Business.Services;

public class WorkoutService : IWorkoutService
{
    private readonly IWorkoutRepository _workoutRepository;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;
    private readonly IValidator<CreateWorkoutRequest> _validator;

    public WorkoutService(
        IWorkoutRepository workoutRepository, 
        IMapper mapper, 
        IFileService fileService,
        IValidator<CreateWorkoutRequest> validator)
    {
        _workoutRepository = workoutRepository;
        _mapper = mapper;
        _fileService = fileService;
        _validator = validator;
    }

    public async Task<WorkoutDto> CreateAsync(string userId, CreateWorkoutRequest request, List<FileModel> photos, CancellationToken ct)
    {
        var validationResult = await _validator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
        {
            throw new Core.Exceptions.ValidationException(validationResult.Errors.First().ErrorMessage);
        }

        var (workout, workoutErrors) = Workout.Create(
            Guid.NewGuid().ToString(),
            userId,
            request.Title,
            (WorkoutType)request.TypeId,
            TimeSpan.FromMinutes(request.DurationMinutes),
            request.CaloriesBurned,
            request.WorkoutDate,
            DateTime.UtcNow);

        if (workoutErrors.Any())
        {
            throw new ConflictException(string.Join(", ", workoutErrors));
        }

        foreach (var ex in request.Exercises)
        {
            var (exercise, exErrors) = Exercise.Create(
                Guid.NewGuid().ToString(),
                workout!.Id,
                ex.Name,
                DateTime.UtcNow);

            if (exErrors.Any())
            {
                throw new ConflictException(string.Join(", ", exErrors));
            }

            foreach (var setReq in ex.Sets)
            {
                var (set, setErrors) = Set.Create(
                    Guid.NewGuid().ToString(),
                    exercise!.Id,
                    setReq.Reps,
                    setReq.Weight,
                    DateTime.UtcNow);

                if (setErrors.Any())
                {
                    throw new ConflictException(string.Join(", ", setErrors));
                }

                exercise.AddSet(set!);
            }

            workout.Exercises.Add(exercise!);
        }

        var uploadedFiles = new List<string>();
        try
        {
            foreach (var photo in photos)
            {
                var filePath = await _fileService.UploadFileAsync(photo.Content, photo.FileName, photo.ContentType, ct);
                uploadedFiles.Add(filePath);

                var (photoEntity, photoErrors) = Photo.Create(
                    Guid.NewGuid().ToString(),
                    workout!.Id,
                    filePath,
                    DateTime.UtcNow);

                if (!photoErrors.Any())
                {
                    workout.AddPhoto(photoEntity!);
                }
            }

            await _workoutRepository.AddAsync(workout!, ct);
        }
        catch (Exception)
        {
            foreach(var path in uploadedFiles)
            {
                await _fileService.DeleteFileAsync(path, ct);
            }
            throw;
        }

        return _mapper.Map<WorkoutDto>(workout);
    }

    public async Task<WorkoutDto?> GetByIdAsync(string userId, string workoutId, CancellationToken ct)
    {
        var workout = await _workoutRepository.GetByIdAsync(workoutId, ct);

        if (workout == null || workout.UserId != userId)
        {
            return null;
        }

        return _mapper.Map<WorkoutDto>(workout);
    }

    public async Task<int> GetCountAsync(string userId, WorkoutFilter filter, CancellationToken ct) =>
        await _workoutRepository.GetCountAsync(userId, filter, ct);

    public async Task<List<WorkoutDto>> GetUserWorkoutsAsync(string userId, WorkoutFilter filter, CancellationToken ct)
    {
        var workouts = await _workoutRepository.GetByUserIdAsync(userId, filter, ct);

        return _mapper.Map<List<WorkoutDto>>(workouts);
    }

    public async Task<string> UpdateWorkout(string userId, string id, WorkoutUpdateModel model, CancellationToken ct)
    {
        var existing = await _workoutRepository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException("Workout not found");

        if (existing.UserId != userId)
        {
            throw new UnauthorizedAccessException("You don't own this workout");
        }

        return await _workoutRepository.UpdateAsync(id, model, ct);
    }

    public async Task<string> DeleteWorkout(string userId, string id, CancellationToken ct)
    {
        var existing = await _workoutRepository.GetByIdAsync(id, ct)
           ?? throw new NotFoundException("Workout not found");

        if (existing.UserId != userId)
        {
            throw new UnauthorizedAccessException("You don't own this workout");
        }

        foreach (var photo in existing.Photos)
        {
            await _fileService.DeleteFileAsync(photo.FilePath, ct);
        }

        return await _workoutRepository.DeleteAsync(id, ct);
    }

    public async Task<(Stream FileStream, string ContentType)> GetPhotoAsync(string userId, string workoutId, string fileName, CancellationToken ct)
    {
        var workout = await _workoutRepository.GetByIdAsync(workoutId, ct)
            ?? throw new NotFoundException("Workout not found");

        if (workout.UserId != userId)
        {
            throw new UnauthorizedAccessException("You don't have access to this photo");
        }

        var photo = workout.Photos.FirstOrDefault(p => p.FilePath == fileName)
            ?? throw new NotFoundException("Photo not found in this workout");

        var stream = await _fileService.GetFileAsync(photo.FilePath, ct);

        return (stream, "image/jpeg");
    }
}
