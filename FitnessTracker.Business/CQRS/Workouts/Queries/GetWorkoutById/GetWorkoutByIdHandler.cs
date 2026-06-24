using Dapper;
using FitnessTracker.Business.CQRS.Workouts.Queries.GetWorkoutById.DTOs;
using FitnessTracker.Core.Abstractions;
using MediatR;
using Shared.Result;

namespace FitnessTracker.Business.CQRS.Workouts.Queries.GetWorkoutById;

public sealed class GetWorkoutByIdHandler(ISqlConnectionFactory sqlConnectionFactory)
    : IRequestHandler<GetWorkoutByIdQuery, Result<WorkoutDetailsDto>>
{
    public async Task<Result<WorkoutDetailsDto>> Handle(
        GetWorkoutByIdQuery request,
        CancellationToken cancellationToken)
    {
        using var connection = sqlConnectionFactory.CreateConnection();

        const string sql = """
            SELECT 
                id, title, type, duration, calories AS CaloriesBurned, workout_date AS WorkoutDate 
            FROM workouts 
            WHERE id = @Id AND user_id = @UserId;

            SELECT 
                e.id AS ExerciseId, e.name AS EserciseName, 
                s.repetitions AS Reps, s.weight AS WeightValue
            FROM exercises e
            LEFT JOIN sets s ON e.id = s.exercise_id
            WHERE e.workout_id = @Id;

            SELECT file_path AS FilePath 
            FROM photos 
            WHERE workout_id = @Id;
            """;

        using var multi = await connection.QueryMultipleAsync(sql, new
        {
            Id = request.WorkoutId,
            UserId = request.UserId
        });

        var workout = await multi.ReadFirstOrDefaultAsync<WorkoutDetailsDto>();
        if (workout == null)
        {
            return Result<WorkoutDetailsDto>.Failure(Error.NotFound<WorkoutDetailsDto>(
                "Workout not found"));
        }

        var exercisesAndSets = (await multi.ReadAsync<dynamic>()).ToList();

        workout.Exercises = exercisesAndSets
            .GroupBy(x => x.exerciseid)
            .Select(group => new ExerciseDto
            {
                Id = group.Key,
                EserciseName = group.First().esercisename,
                Sets = group
                        .Where(x => x.reps is not null)
                        .Select(s => new SetDto
                        {
                            Reps = s.reps,
                            Weight = $"{s.weightvalue} kg"
                        }).ToList()
            }).ToList();

        var photos = await multi.ReadAsync<PhotoDto>();
        workout.Photos = photos.ToList();

        return Result<WorkoutDetailsDto>.Success(workout);
    }
}
