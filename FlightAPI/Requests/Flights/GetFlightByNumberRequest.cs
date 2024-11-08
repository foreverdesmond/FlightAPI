using System.ComponentModel.DataAnnotations;

namespace FlightAPI.Requests.Flights
{
    public class GetFlightByNumberRequest
    {
        [Required]
        [StringLength(10, ErrorMessage = "Flight number must be no more than 10 characters.")]
        public required string FlightNumber { get; set; }
    }
}