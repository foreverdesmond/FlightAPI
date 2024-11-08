using FlightAPI.Controllers;
using FlightAPI.Data.DTO;
using FlightAPI.Services;
using FlightAPI.Requests.Flights;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Moq;

namespace FlightAPI.Tests
{
    public class FlightControllerTests
    {
        private readonly Mock<IFlightService> _flightServiceMock;
        private readonly FlightsController sut;

        public FlightControllerTests()
        {
            _flightServiceMock = new Mock<IFlightService>();
            sut = new FlightsController(_flightServiceMock.Object);
        }

        [Fact]
        public async Task GetAllFlights_ReturnsOkResult()
        {
            // Arrange
            var existingFlight = new FlightDTO
            {
                FlightId = 1,
                FlightNumber = "AB123",
                Airline = "Test Airline",
                DepartureAirport = "ABC",
                ArrivalAirport = "XYZ",
                Status = "Scheduled"
            };

            // Act
            _flightServiceMock.Setup(service => service.GetAllFlightsAsync())
                .ReturnsAsync(new List<FlightDTO> { existingFlight });

            var expectedResult = await sut.GetAllFlights();

            // Assert
            Assert.IsType<OkObjectResult>(expectedResult.Result);
        }

        [Fact]
        public async Task GetAllFlights_ReturnsNotFound_WhenNoFlightsExist()
        {
            // Arrange
            _flightServiceMock.Setup(service => service.GetAllFlightsAsync())
                .ReturnsAsync(new List<FlightDTO>());

            // Act
            var expectedResult = await sut.GetAllFlights();

            // Assert
            Assert.IsType<NotFoundResult>(expectedResult.Result);
        }

        [Fact]
        public async Task GetFlight_ReturnsOkResult_WhenFlightExists()
        {
            // Arrange
            var existingFlight = new FlightDTO
            {
                FlightId = 1,
                FlightNumber = "AB123",
                Airline = "Test Airline",
                DepartureAirport = "ABC",
                ArrivalAirport = "XYZ",
                Status = "Scheduled"
            };

            _flightServiceMock.Setup(service => service.GetFlightByIdAsync(It.IsAny<GetFlightByIdRequest>()))
                .ReturnsAsync(existingFlight);

            // Act
            var expectedResult = await sut.GetFlight(1);

            // Assert
            Assert.IsType<OkObjectResult>(expectedResult.Result);
        }

        [Fact]
        public async Task GetFlight_ReturnsNotFound_WhenFlightDoesNotExist()
        {
            // Arrange
            _flightServiceMock.Setup(service => service.GetFlightByIdAsync(It.IsAny<GetFlightByIdRequest>()))
                .ReturnsAsync((FlightDTO)null);

            // Act
            var expectedResult = await sut.GetFlight(1);

            // Assert
            Assert.IsType<NotFoundResult>(expectedResult.Result);
        }

        [Fact]
        public async Task PostFlight_ReturnsCreatedAtAction()
        {
            // Arrange
            var newFlight = new FlightDTO
            {
                FlightId = 1,
                FlightNumber = "AB123",
                Airline = "Test Airline",
                DepartureAirport = "ABC",
                ArrivalAirport = "XYZ",
                Status = "Scheduled"
            };

            _flightServiceMock.Setup(service => service.AddFlightAsync(It.IsAny<FlightDTO>()))
                .ReturnsAsync(newFlight);

            // Act
            var expectedResult = await sut.PostFlight(newFlight);

            // Assert
            Assert.IsType<CreatedAtActionResult>(expectedResult.Result);
        }

        [Fact]
        public async Task PostFlight_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var invalidFlight = new FlightDTO
            {
                FlightId = 0,
                FlightNumber = "AB000",
                Airline = "No Airline",
                DepartureAirport = "AAA",
                ArrivalAirport = "ZZZ",
                Status = "Cancelled"
            };

            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            sut.ModelState.AddModelError("FlightNumber", "Invalid request parameters.");

            // Act
            var expectedResult = await sut.PostFlight(invalidFlight);

            // Assert
            Assert.IsType<BadRequestObjectResult>(expectedResult.Result);
        }

        [Fact]
        public async Task PostFlight_ReturnsBadRequest_WhenAddFlightFails()
        {
            // Arrange
            var flight = new FlightDTO
            {
                FlightId = 1,
                FlightNumber = "AB123",
                Airline = "Test Airline",
                DepartureAirport = "ABC",
                ArrivalAirport = "XYZ",
                Status = "Scheduled"
            };

            _flightServiceMock.Setup(service => service.AddFlightAsync(It.IsAny<FlightDTO>()))
                .ReturnsAsync((FlightDTO)null);

            // Act
            var expectedResult = await sut.PostFlight(flight);

            // Assert
            Assert.IsType<BadRequestObjectResult>(expectedResult.Result);
        }

