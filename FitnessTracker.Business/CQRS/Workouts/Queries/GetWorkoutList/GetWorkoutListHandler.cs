using Dapper;
using FitnessTracker.Core.Abstractions;
using MediatR;
using Shared.Result;

namespace FitnessTracker.Business.CQRS.Workouts.Queries.GetWorkoutList;

public sealed class GetWorkoutListHandler(ISqlConnectionFactory connectionFactory) 
    : IRequestHandler<GetWorkoutListQuery, Result<IReadOnlyList<WorkoutListItemDto>>>
{
    public async Task<Result<IReadOnlyList<WorkoutListItemDto>>> Handle(
        GetWorkoutListQuery request, 
        CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();

        const string sql = """
            SELECT 
                w.id AS Id,
                w.title AS Title,
                w.type AS Type, 
                w.duration AS Duration, 
                w.calories AS CaloriesBurned, 
                w.workout_date AS WorkoutDate,
                COUNT(e.Id) AS ExercisesCount
            FROM workouts w
            LEFT JOIN exercises e ON w.Id = e.workoutId
            WHERE w.user_id = @UserId 
                AND (@Type IS NULL OR w.type = @Type)
            GROUP BY 
                w.id, w.title, w.type, w.duration, w.calories_burned, w.workout_date
            ORDER BY w.workout_date DESC
            OFFSET @Skip
            LIMIT @Take
            """;

        var typeString = request.Type?.ToString();
        var workoutList = await connection.QueryAsync<WorkoutListItemDto>(sql, new
        {
            UserId = request.UserId,
            Type = typeString,
            Skip = request.Skip,
            Take = request.Take,
        });

        return Result<IReadOnlyList<WorkoutListItemDto>>.Success(workoutList.ToList());
    }
}
