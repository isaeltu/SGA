using MediatR;
using Microsoft.AspNetCore.Mvc;
using SGA.Application.Commands;
using SGA.Application.Queries;

namespace SGA.Api.Controllers;

[ApiController]
[Route("api/routes")]
public class RoutesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RoutesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateRouteCommand command, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetAll), new { routeId = id }, id);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? institutionId, CancellationToken cancellationToken)
    {
        var routes = await _mediator.Send(new GetRoutesQuery(institutionId), cancellationToken);
        return Ok(routes);
    }

    [HttpGet("{routeId:int}")]
    public async Task<IActionResult> GetById(int routeId, CancellationToken cancellationToken)
    {
        var route = await _mediator.Send(new GetRouteByIdQuery(routeId), cancellationToken);
        if (route is null)
        {
            return NotFound();
        }

        return Ok(route);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateRouteCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{routeId:int}")]
    public async Task<IActionResult> Delete(int routeId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteRouteCommand(routeId), cancellationToken);
        return NoContent();
    }
}
