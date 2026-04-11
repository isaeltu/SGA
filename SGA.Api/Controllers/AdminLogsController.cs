using Microsoft.AspNetCore.Mvc;
using SGA.Api.Services;

namespace SGA.Api.Controllers;

[ApiController]
[Route("admin/logs")]
public sealed class AdminLogsController : ControllerBase
{
    private readonly IErrorLogWriter errorLogWriter;

    public AdminLogsController(IErrorLogWriter errorLogWriter)
    {
        this.errorLogWriter = errorLogWriter;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync(
        [FromQuery(Name = "fecha_inicio")] DateTimeOffset? startDate,
        [FromQuery(Name = "fecha_fin")] DateTimeOffset? endDate,
        [FromQuery(Name = "usuario_id")] string? user,
        [FromQuery(Name = "tipo_error")] string? errorCode,
        CancellationToken cancellationToken)
    {
        var logs = await errorLogWriter.QueryAsync(startDate, endDate, user, errorCode, cancellationToken).ConfigureAwait(false);
        return Ok(logs);
    }
}
