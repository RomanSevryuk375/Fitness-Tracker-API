using AutoMapper;
using FitnessTracker.Business.Abstractions;
using FitnessTracker.Business.DTOs;
using FitnessTracker.Business.Services;
using FitnessTracker.Core.Abstractions;
using FitnessTracker.Core.AggregateRoots.Workouts;
using FitnessTracker.Core.Enums;
using FitnessTracker.Core.Models;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace FitnessTracker.Tests;

public class WorkoutServiceTests
{
    private readonly Mock<IWorkoutRepository> _repoMock;
    private readonly Mock<IFileService> _fileMock;
    private readonly Mock<IValidator<CreateWorkoutRequest>> _validatorMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly WorkoutService _service;

    public WorkoutServiceTests()
    {
        _repoMock = new Mock<IWorkoutRepository>();
        _fileMock = new Mock<IFileService>();
        _validatorMock = new Mock<IValidator<CreateWorkoutRequest>>();
        _mapperMock = new Mock<IMapper>();

        _service = new WorkoutService(
            _repoMock.Object, 
            _mapperMock.Object, 
            _fileMock.Object, 
            _validatorMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldCleanupS3_WhenDatabaseFails()
    {
        // Arrange
        var userId = "user-123";
        var request = new CreateWorkoutRequest
        {
            Title = "Heavy Leg Day",
            TypeId = (int)WorkoutType.Strength,
            DurationMinutes = 60,
            CaloriesBurned = 500,
            WorkoutDate = DateTime.UtcNow,
            Exercises = []
        };

        var photoModels = new List<FileModel> { new(new MemoryStream(), "test.jpg", "image/jpeg") };
        var s3Path = "s3://bucket/test.jpg";

        _validatorMock.Setup(v => v.ValidateAsync(
            request, 
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _fileMock.Setup(f => f.UploadFileAsync(
            It.IsAny<Stream>(), 
            It.IsAny<string>(), 
            It.IsAny<string>(), 
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(s3Path);

        _repoMock.Setup(r => r.AddAsync(
            It.IsAny<Workout>(),
            It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB fail"));

        var act = () => _service.CreateAsync(userId, request, photoModels, CancellationToken.None);

        await act.Should().ThrowAsync<Exception>();

        _fileMock.Verify(f => f.DeleteFileAsync(
            s3Path, 
            It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenUserRequestsSomeoneElsesWorkout()
    {
        var workoutId = "workout-1";
        var realOwnerId = "owner-id";
        var hackerId = "hacker-id";

        var workout = Workout.Create(
            workoutId,
            realOwnerId, 
            "Secret Workout",
            WorkoutType.HIIT, 
            TimeSpan.FromMinutes(30),
            300,
            DateTime.UtcNow, 
            DateTime.UtcNow).workout;

        _repoMock.Setup(r => r.GetByIdAsync(
            workoutId, 
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(workout);

        var result = await _service.GetByIdAsync(hackerId, workoutId, CancellationToken.None);

        result.Should().BeNull(); 
    }

    [Fact]
    public async Task CreateAsync_ShouldNotUploadToS3_WhenValidationFails()
    {
        var request = new CreateWorkoutRequest
        {
            Title = "Heavy Leg Day",
            TypeId = (int)WorkoutType.Strength,
            DurationMinutes = 60,
            CaloriesBurned = 500,
            WorkoutDate = DateTime.UtcNow,
            Exercises = []
        };

        var failures = new List<ValidationFailure> { new("Title", "Title is empty") };

        _validatorMock.Setup(v => v.ValidateAsync(
            request, 
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(failures));

        var act = () => _service.CreateAsync("user-1", request, [], CancellationToken.None);

        await act.Should().ThrowAsync<Core.Exceptions.ValidationException>();

        _fileMock.Verify(x => x.UploadFileAsync(
            It.IsAny<Stream>(),
            It.IsAny<string>(), 
            It.IsAny<string>(), 
            It.IsAny<CancellationToken>()), 
            Times.Never);
    }
}