        [Fact]
        public async Task PutFlight_ReturnsBadRequest_WhenIdMismatch()
        {
            // Arrange
            var flight = new FlightDTO
            {
                FlightId = 1,
                FlightNumber = "AB123",
                Airline = "Test Airline",
                DepartureAirport = "ABC",
                ArrivalAirport = "XYZ",
                Status = "Scheduled"
            };

            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var expectedResult = await sut.PutFlight(2, flight); // ID mismatch

            // Assert
            Assert.IsType<BadRequestObjectResult>(expectedResult);
        }

        [Fact]
        public async Task PutFlight_ReturnsNoContent_WhenUpdateIsSuccessful()
        {
            // Arrange
            var flight = new FlightDTO
            {
                FlightId = 1,
                FlightNumber = "AB123",
                Airline = "Test Airline",
                DepartureAirport = "ABC",
                ArrivalAirport = "XYZ",
                Status = "Scheduled"
            };

            _flightServiceMock.Setup(service => service.UpdateFlightAsync(It.IsAny<FlightDTO>()))
                .ReturnsAsync(flight);

            // Act
            var expectedResult = await sut.PutFlight(1, flight);

            // Assert
            Assert.IsType<NoContentResult>(expectedResult);
        }

        [Fact]
        public async Task PutFlight_ReturnsNotFound_WhenFlightDoesNotExist()
        {
            // Arrange
            var nonExistentFlight = new FlightDTO
            {
                FlightId = 0,
                FlightNumber = "AB000",
                Airline = "No Airline",
                DepartureAirport = "AAA",
                ArrivalAirport = "ZZZ",
                Status = "Cancelled"
            };

            _flightServiceMock.Setup(service => service.UpdateFlightAsync(It.IsAny<FlightDTO>()))
                .ReturnsAsync((FlightDTO)null);

            // Act
            var expectedResult = await sut.PutFlight(0, nonExistentFlight);

            // Assert
            Assert.IsType<NotFoundResult>(expectedResult);
        }

        [Fact]
        public async Task DeleteFlight_ReturnsNoContent_WhenDeletionIsSuccessful()
        {
            // Arrange
            _flightServiceMock.Setup(service => service.DeleteFlightAsync(It.IsAny<GetFlightByIdRequest>()))
                .ReturnsAsync(true);

            // Act
            var expectedResult = await sut.DeleteFlight(2);

            // Assert
            Assert.IsType<NoContentResult>(expectedResult);
        }

        [Fact]
        public async Task DeleteFlight_ReturnsNotFound_WhenFlightDoesNotExist()
        {
            // Arrange
            _flightServiceMock.Setup(service => service.DeleteFlightAsync(It.IsAny<GetFlightByIdRequest>()))
                .ReturnsAsync(false);

            // Act
            var expectedResult = await sut.DeleteFlight(0);

            // Assert
            Assert.IsType<NotFoundResult>(expectedResult);
        }

        [Fact]
        public async Task SearchFlights_ReturnsOkResult_WithMatchingFlights()
        {
            // Arrange
            var flights = new List<FlightDTO>
            {
                new FlightDTO
                {
                    FlightId = 1,
                    FlightNumber = "AB123",
                    Airline = "Test Airline",
                    DepartureAirport = "ABC",
                    ArrivalAirport = "XYZ",
                    Status = "Scheduled"
                }
            };

            _flightServiceMock.Setup(service => service.SearchFlightsAsync(It.IsAny<SearchFlightsRequest>()))
                .ReturnsAsync(flights);

            // Act
            var expectedResult = await sut.SearchFlights("Test Airline", "ABC", "XYZ");

            // Assert
            Assert.IsType<OkObjectResult>(expectedResult.Result);
        }

        [Fact]
        public async Task SearchFlights_ReturnsOkResult_WhenNoFlightsMatch()
        {
            // Arrange
            _flightServiceMock.Setup(service => service.SearchFlightsAsync(It.IsAny<SearchFlightsRequest>()))
                .ReturnsAsync(new List<FlightDTO>());

            // Act
            var expectedResult = await sut.SearchFlights("Nonexistent Airline", "AAA", "ZZZ");

            // Assert
            Assert.IsType<OkObjectResult>(expectedResult.Result);
        }
    }
}
