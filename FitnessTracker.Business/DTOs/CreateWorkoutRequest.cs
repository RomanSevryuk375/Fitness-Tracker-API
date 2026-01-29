namespace FitnessTracker.Business.DTOs;

public record CreateWorkoutRequest
(
    string Title,
    int TypeId,
    int DurationMinutes,
    double CaloriesBurned,
    DateTime WorkoutDate,
    List<CreateExerciseRequest> Exercises
);
