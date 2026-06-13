using Dapper;
using FitnessTracker.Core.Abstractions;
using MediatR;
using Shared.Result;
using System.Text;

namespace FitnessTracker.Business.CQRS.Workouts.Queries.GetWorkoutList;

public sealed class GetWorkoutListHandler(
    ISqlConnectionFactory connectionFactory)
    : IRequestHandler<GetWorkoutListQuery, Result<IReadOnlyList<WorkoutListItemDto>>>
{
    public async Task<Result<IReadOnlyList<WorkoutListItemDto>>> Handle(
        GetWorkoutListQuery request,
        CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();

        var sqlBuilder = new StringBuilder();

        sqlBuilder.AppendLine("""
            SELECT 
                w.id AS Id,
                w.title AS Title,
                w.type AS Type, 
                w.duration AS Duration, 
                w.calories AS CaloriesBurned, 
                w.workout_date AS WorkoutDate,
                COUNT(e.id) AS ExercisesCount
            FROM workouts w
            LEFT JOIN exercises e ON w.id = e.workout_id
            WHERE w.user_id = @UserId
            """);

        var typeString = request.Type?.ToString();

        if (!string.IsNullOrEmpty(typeString))
        {
            sqlBuilder.AppendLine("  AND w.type = @Type");
        }

        sqlBuilder.AppendLine("""
            GROUP BY 
                w.id, w.title, w.type, w.duration, w.calories, w.workout_date
            ORDER BY w.workout_date DESC
            LIMIT @Take OFFSET @Skip
            """);

        var workoutList = await connection.QueryAsync<WorkoutListItemDto>(
            sqlBuilder.ToString(),
            new
            {
                UserId = request.UserId,
                Type = typeString,
                Skip = request.Skip,
                Take = request.Take,
            });

        return Result<IReadOnlyList<WorkoutListItemDto>>.Success(workoutList.ToList());
    }
}