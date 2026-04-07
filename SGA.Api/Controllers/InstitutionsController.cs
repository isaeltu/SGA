using MediatR;
using Microsoft.AspNetCore.Mvc;
using SGA.Application.Commands;
using SGA.Application.Queries;

namespace SGA.Api.Controllers;

[ApiController]
[Route("api/institutions")]
public class InstitutionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public InstitutionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateInstitutionCommand command, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { institutionId = id }, id);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateInstitutionCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpGet("{institutionId:int}")]
    public async Task<IActionResult> GetById(int institutionId, CancellationToken cancellationToken)
    {
        var institution = await _mediator.Send(new GetInstitutionByIdQuery(institutionId), cancellationToken);

        if (institution is null)
        {
            return NotFound();
        }

        return Ok(institution);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var institutions = await _mediator.Send(new GetInstitutionsQuery(), cancellationToken);
        return Ok(institutions);
    }
}