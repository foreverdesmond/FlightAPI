using System.ComponentModel.DataAnnotations;

namespace FlightAPI.Requests.Flights
{
    public class SearchFlightsRequest
    {
        [Required]
        [StringLength(50, ErrorMessage = "Airline name must be no more than 50 characters.")]
        public required string Airline { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Departure airport code must be exactly 3 characters.")]
        public required string DepartureAirport { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Arrival airport code must be exactly 3 characters.")]
        public required string ArrivalAirport { get; set; }
    }
}