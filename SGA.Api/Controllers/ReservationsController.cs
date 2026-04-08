using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SGA.Application.Commands;
using SGA.Application.Queries;

namespace SGA.Api.Controllers;

[ApiController]
[Route("api/reservations")]
public class ReservationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReservationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateReservationCommand command, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { reservationId = id }, id);
    }

    [HttpPost("guest")]
    public async Task<ActionResult<int>> CreateGuest([FromBody] CreateGuestReservationCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var id = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { reservationId = id }, id);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (DbUpdateException)
        {
            return BadRequest("No se pudo crear la reserva con los datos recibidos. Intenta nuevamente.");
        }
    }

    [HttpGet("{reservationId:int}")]
    public async Task<IActionResult> GetById(int reservationId, CancellationToken cancellationToken)
    {
        var reservation = await _mediator.Send(new GetReservationByIdQuery(reservationId), cancellationToken);

        if (reservation is null)
        {
            return NotFound();
        }

        return Ok(reservation);
    }

    [HttpPost("board")]
    public async Task<IActionResult> Board([FromBody] BoardReservationCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPost("cancel")]
    public async Task<IActionResult> Cancel([FromBody] CancelReservationCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}