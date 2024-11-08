namespace FlightAPI.Data.DTO;

public class FlightDTO
{
    public required int FlightId { get; set; }
    public string FlightNumber { get; set; }
    public string Airline { get; set; }
    public string DepartureAirport { get; set; }
    public string ArrivalAirport { get; set; }
    public string Status { get; set; }
}