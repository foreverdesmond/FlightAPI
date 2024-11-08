using Microsoft.EntityFrameworkCore;
using FlightAPI.Models;


namespace FlightAPI.Data
{
    public class FlightDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        public FlightDbContext(DbContextOptions<FlightDbContext> options) : base(options) { }

        /// <summary>
        /// Gets or sets the DbSet for flights.
        /// This property is used to query and save instances of <see cref="Flight"/>.
        /// </summary>
        public DbSet<Flight> Flights { get; set; }
    }
}
