using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Shared.Result;

public static class ResultExtensions
{
    public static ActionResult ToActionResult<T>(
        this ControllerBase controller, Result<T> result)
    {
        if (result.IsSuccess)
        {
            return controller.Ok(result.Value);
        }

        return MapError(controller, result.Error);
    }

    public static ActionResult ToActionResult(
        this ControllerBase controller, Result result)
    {
        if (result.IsSuccess)
        {
            return controller.NoContent();
        }

        return MapError(controller, result.Error);
    }

    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var sanitizedPage = pageNumber < 1 ? 1 : pageNumber;
        var sanitizedSize = pageSize < 1 ? 10 : pageSize;

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((sanitizedPage - 1) * sanitizedSize)
            .Take(sanitizedSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<T>(items, totalCount, sanitizedPage, sanitizedSize);
    }

    private static ObjectResult MapError(
        ControllerBase controller, Error error)
    {
        var response = new
        {
            error = error.Code,
            message = error.Message
        };

        return error.Type switch
        {
            ErrorType.NotFound => controller.NotFound(response),
            ErrorType.Validation => controller.BadRequest(response),
            ErrorType.Conflict => controller.Conflict(response),

            _ => controller.StatusCode(500, new { error = "InternalError", message = error.Message })
        };
    }
}
