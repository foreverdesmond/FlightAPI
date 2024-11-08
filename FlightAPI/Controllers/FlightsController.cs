using FlightAPI.Data.DTO;
using FlightAPI.Services;
using FlightAPI.Requests.Flights;
using FlightAPI.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace FlightAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class FlightsController : ControllerBase
    {
        private readonly IFlightService _flightService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlightsController"/> class.
        /// </summary>
        /// <param name="flightService">The flight service used to manage flight operations.</param>
        public FlightsController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        /// <summary>
        /// Retrieves all flights from the database.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/flights
        ///
        /// </remarks>
        /// <returns>A list of flights.</returns>
        [HttpGet]
        [ValidateFlightFilter]
        [ProducesResponseType(typeof(IEnumerable<FlightDTO>), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<FlightDTO>>> GetAllFlights()
        {
            var flights = await _flightService.GetAllFlightsAsync();
            if (flights == null || !flights.Any()) 
            {
                return NotFound();
            }
            return Ok(flights);
        }

        /// <summary>
        /// Retrieves a flight by its ID.
        /// </summary>
        /// <param name="flightId">The ID of the flight to retrieve.</param>
        /// <returns>The flight with the specified ID, or a 404 Not Found response if not found.</returns>
        [HttpGet("{flightId}")]
        [ValidateFlightFilter]
        [ProducesResponseType(typeof(FlightDTO), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FlightDTO>> GetFlight(int flightId)
        {
            var flight = await _flightService.GetFlightByIdAsync(new GetFlightByIdRequest { FlightId = flightId });
            if (flight == null)
                return NotFound();
            return Ok(flight);
        }

        /// <summary>
        /// Adds a new flight to the database.
        /// </summary>
        /// <param name="flight">The flight object to add.</param>
        /// <returns>The created flight object, along with a 201 Created response.</returns>
        [HttpPost]
        [ValidateFlightFilter]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(FlightDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FlightDTO>> PostFlight(FlightDTO flight)
        {
            var newFlight = await _flightService.AddFlightAsync(flight);

            if (newFlight == null)
                return BadRequest("Unable to add flight.");
            
            return CreatedAtAction(nameof(GetFlight), new { flightId = newFlight.FlightId }, newFlight);
        }

        /// <summary>
        /// Updates an existing flight in the database.
        /// </summary>
        /// <param name="flightId">The ID of the flight to update.</param>
        /// <param name="flight">The updated flight object.</param>
        /// <returns>A 204 No Content response if the update is successful, or a 400 Bad Request if the ID does not match.</returns>
        [HttpPut("{flightId}")]
        [ValidateFlightFilter]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutFlight(int flightId, FlightDTO flight)
        {
            if (flightId != flight.FlightId)
            {
                var problemDetails = new ProblemDetails
                {
                    Title = "Invalid Flight ID",
                    Detail = $"The provided flight ID {flight.FlightId} does not match the route ID {flightId}.",
                    Status = StatusCodes.Status400BadRequest,
                    Instance = HttpContext.Request.Path
                };
                return BadRequest(problemDetails);
            }

            var updatedFlight = await _flightService.UpdateFlightAsync(flight);
            if (updatedFlight == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes a flight by its ID.
        /// </summary>
        /// <param name="flightId">The ID of the flight to delete.</param>
        /// <returns>A 204 No Content response if the deletion is successful, or a 404 Not Found response if the flight does not exist.</returns>
        [HttpDelete("{flightId}")]
        [ValidateFlightFilter]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteFlight(int flightId)
        {
            var result = await _flightService.DeleteFlightAsync(new GetFlightByIdRequest { FlightId = flightId });
            if (!result)
                return NotFound(); 
            return NoContent();
        }

        /// <summary>
        /// Searches for flights based on specified criteria.
        /// </summary>
        /// <param name="airline">The airline to filter by.</param>
        /// <param name="departureAirport">The departure airport to filter by.</param>
        /// <param name="arrivalAirport">The arrival airport to filter by.</param>
        /// <returns>A list of flights that match the search criteria.</returns>
        [HttpGet("search")]
        [ValidateFlightFilter]
        [ProducesResponseType(typeof(IEnumerable<FlightDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<FlightDTO>>> SearchFlights(string airline, string departureAirport, string arrivalAirport)
        {
            return Ok(await _flightService.SearchFlightsAsync(new SearchFlightsRequest { Airline = airline, DepartureAirport = departureAirport, ArrivalAirport = arrivalAirport })); 
        }
    }
}
