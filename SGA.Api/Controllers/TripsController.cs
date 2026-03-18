using MediatR;
using Microsoft.AspNetCore.Mvc;
using SGA.Application.Commands;
using SGA.Application.Queries;

namespace SGA.Api.Controllers;

[ApiController]
[Route("api/trips")]
public class TripsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TripsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateTripCommand command, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { tripId = id }, id);
    }

    [HttpGet("{tripId:int}")]
    public async Task<IActionResult> GetById(int tripId, CancellationToken cancellationToken)
    {
        var trip = await _mediator.Send(new GetTripByIdQuery(tripId), cancellationToken);

        if (trip is null)
        {
            return NotFound();
        }

        return Ok(trip);
    }

    [HttpPost("start")]
    public async Task<IActionResult> Start([FromBody] StartTripCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPost("complete")]
    public async Task<IActionResult> Complete([FromBody] CompleteTripCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPost("cancel")]
    public async Task<IActionResult> Cancel([FromBody] CancelTripCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}