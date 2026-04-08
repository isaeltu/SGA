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
}
