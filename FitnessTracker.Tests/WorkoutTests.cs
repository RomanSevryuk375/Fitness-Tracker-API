using FitnessTracker.Core.AggregateRoots.Workouts;
using FitnessTracker.Core.Enums;
using FluentAssertions;

namespace FitnessTracker.Tests;

public class WorkoutTests
{
    [Fact]
    public void Create_WithValidData_ShouldReturnSuccess()
    {
        var id = Guid.NewGuid().ToString();
        var userId = Guid.NewGuid().ToString();

        var (workout, errors) = Workout.Create(
            id, 
            userId, 
            "Morning Yoga", 
            WorkoutType.Flexibility,
            TimeSpan.FromMinutes(45),
            200,
            DateTime.UtcNow, 
            DateTime.UtcNow);

        workout.Should().NotBeNull();
        errors.Should().BeEmpty();
        workout!.Title.Should().Be("Morning Yoga");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_WithInvalidTitle_ShouldReturnError(string invalidTitle)
    {

        var (_, errors) = Workout.Create(
            Guid.NewGuid().ToString(), 
            "user1", 
            invalidTitle, 
            WorkoutType.Cardio, 
            TimeSpan.FromMinutes(10), 
            100, 
            DateTime.UtcNow, 
            DateTime.UtcNow);


        errors.Should().Contain("Title can not be empty");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void Create_WithInvalidDuration_ShouldReturnError(int minutes)
    {
        var (_, errors) = Workout.Create(
            Guid.NewGuid().ToString(),
            "user1", 
            "Title", 
            WorkoutType.Cardio, 
            TimeSpan.FromMinutes(minutes), 
            100, 
            DateTime.UtcNow, 
            DateTime.UtcNow);

        errors.Should().Contain("Duration can not be negative");
    }

    [Fact]
    public void Update_WithValidData_ShouldChangeProperties()
    {
        var workout = Workout.Create(
            Guid.NewGuid().ToString(),
            "user1", 
            "Old Title", 
            WorkoutType.Cardio, 
            TimeSpan.FromMinutes(30), 
            100, 
            DateTime.UtcNow, 
            DateTime.UtcNow).workout;

        workout!.Update("New Title", WorkoutType.Strength, TimeSpan.FromMinutes(60), 500, null);

        workout.Title.Should().Be("New Title");
        workout.Type.Should().Be(WorkoutType.Strength);
        workout.Duration.TotalMinutes.Should().Be(60);
    }
}