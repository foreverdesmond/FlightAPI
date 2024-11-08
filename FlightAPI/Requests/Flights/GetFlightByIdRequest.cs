using System.ComponentModel.DataAnnotations;

namespace FlightAPI.Requests.Flights
{
    public class GetFlightByIdRequest
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Flight ID must be a positive integer.")]
        public required int FlightId { get; set; }
    }
}
