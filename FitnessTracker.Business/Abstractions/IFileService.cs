namespace FitnessTracker.Business.Abstractions;

public interface IFileService
{
    Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, CancellationToken ct);
    Task<Stream> GetFileAsync(string fileName, CancellationToken ct);
    Task DeleteFileAsync(string fileName, CancellationToken ct);
}
