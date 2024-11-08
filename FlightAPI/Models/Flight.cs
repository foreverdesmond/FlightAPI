using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightAPI.Models
{
    /// <summary>
    /// Enumerates the possible statuses of a flight.
    /// </summary>
    public enum FlightStatus
    {
        Scheduled,
        Delayed,
        Cancelled,
        InAir,
        Landed
    }

    /// <summary>
    /// Represents a flight with details such as flight number, airline, 
    /// departure and arrival airports, times, and status.
    /// </summary>
    [Table("flights")] // Maps the class to the 'flights' table
    public class Flight
    {
        /// <summary>
        /// The unique identifier for the flight.
        /// </summary>
        [Key]
        [Column("id")] // Maps the property to the 'id' column
        public int FlightId { get; set; }

        /// <summary>
        /// Gets or sets the flight number.
        /// </summary>
        [Required]
        [StringLength(10)]
        [Column("flight_number")] // Maps the property to the 'flight_number' column
        public string FlightNumber { get; set; }

        /// <summary>
        /// The airline operating the flight.
        /// </summary>
        [Required]
        [StringLength(50)]
        [Column("airline")] // Maps the property to the 'airline' column
        public string Airline { get; set; }

        /// <summary>
        ///The IATA code of the departure airport.
        /// </summary>
        [Required]
        [StringLength(3)]
        [Column("departure_airport")] // Maps the property to the 'departure_airport' column
        public string DepartureAirport { get; set; }

        /// <summary>
        /// The IATA code of the arrival airport.
        /// </summary>
        [Required]
        [StringLength(3)]
        [Column("arrival_airport")] // Maps the property to the 'arrival_airport' column
        public required string ArrivalAirport { get; set; }

        /// <summary>
        /// The scheduled departure time of the flight.
        /// </summary>
        [Required]
        [Column("departure_time")] // Maps the property to the 'departure_time' column
        public DateTime DepartureTime { get; set; }

        /// <summary>
        ///The scheduled arrival time of the flight.
        /// </summary>
        [Required]
        [Column("arrival_time")] // Maps the property to the 'arrival_time' column
        public DateTime ArrivalTime { get; set; }

        /// <summary>
        /// The current status of the flight.
        /// </summary>
        [Required]
        [Column("status")] // Maps the property to the 'status' column
        public FlightStatus Status { get; set; }
    }
}
