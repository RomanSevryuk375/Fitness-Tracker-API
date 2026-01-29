namespace FitnessTracker.Business.DTOs;

public record WorkoutDto
(
    string Id,
    string Title,
    string Type,
    List<string> ProgressPhotos,
    TimeSpan Duration,
    double CaloriesBurned,
    DateTime WorkoutDate,
    List<ExerciseDto> Exercises
);