namespace FitnessTracker.Business.DTOs;

public record ExerciseDto
(
    string Name, 
    List<SetDto> Sets
);
