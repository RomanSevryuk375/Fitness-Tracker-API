namespace FitnessTracker.Core.Abstractions;

public interface IDocument
{
    Guid Id { get; }
    DateTime CreatedAt { get; }
}
