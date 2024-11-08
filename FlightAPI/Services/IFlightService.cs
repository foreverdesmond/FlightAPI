using FlightAPI.Data.DTO;
using FlightAPI.Requests.Flights;

namespace FlightAPI.Services
{
    public interface IFlightService
    {
        /// <summary>
        /// Retrieves all flights from the database.
        /// </summary>
        /// <returns>A list of all flights.</returns>
        Task<IEnumerable<FlightDTO>> GetAllFlightsAsync();

        /// <summary>
        /// Retrieves a flight by its ID.
        /// </summary>
        /// <param name="request">The request containing the flight ID.</param>
        /// <returns>The flight with the specified ID.</returns>
        Task<FlightDTO> GetFlightByIdAsync(GetFlightByIdRequest request);

        /// <summary>
        /// Retrieves a flight by its flight number.
        /// </summary>
        /// <param name="request">The request containing the flight number.</param>
        /// <returns>The flight with the specified flight number.</returns>
        Task<FlightDTO> GetFlightByNumberAsync(GetFlightByNumberRequest request);

        /// <summary>
        /// Adds a new flight to the database.
        /// </summary>
        /// <param name="flight">The flight to add.</param>
        /// <returns>The added flight.</returns>
        Task<FlightDTO> AddFlightAsync(FlightDTO flight);

        /// <summary>
        /// Updates an existing flight in the database.
        /// </summary>
        /// <param name="flight">The flight to update.</param>
        /// <returns>The updated flight.</returns>
        Task<FlightDTO> UpdateFlightAsync(FlightDTO flight);

        /// <summary>
        /// Deletes a flight by its ID.
        /// </summary>
        /// <param name="request">The request containing the flight ID to delete.</param>
        /// <returns>True if the flight was deleted; otherwise, false.</returns>
        Task<bool> DeleteFlightAsync(GetFlightByIdRequest request);

        /// <summary>
        /// Searches for flights based on airline, departure airport, and arrival airport.
        /// </summary>
        /// <param name="request">The request containing search criteria.</param>
        /// <returns>A list of flights matching the search criteria.</returns>
        Task<IEnumerable<FlightDTO>> SearchFlightsAsync(SearchFlightsRequest request);
    }
}
