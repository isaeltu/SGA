using MediatR;
using Microsoft.AspNetCore.Mvc;
using SGA.Application.Commands;
using SGA.Application.Queries;

namespace SGA.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("persons")]
    public async Task<ActionResult<int>> CreatePerson([FromBody] CreatePersonCommand command, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(command, cancellationToken);
        return Ok(id);
    }

    [HttpPost("students")]
    public async Task<ActionResult<int>> CreateStudent([FromBody] CreateStudentCommand command, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetStudentById), new { studentId = id }, id);
    }

    [HttpPut("students")]
    public async Task<IActionResult> UpdateStudent([FromBody] UpdateStudentCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpGet("students/{studentId:int}")]
    public async Task<IActionResult> GetStudentById(int studentId, CancellationToken cancellationToken)
    {
        var student = await _mediator.Send(new GetStudentByIdQuery(studentId), cancellationToken);

        if (student is null)
        {
            return NotFound();
        }

        return Ok(student);
    }

    [HttpPost("drivers")]
    public async Task<ActionResult<int>> CreateDriver([FromBody] CreateDriverCommand command, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(command, cancellationToken);
        return Ok(id);
    }

    [HttpPost("employees")]
    public async Task<ActionResult<int>> CreateEmployee([FromBody] CreateEmployeeCommand command, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(command, cancellationToken);
        return Ok(id);
    }

    [HttpPost("administrators")]
    public async Task<ActionResult<int>> CreateAdministrator([FromBody] CreateAdministratorCommand command, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(command, cancellationToken);
        return Ok(id);
    }

    [HttpPost("operators")]
    public async Task<ActionResult<int>> CreateOperator([FromBody] CreateOperatorCommand command, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(command, cancellationToken);
        return Ok(id);
    }
}