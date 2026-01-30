using Amazon.S3;
using Amazon.S3.Transfer;
using FitnessTracker.Business.Abstractions;
using Microsoft.Extensions.Configuration;

namespace FitnessTracker.Business.Secure;

public class MinioFileService : IFileService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    public MinioFileService(IConfiguration configuration)
    {
        var config = configuration.GetSection("Minio");
        _bucketName = config["BucketName"]!;

        var s3Config = new AmazonS3Config
        {
            ServiceURL = config["ServiceUrl"],
            ForcePathStyle = true
        };

        _s3Client = new AmazonS3Client(
            config["AccessKey"],
            config["SecretKey"],
            s3Config);
    }

    public async Task<Stream> GetFileAsync(string fileName, CancellationToken ct)
    {
        try
        {
            var response = await _s3Client.GetObjectAsync(_bucketName, fileName, ct);

            return response.ResponseStream;
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            throw new FileNotFoundException($"File {fileName} not found");
        }
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, CancellationToken ct)
    {
        var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";

        try
        {
            var transferUtility = new TransferUtility(_s3Client);

            await transferUtility.UploadAsync(new TransferUtilityUploadRequest
            {
                InputStream = fileStream,
                Key = uniqueFileName,
                BucketName = _bucketName,
                ContentType = contentType,
                AutoCloseStream = false,
            }, ct);

            return uniqueFileName;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task DeleteFileAsync(string fileName, CancellationToken ct)
    {
        await _s3Client.DeleteObjectAsync(_bucketName, fileName, ct);
    }
}

