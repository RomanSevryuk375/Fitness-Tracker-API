using FitnessTracker.Business.Abstractions;
using FitnessTracker.Core.Abstractions;
using FitnessTracker.DataAccess.Minio;
using FitnessTracker.DataAccess.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FitnessTracker.DataAccess.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var minioSection = configuration.GetSection(MinioOptions.SectionName);
        var minioOptions = minioSection.Get<MinioOptions>()
            ?? throw new InvalidOperationException("MinIO configuration is missing.");
        services.Configure<MinioOptions>(minioSection);

        services.AddDbContext<FitnessDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString(nameof(FitnessDbContext)))
                   .UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IFileService, MinioFileService>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IWorkoutRepository, WorkoutRepository>();
        services.AddScoped<IPhotoRepository, PhotoRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
