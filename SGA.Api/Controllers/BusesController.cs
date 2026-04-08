using MediatR;
using Microsoft.AspNetCore.Mvc;
using SGA.Application.Commands;
using SGA.Application.Queries;

namespace SGA.Api.Controllers;

[ApiController]
[Route("api/buses")]
public class BusesController : ControllerBase
{
    private readonly IMediator _mediator;

    public BusesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateBusCommand command, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { busId = id }, id);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var buses = await _mediator.Send(new GetBusesQuery(), cancellationToken);
        return Ok(buses);
    }

    [HttpGet("{busId:int}")]
    public async Task<IActionResult> GetById(int busId, CancellationToken cancellationToken)
    {
        var bus = await _mediator.Send(new GetBusByIdQuery(busId), cancellationToken);
        if (bus is null)
        {
            return NotFound();
        }

        return Ok(bus);
    }

    [HttpDelete("{busId:int}")]
    public async Task<IActionResult> Delete(int busId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteBusCommand(busId), cancellationToken);
        return NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateBusCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}
