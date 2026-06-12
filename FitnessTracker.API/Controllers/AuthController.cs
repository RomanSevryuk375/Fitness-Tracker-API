using FitnessTracker.Business.CQRS.Users.Commands.RegisterUser;
using FitnessTracker.Business.CQRS.Users.Queries.LoginUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Result;

namespace FitnessTracker.API.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(ISender sender) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterUserCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return this.ToActionResult(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginUserQuery query,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(query, cancellationToken);
        if (result.IsSuccess)
        {
            return Ok(new { Token = result.Value });
        }

        return Unauthorized(result.Error);
    }
}