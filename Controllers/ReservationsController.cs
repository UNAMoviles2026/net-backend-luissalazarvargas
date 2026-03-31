using Microsoft.AspNetCore.Mvc;
using reservations_api.DTOs.Requests;
using reservations_api.DTOs.Responses;
using reservations_api.Mappers;
using reservations_api.Services;

namespace reservations_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
  private readonly IReservationService _reservationService;

  public ReservationsController(IReservationService reservationService)
  {
    _reservationService = reservationService;
  }

  [HttpPost]
  public async Task<IActionResult> Create([FromBody] CreateReservationRequest request)
  {
    if (!ModelState.IsValid)
    {
      return ValidationProblem(ModelState);
    }

    try
    {
      var createdReservation = await _reservationService.CreateAsync(request);
      return CreatedAtAction(
          nameof(Create),
          createdReservation);
    }
    catch (InvalidOperationException ex)
    {
      if (ex.Message.Contains("StartTime"))
      {
        return BadRequest(new { message = ex.Message });
      }

      if (ex.Message.Contains("Time conflict"))
      {
        return Conflict(new { message = ex.Message });
      }

      throw;
    }
  }

  [HttpGet]
  public async Task<ActionResult<List<ReservationResponse>>> GetByDate([FromQuery] GetReservationByDateRequest request)
  {

    if (!ModelState.IsValid)
    {
      return ValidationProblem(ModelState);
    }
    var reservations = await _reservationService.GetByDateAsync(request.Date);
    return Ok(reservations);
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(Guid id)
  {

    var isDeleted = await _reservationService.DeleteAsync(id);

    if (!isDeleted)
    {
        return NotFound(); 
    }

    return NoContent(); 
  }
}