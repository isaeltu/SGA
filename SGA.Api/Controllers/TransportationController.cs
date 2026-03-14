using MediatR;
using Microsoft.AspNetCore.Mvc;
using SGA.Application.Commands;

namespace SGA.Api.Controllers;

[ApiController]
[Route("api/transportation")]
public class TransportationController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransportationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("modes")]
    public async Task<ActionResult<int>> CreateMode([FromBody] CreateModeCommand command, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(command, cancellationToken);
        return Ok(id);
    }

    [HttpPut("modes")]
    public async Task<IActionResult> UpdateMode([FromBody] UpdateModeCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}