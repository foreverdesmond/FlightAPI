using FlightAPI.Data;
using FlightAPI.Services;
using FlightAPI.Filters;
using FlightAPI.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using NLog;
using NLog.Web;

var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);

    // 配置 NLog
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // Add services to the container.
    builder.Services.AddControllers();

    // 根据当前环境加载配置文件（Development、Production 等）
    builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

    // Configure MySQL database
    var connectionString = builder.Configuration.GetConnectionString("DevConnection");
    builder.Services.AddDbContext<FlightDbContext>(options =>
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

    // Register the LoggingFilter globally
    builder.Services.AddControllers(options =>
    {
        options.Filters.Add<LogFilter>();
        options.Filters.Add<ExceptionFilter>();
    });

    builder.Services.AddScoped<IFlightService, FlightService>();

    // Swagger
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Flight API", Version = "v1" });
        // Add XML comments if available
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
    });

    // Register AutoMapper
    builder.Services.AddAutoMapper(typeof(FlightMapping).Assembly);

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception exception)
{
    // 捕获启动异常
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // 确保在应用程序关闭时关闭 NLog
    LogManager.Shutdown();
}
