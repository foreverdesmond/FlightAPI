using AutoMapper;
using FlightAPI.Data;
using FlightAPI.Data.DTO;
using FlightAPI.Models;
using FlightAPI.Requests.Flights;
using FlightAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace FlightAPI.Tests.Services
{
    public class FlightServiceTests
    {
        private readonly FlightDbContext _context;
        private readonly IMapper _mapper;
        private readonly FlightService _flightService;

        public FlightServiceTests()
        {
            var options = new DbContextOptionsBuilder<FlightDbContext>()
                .UseInMemoryDatabase(databaseName: "FlightDb")
                .Options;

            _context = new FlightDbContext(options);

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Flight, FlightDTO>().ReverseMap();
            });

            _mapper = config.CreateMapper();
            _flightService = new FlightService(_context, _mapper);
        }

        [Fact]
        public async Task GetAllFlightsAsync_ReturnsAllFlights()
        {
            // Arrange
            var flight = new Flight { FlightId = 1, FlightNumber = "AB123", Airline = "Test Airline", DepartureAirport = "ABC", ArrivalAirport = "XYZ", Status = FlightStatus.Scheduled };
            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();

            // Act
            var result = await _flightService.GetAllFlightsAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal("AB123", result.First().FlightNumber);
        }

        [Fact]
        public async Task GetFlightByIdAsync_ReturnsFlight_WhenFlightExists()
        {
            // Arrange
            var flight = new Flight { FlightId = 2, FlightNumber = "CD456", Airline = "Another Airline", DepartureAirport = "DEF", ArrivalAirport = "UVW", Status = FlightStatus.Scheduled };
            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();

            // Act
            var result = await _flightService.GetFlightByIdAsync(new GetFlightByIdRequest { FlightId = 2 });

            // Assert
            Assert.NotNull(result);
            Assert.Equal("CD456", result.FlightNumber);
        }

        [Fact]
        public async Task GetFlightByIdAsync_ReturnsNull_WhenFlightDoesNotExist()
        {
            // Act
            var result = await _flightService.GetFlightByIdAsync(new GetFlightByIdRequest { FlightId = 999 });

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddFlightAsync_AddsFlightSuccessfully()
        {
            // Arrange
            var flightDto = new FlightDTO { FlightId=3, FlightNumber = "EF789", Airline = "New Airline", DepartureAirport = "GHI", ArrivalAirport = "RST", Status = FlightStatus.Scheduled.ToString() };

            // Act
            var result = await _flightService.AddFlightAsync(flightDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("EF789", result.FlightNumber);
            Assert.Equal(1, _context.Flights.Count());
        }

        [Fact]
        public async Task UpdateFlightAsync_UpdatesFlightSuccessfully()
        {
            // Arrange
            var flight = new Flight { FlightId = 4, FlightNumber = "GH012", Airline = "Update Airline", DepartureAirport = "JKL", ArrivalAirport = "NOP", Status = FlightStatus.Scheduled };
            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();

            var existingFlight = await _context.Flights.FindAsync(4);
            existingFlight.Airline = "Updated Airline";
            existingFlight.Status = FlightStatus.Delayed;

            var updatedFlightDto = _mapper.Map<FlightDTO>(existingFlight);

            // Act
            var result = await _flightService.UpdateFlightAsync(updatedFlightDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Airline", result.Airline);
            Assert.Equal(FlightStatus.Delayed.ToString(), result.Status);
        }

        [Fact]
        public async Task DeleteFlightAsync_DeletesFlightSuccessfully()
        {
            // Arrange
            var flight = new Flight { FlightId = 5, FlightNumber = "IJ345", Airline = "Delete Airline", DepartureAirport = "QRS", ArrivalAirport = "TUV", Status = FlightStatus.Scheduled };
            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();

            // Act
            var result = await _flightService.DeleteFlightAsync(new GetFlightByIdRequest { FlightId = 5 });

            // Assert
            Assert.True(result);
            Assert.Empty(_context.Flights);
        }

        [Fact]
        public async Task SearchFlightsAsync_ReturnsMatchingFlights()
        {
            // Arrange
            var flight1 = new Flight { FlightId = 6, FlightNumber = "KL678", Airline = "Search Airline", DepartureAirport = "WXY", ArrivalAirport = "ZAB", Status = FlightStatus.Scheduled };
            var flight2 = new Flight { FlightId = 7, FlightNumber = "MN901", Airline = "Search Airline", DepartureAirport = "WXY", ArrivalAirport = "ZAB", Status = FlightStatus.Scheduled };
            _context.Flights.AddRange(flight1, flight2);
            await _context.SaveChangesAsync();

            var searchRequest = new SearchFlightsRequest { Airline = "Search Airline", DepartureAirport = "WXY", ArrivalAirport = "ZAB" };

            // Act
            var result = await _flightService.SearchFlightsAsync(searchRequest);

            // Assert
            Assert.Equal(2, result.Count());
        }
    }
}
