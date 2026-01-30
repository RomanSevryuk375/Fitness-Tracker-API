namespace FitnessTracker.Core.Models;

public record FileModel
(
    Stream Content, 
    string FileName, 
    string ContentType
);
