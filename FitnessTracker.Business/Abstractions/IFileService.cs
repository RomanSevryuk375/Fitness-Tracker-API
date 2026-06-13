using Shared.Result;

namespace FitnessTracker.Business.Abstractions;

public interface IFileService
{
    Task<Result<string>> UploadFileAsync(
        Stream fileStream,
        string fileName,
        string contentType,
        CancellationToken cancellationToken);
    Task<Result<Stream>> GetFileAsync(string fileName, CancellationToken cancellationToken);
    Task<Result> DeleteFileAsync(string fileName, CancellationToken cancellationToken);
}
