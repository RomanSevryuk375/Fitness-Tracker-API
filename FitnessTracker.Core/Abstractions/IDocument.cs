namespace FitnessTracker.Core.Abstractions;

public interface IDocument
{
    string Id { get; set; }
    DateTime CreatedAt { get; set; }
}
