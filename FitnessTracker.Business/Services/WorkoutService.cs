using AutoMapper;
using FitnessTracker.Business.DTOs;
using FitnessTracker.Core.Abstractions;
using FitnessTracker.Core.Entities;
using FitnessTracker.Core.Enums;
using FitnessTracker.Core.Exceptions;
using FitnessTracker.Core.Models;

namespace FitnessTracker.Business.Services;

public class WorkoutService : IWorkoutService
{
    private readonly IWorkoutRepository _workoutRepository;
    private readonly IMapper _mapper;

    public WorkoutService(IWorkoutRepository workoutRepository, IMapper mapper)
    {
        _workoutRepository = workoutRepository;
        _mapper = mapper;
    }

    public async Task<WorkoutDto> CreateAsync(string userId, CreateWorkoutRequest request, CancellationToken ct)
    {
        var (workout, workoutErrors) = WorkoutEntity.Create(
            Guid.NewGuid().ToString(),
            userId,
            request.Title,
            (WorkoutType)request.TypeId,
            TimeSpan.FromMinutes(request.DurationMinutes),
            request.CaloriesBurned,
            request.WorkoutDate,
            DateTime.UtcNow);

        if (workoutErrors.Any())
            throw new ConflictException(string.Join(", ", workoutErrors));

        foreach (var ex in request.Exercises)
        {
            var (exercise, exErrors) = ExerciseEntity.Create(
                Guid.NewGuid().ToString(),
                workout!.Id,
                ex.Name,
                DateTime.UtcNow);

            if (exErrors.Any())
                throw new ConflictException(string.Join(", ", exErrors));

            foreach (var setReq in ex.Sets)
            {
                var (set, setErrors) = SetEntity.Create(
                    Guid.NewGuid().ToString(),
                    exercise!.Id,
                    setReq.Reps,
                    setReq.Weight,
                    DateTime.UtcNow);

                if (setErrors.Any())
                    throw new ConflictException(string.Join(", ", setErrors));

                exercise.Sets.Add(set!);
            }

            workout.Exercises.Add(exercise!);
        }

        await _workoutRepository.AddAsync(workout!, ct);

        return _mapper.Map<WorkoutDto>(workout);
    }

    public async Task<WorkoutDto?> GetByIdAsync(string userId, string workoutId, CancellationToken ct)
    {
        var workout = await _workoutRepository.GetByIdAsync(workoutId, ct);

        if (workout == null || workout.UserId != userId)
            return null;

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
            throw new UnauthorizedAccessException("You don't own this workout");

        return await _workoutRepository.UpdateAsync(id, model, ct);
    }

    public async Task<string> DeleteWorkout(string userId, string id, CancellationToken ct)
    {
        var existing = await _workoutRepository.GetByIdAsync(id, ct)
           ?? throw new NotFoundException("Workout not found");

        if (existing.UserId != userId)
            throw new UnauthorizedAccessException("You don't own this workout");

        return await _workoutRepository.DeleteAsync(id, ct);
    }
}
