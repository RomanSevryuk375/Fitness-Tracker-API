using Amazon.S3;
using FitnessTracker.Business.Abstractions;
using FitnessTracker.Core.Abstractions;
using FitnessTracker.DataAccess.Factories;
using FitnessTracker.DataAccess.Minio;
using FitnessTracker.DataAccess.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FitnessTracker.DataAccess.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var minioSection = configuration.GetSection(MinioOptions.SectionName);
        services.Configure<MinioOptions>(minioSection);

        services.AddSingleton<IAmazonS3>(sp =>
        {
            var minioOptions = sp.GetRequiredService<IOptions<MinioOptions>>().Value;

            var s3Config = new AmazonS3Config
            {
                ServiceURL = minioOptions.ServiceUrl,
                ForcePathStyle = true
            };

            return new AmazonS3Client(minioOptions.AccessKey, minioOptions.SecretKey, s3Config);
        });

        services.AddDbContext<FitnessDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString(nameof(FitnessDbContext)))
                   .UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IFileService, MinioFileService>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IWorkoutRepository, WorkoutRepository>();
        services.AddScoped<IPhotoRepository, PhotoRepository>();

        services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
