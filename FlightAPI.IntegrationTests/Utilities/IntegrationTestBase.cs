using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using FlightAPI.IntegrationTests.Utilities;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace FlightAPI.IntegrationTests
{
    public class IntegrationTestBase : IClassFixture<WebApplicationFactory<Program>>
    {
        protected readonly HttpClient _client;

        public IntegrationTestBase(WebApplicationFactory<Program> factory)
        {
            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.Test.json");
                });

                builder.ConfigureServices(services =>
                {
                    TestDatabaseInitializer.InitializeInMemoryDatabase(services);
                });
            }).CreateClient();
        }
    }
}