using FitnessTracker.Business.CQRS.Workouts.Commands.AddExerciseToWorkout;
using FitnessTracker.Business.CQRS.Workouts.Commands.AddSetToExercise;
using FitnessTracker.Business.CQRS.Workouts.Commands.AttachPhotoToWorkout;
using FitnessTracker.Business.CQRS.Workouts.Commands.CreateWorkout;
using FitnessTracker.Business.CQRS.Workouts.Commands.DeleteWorkout;
using FitnessTracker.Business.CQRS.Workouts.Commands.RemoveExerciseFromWorkout;
using FitnessTracker.Business.CQRS.Workouts.Commands.RemovePhotoFromWorkout;
using FitnessTracker.Business.CQRS.Workouts.Commands.RemoveSetFromExercise;
using FitnessTracker.Business.CQRS.Workouts.Commands.RenameExercise;
using FitnessTracker.Business.CQRS.Workouts.Commands.UpdateSet;
using FitnessTracker.Business.CQRS.Workouts.Commands.UpdateWorkout;
using FitnessTracker.Business.CQRS.Workouts.Queries.GetExerciseHistory;
using FitnessTracker.Business.CQRS.Workouts.Queries.GetWorkoutById;
using FitnessTracker.Business.CQRS.Workouts.Queries.GetWorkoutList;
using FitnessTracker.Business.CQRS.Workouts.Queries.GetWorkoutPhoto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Result;
using System.Security.Claims;

namespace FitnessTracker.API.Controllers;

[Authorize]
[ApiController]
[Route("api/workouts")]
public sealed class WorkoutsController(ISender sender) : ControllerBase
{
    private Guid CurrentUserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);


    [HttpGet]
    public async Task<IActionResult> GetList(
        [FromQuery] GetWorkoutListQuery query,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            query with { UserId = CurrentUserId }, cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetWorkoutByIdQuery
        {
            UserId = CurrentUserId,
            WorkoutId = id
        }, cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateWorkoutCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            command with { UserId = CurrentUserId }, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value)
            : this.ToActionResult(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateWorkoutCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            command with { UserId = CurrentUserId, WorkoutId = id }, cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteWorkoutCommand
        {
            UserId = CurrentUserId,
            WorkoutId = id
        }, cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpPost("{id:guid}/exercises")]
    public async Task<IActionResult> AddExercise(
        Guid id,
        [FromBody] AddExerciseToWorkoutCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            command with { UserId = CurrentUserId, WorkoutId = id }, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = id }, result.Value)
            : this.ToActionResult(result);
    }

    [HttpPatch("{id:guid}/exercises/{exerciseId:guid}/rename")]
    public async Task<IActionResult> RenameExercise(
        Guid id,
        Guid exerciseId,
        [FromBody] string newName,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new RenameExerciseCommand
        {
            UserId = CurrentUserId,
            WorkoutId = id,
            ExerciseId = exerciseId,
            NewName = newName
        }, cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpDelete("{id:guid}/exercises/{exerciseId:guid}")]
    public async Task<IActionResult> RemoveExercise(
        Guid id,
        Guid exerciseId,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new RemoveExerciseFromWorkoutCommand
        {
            UserId = CurrentUserId,
            WorkoutId = id,
            ExerciseId = exerciseId
        }, cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetHistory(
        [FromQuery] GetExerciseHistoryQuery query,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            query with { UserId = CurrentUserId }, cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpPost("{id:guid}/exercises/{exerciseId:guid}/sets")]
    public async Task<IActionResult> AddSet(
        Guid id,
        Guid exerciseId,
        [FromBody] AddSetToExerciseCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            command with { UserId = CurrentUserId, WorkoutId = id, ExerciseId = exerciseId }, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = id }, result.Value)
            : this.ToActionResult(result);
    }

    [HttpPut("{id:guid}/exercises/{exerciseId:guid}/sets/{setId:guid}")]
    public async Task<IActionResult> UpdateSet(
        Guid id,
        Guid exerciseId,
        Guid setId,
        [FromBody] UpdateSetCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            command with
            {
                UserId = CurrentUserId,
                WorkoutId = id,
                ExerciseId = exerciseId,
                SetId = setId
            }, cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpDelete("{id:guid}/exercises/{exerciseId:guid}/sets/{setId:guid}")]
    public async Task<IActionResult> RemoveSet(
        Guid id,
        Guid exerciseId,
        Guid setId,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new RemoveSetFromExerciseCommand
        {
            UserId = CurrentUserId,
            WorkoutId = id,
            ExerciseId = exerciseId,
            SetId = setId
        }, cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpPost("{id:guid}/photos")]
    public async Task<IActionResult> UploadPhoto(
        Guid id,
        IFormFile file,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new AttachPhotoToWorkoutCommand
        {
            UserId = CurrentUserId,
            WorkoutId = id,
            Content = file.OpenReadStream(),
            FileName = file.FileName,
            ContentType = file.ContentType
        }, cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpGet("{id:guid}/photos/{fileName}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPhoto(
        Guid id,
        string fileName,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetWorkoutPhotoQuery
        {
            UserId = CurrentUserId,
            WorkoutId = id,
            FileName = fileName
        }, cancellationToken);

        if (result.IsSuccess)
        {
            var response = result.Value;
            return File(response.Stream, response.ContentType, response.FileName);
        }

        return this.ToActionResult(result);
    }

    [HttpDelete("{id:guid}/photos/{photoId:guid}")]
    public async Task<IActionResult> RemovePhoto(
        Guid id,
        Guid photoId,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new RemovePhotoFromWorkoutCommand
        {
            UserId = CurrentUserId,
            WorkoutId = id,
            PhotoId = photoId
        }, cancellationToken);

        return this.ToActionResult(result);
    }
}