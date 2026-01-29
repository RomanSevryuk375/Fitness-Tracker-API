namespace FitnessTracker.Business.DTOs;

public record CreateExerciseRequest
(
    string Name, 
    List<CreateSetRequest> Sets
);
