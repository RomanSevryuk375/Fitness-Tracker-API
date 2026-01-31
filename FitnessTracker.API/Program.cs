using FitnessTracker.API.Middlewares;
using FitnessTracker.Business.Abstractions;
using FitnessTracker.Business.MapProfiles;
using FitnessTracker.Business.Secure;
using FitnessTracker.Business.Services;
using FitnessTracker.Business.Validators;
using FitnessTracker.Core.Abstractions;
using FitnessTracker.DataAccess;
using FitnessTracker.DataAccess.Extentions;
using FitnessTracker.DataAccess.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "FitnessTracker API", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(WorkoutProfile).Assembly));

builder.Services.AddDbContext<SystemDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(SystemDbContext)))
           .UseSnakeCaseNamingConvention();
});

var jwtOptions = builder.Configuration.GetSection("JwtOptions").Get<JwtOptions>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false, 
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions!.SecretKey))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddValidatorsFromAssemblyContaining<CreateWorkoutRequestValidator>();

builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));
builder.Services.AddScoped<IMyPasswordHasher, MyPasswordHasher>();
builder.Services.AddScoped<IFileService, MinioFileService>();

builder.Services.AddScoped<IWorkoutService, WorkoutService>();
builder.Services.AddScoped<IWorkoutRepository, WorkoutRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SystemDbContext>();
    dbContext.Database.Migrate();
}

app.UseCustomException();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
