using FitnessTracker.Business.CQRS.Users.Commands.RegisterUser;
using FitnessTracker.Business.CQRS.Users.Queries.LoginUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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

        return result.IsSuccess 
            ? Ok() 
            : BadRequest(result.Error);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginUserQuery query, 
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(query, cancellationToken);

        return result.IsSuccess 
            ? Ok(new { Token = result.Value }) 
            : Unauthorized(result.Error);
    }
}