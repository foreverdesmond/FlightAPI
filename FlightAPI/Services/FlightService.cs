using AutoMapper;
using FlightAPI.Data;
using FlightAPI.Models;
using FlightAPI.Data.DTO;
using FlightAPI.Requests.Flights;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace FlightAPI.Services
{
    public class FlightService : IFlightService
    {
        private static readonly NLog.ILogger Logger = LogManager.GetCurrentClassLogger();
        private readonly FlightDbContext _context;
        private readonly IMapper _mapper;

        public FlightService(FlightDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all flights from the database.
        /// </summary>
        /// <returns>A list of all flights.</returns>
        public async Task<IEnumerable<FlightDTO>> GetAllFlightsAsync()
        {
            Logger.Info("Getting all flights");
            var flights = await _context.Flights.ToListAsync();
            return _mapper.Map<IEnumerable<FlightDTO>>(flights);
        }

        /// <summary>
        /// Retrieves a flight by its ID.
        /// </summary>
        /// <param name="request">The request containing the flight ID.</param>
        /// <returns>The flight with the specified ID.</returns>
        public async Task<FlightDTO> GetFlightByIdAsync(GetFlightByIdRequest request)
        {
            Logger.Info($"Getting flight with ID: {request.FlightId}");
            var flight = await _context.Flights.FindAsync(request.FlightId);
            return _mapper.Map<FlightDTO>(flight);
        }

        /// <summary>
        /// Retrieves a flight by its flight number.
        /// </summary>
        /// <param name="request">The request containing the flight number.</param>
        /// <returns>The flight with the specified flight number.</returns>
        public async Task<FlightDTO> GetFlightByNumberAsync(GetFlightByNumberRequest request)
        {
            Logger.Info($"Getting flight with number: {request.FlightNumber}");
            var flight = await _context.Flights
                .FirstOrDefaultAsync(f => f.FlightNumber == request.FlightNumber);
            return _mapper.Map<FlightDTO>(flight);
        }

        /// <summary>
        /// Adds a new flight to the database.
        /// </summary>
        /// <param name="flight">The flight to add.</param>
        /// <returns>The added flight.</returns>
        public async Task<FlightDTO> AddFlightAsync(FlightDTO flight)
        {
            Logger.Info($"Adding a new flight: {flight}");
            var flightEntity = _mapper.Map<Flight>(flight);
            _context.Flights.Add(flightEntity);
            await _context.SaveChangesAsync();
            return _mapper.Map<FlightDTO>(flightEntity);
        }

        /// <summary>
        /// Updates an existing flight in the database.
        /// </summary>
        /// <param name="flight">The flight to update.</param>
        /// <returns>The updated flight.</returns>
        public async Task<FlightDTO> UpdateFlightAsync(FlightDTO flight)
        {
            Logger.Info($"Updating flight with ID: {flight.FlightId}");

            var existingFlight = await _context.Flights.FindAsync(flight.FlightId);
            if (existingFlight == null)
            {
                Logger.Warn($"Flight with ID: {flight.FlightId} not found");
                return null;
            }

            _mapper.Map(flight, existingFlight);

            await _context.SaveChangesAsync();
            return _mapper.Map<FlightDTO>(existingFlight);
        }

        /// <summary>
        /// Deletes a flight by its ID.
        /// </summary>
        /// <param name="request">The request containing the flight ID to delete.</param>
        /// <returns>True if the flight was deleted; otherwise, false.</returns>
        public async Task<bool> DeleteFlightAsync(GetFlightByIdRequest request)
        {
            Logger.Info($"Deleting flight with ID: {request.FlightId}");
            var flight = await _context.Flights.FindAsync(request.FlightId);
            if (flight == null)
            {
                Logger.Warn($"Flight with ID: {request.FlightId} not found");
                return false;
            }

            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Searches for flights based on airline, departure airport, and arrival airport.
        /// </summary>
        /// <param name="request">The request containing search criteria.</param>
        /// <returns>A list of flights matching the search criteria.</returns>
        public async Task<IEnumerable<FlightDTO>> SearchFlightsAsync(SearchFlightsRequest request)
        {
            Logger.Info($"Searching flights with Airline: {request.Airline}, Departure: {request.DepartureAirport}, Arrival: {request.ArrivalAirport}");
            var flights = await _context.Flights
                .Where(f => (string.IsNullOrEmpty(request.Airline) || f.Airline.Contains(request.Airline)) &&
                            (string.IsNullOrEmpty(request.DepartureAirport) || f.DepartureAirport == request.DepartureAirport) &&
                            (string.IsNullOrEmpty(request.ArrivalAirport) || f.ArrivalAirport == request.ArrivalAirport))
                .ToListAsync();
            return _mapper.Map<IEnumerable<FlightDTO>>(flights);
        }
    }
}
