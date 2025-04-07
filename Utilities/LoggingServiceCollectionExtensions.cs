using Microsoft.Extensions.DependencyInjection;
using Serilog.Events;
using Serilog;

namespace Utilities;
public static class LoggingServiceCollectionExtensions {
    public static IServiceCollection UseLog(this IServiceCollection services, bool isDevelopment) {

        var loggerConfiguration = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Information()
            .WriteTo.Console();

        if (!isDevelopment) {
            loggerConfiguration.MinimumLevel.Override("Microsoft", LogEventLevel.Warning);

            var homePath = Environment.GetEnvironmentVariable("HOME")
                ?? Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            //
            var filePath = Path.Combine(homePath, "LogFiles", "Application", "diagnostics.txt");

            loggerConfiguration.WriteTo.File(
                path: filePath,
                rollingInterval: RollingInterval.Day,
                fileSizeLimitBytes: 10 * 1024 * 1024,
                retainedFileCountLimit: 2,
                rollOnFileSizeLimit: true,
                shared: true,
                flushToDiskInterval: TimeSpan.FromSeconds(1),
                restrictedToMinimumLevel: LogEventLevel.Warning);
        }

        Log.Logger = loggerConfiguration.CreateLogger();

        services.AddSerilog();

        return services;

    }
}
