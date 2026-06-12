using Dapper;
using FitnessTracker.Core.Abstractions;
using MediatR;
using Shared.Result;

namespace FitnessTracker.Business.CQRS.Workouts.Queries.GetExerciseHistory;

public sealed class GetExerciseHistoryHandler(ISqlConnectionFactory sqlConnectionFactory)
    : IRequestHandler<GetExerciseHistoryQuery, Result<IReadOnlyList<ExerciseHistoryItemDto>>>
{
    public async Task<Result<IReadOnlyList<ExerciseHistoryItemDto>>> Handle(
        GetExerciseHistoryQuery request,
        CancellationToken cancellationToken)
    {
        using var connection = sqlConnectionFactory.CreateConnection();

        const string sql = """
            SELECT 
                w.workout_date AS Date,
                MAX(s.weight) AS MaxWeight,
                SUM(s.weight * s.repetitions) AS TotalVolume,
                SUM(s.repetitions) AS TotalReps
            FROM workouts w
            INNER JOIN exercises e ON w.id = e.workout_id
            INNER JOIN sets s ON e.id = s.exercise_id
            WHERE w.user_id = @UserId 
              AND LOWER(TRIM(e.name)) = LOWER(TRIM(@ExerciseName))
              AND (@DateFrom IS NULL OR w.workout_date >= @DateFrom)
              AND (@DateTo IS NULL OR w.workout_date <= @DateTo)
            GROUP BY w.workout_date
            ORDER BY w.workout_date DESC
            """;

        var history = await connection.QueryAsync<ExerciseHistoryItemDto>(sql, new
        {
            UserId = request.UserId,
            ExerciseName = request.ExerciseName,
            DateFrom = request.DateFrom,
            DateTo = request.DateTo
        });

        return Result<IReadOnlyList<ExerciseHistoryItemDto>>.Success(history.ToList());
    }
}
