using Amazon.S3;
using Amazon.S3.Transfer;
using FitnessTracker.Business.Abstractions;
using Microsoft.Extensions.Options;
using Shared.Result;

namespace FitnessTracker.DataAccess.Minio;

public sealed class MinioFileService(IAmazonS3 s3Client, IOptions<MinioOptions> options) : IFileService
{
    private readonly string _bucketName = options.Value.BucketName;

    public async Task<Result<Stream>> GetFileAsync(string fileName, CancellationToken cancellationToken)
    {
        try
        {
            var response = await s3Client.GetObjectAsync(_bucketName, fileName, cancellationToken);
            return Result<Stream>.Success(response.ResponseStream);
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return Result<Stream>.Failure(Error.NotFound(
                "File.NotFound", $"File {fileName} not found."));
        }
        catch (Exception ex)
        {
            return Result<Stream>.Failure(Error.Failure(
                "Minio.Error", ex.Message));
        }
    }

    public async Task<Result<string>> UploadFileAsync(
        Stream fileStream, 
        string fileName, 
        string contentType, 
        CancellationToken cancellationToken)
    {
        try
        {
            using var transferUtility = new TransferUtility(s3Client);

            var request = new TransferUtilityUploadRequest
            {
                InputStream = fileStream,
                Key = fileName,
                BucketName = _bucketName,
                ContentType = contentType,
                AutoCloseStream = false
            };

            await transferUtility.UploadAsync(request, cancellationToken);

            return Result<string>.Success(fileName);
        }
        catch (Exception ex)
        {
            return Result<string>.Failure(Error.Failure(
                "Minio.UploadFailed", ex.Message));
        }
    }

    public async Task<Result> DeleteFileAsync(string fileName, CancellationToken cancellationToken)
    {
        try
        {
            await s3Client.DeleteObjectAsync(_bucketName, fileName, cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(Error.Failure(
                "Minio.DeleteFailed", ex.Message));
        }
    }
}