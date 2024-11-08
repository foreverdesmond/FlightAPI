using FlightAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FlightAPI.IntegrationTests.Utilities
{
    public static class TestDatabaseInitializer
    {
        public static void InitializeInMemoryDatabase(IServiceCollection services)
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<FlightDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<FlightDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });
        }
    }
}