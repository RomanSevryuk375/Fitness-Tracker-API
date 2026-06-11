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
    public static IServiceCollection AddInfrastructure(IServiceCollection services, IConfiguration configuration)
    {
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
