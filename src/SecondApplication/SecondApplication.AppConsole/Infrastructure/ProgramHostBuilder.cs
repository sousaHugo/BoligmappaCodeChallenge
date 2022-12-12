using BCCP.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SecondApplication.Application;
using SecondApplication.Infrastructure;
using Serilog;

namespace SecondApplication.AppConsole.Infrastructure;

public static class ProgramHostBuilder
{
    public static IHostBuilder CreateHostBuilder(string[] args = null)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug().MinimumLevel.Error().MinimumLevel.Information()
            .WriteTo.File(LogConstants.LogFileDestination, rollingInterval: RollingInterval.Day).CreateLogger();

        var hostBuilder = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, builder) =>
            {
                builder.SetBasePath(Directory.GetCurrentDirectory());
                builder.AddJsonFile("appsettings.json", optional: false);

            }).ConfigureServices((context, services) =>
            {
                services.AddMediatR(typeof(Program));
                services.AddApplicationServices();
                services.AddInfrastructureServices(context.Configuration);

            })
            .ConfigureLogging((_, logging) => logging.ClearProviders()).UseSerilog();

        return hostBuilder;
    }
}