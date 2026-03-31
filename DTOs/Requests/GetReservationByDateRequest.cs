using System.ComponentModel.DataAnnotations;

namespace reservations_api.DTOs.Requests;

public class GetReservationByDateRequest
{
    [Required]
    public DateOnly Date { get; set; }
}