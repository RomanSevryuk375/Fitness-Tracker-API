using FitnessTracker.Business.Abstractions;
using FitnessTracker.Business.DTOs;
using FitnessTracker.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.API.Controllers;

[Authorize]
[ApiController]
[Route("api/workouts")]
public class WorkoutsController : ControllerBase
{
    private readonly IWorkoutService _workoutService;

    public WorkoutsController(IWorkoutService workoutService)
    {
        _workoutService = workoutService;
    }
    private string CurrentUserId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value
        ?? throw new UnauthorizedAccessException();

    [HttpPost]
    public async Task<IActionResult> CreateWorkout([FromForm] CreateWorkoutRequest request, List<IFormFile> photos, CancellationToken ct)
    {
        var fileModels = photos.Select(p => new FileModel(
            p.OpenReadStream(),
            p.FileName,
            p.ContentType)).ToList();

        var result = await _workoutService.CreateAsync(CurrentUserId, request, fileModels, ct);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet]
    public async Task<ActionResult<List<WorkoutDto>>> GetAll([FromQuery] WorkoutFilter filter, CancellationToken ct)
    {
        var workouts = await _workoutService.GetUserWorkoutsAsync(CurrentUserId, filter, ct);
        var totalCount = await _workoutService.GetCountAsync(CurrentUserId, filter, ct);

        Response.Headers.Append("x-total-count", totalCount.ToString());

        return Ok(workouts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WorkoutDto>> GetById(string id, CancellationToken ct)
    {
        var workout = await _workoutService.GetByIdAsync(CurrentUserId, id, ct);
        if (workout == null)
        {
            return NotFound();
        }

        return Ok(workout);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] WorkoutUpdateModel model, CancellationToken ct)
    {
        await _workoutService.UpdateWorkout(CurrentUserId, id, model, ct);
        return NoContent(); 
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken ct)
    {
        await _workoutService.DeleteWorkout(CurrentUserId, id, ct);
        return NoContent(); 
    }

    [HttpGet("{id}/photos/{photoId}")]
    public async Task<IActionResult> GetPhoto(string id, string fileName, CancellationToken ct)
    {
        var (stream, contentType) = await _workoutService.GetPhotoAsync(CurrentUserId, id, fileName, ct);
        return File(stream, contentType, fileName);
    }
}
