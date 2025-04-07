using Serilog;
using Serilog.Events;

namespace Logging;

public static class LoggerConfigurator {
    public static void ConfigureLogger(bool isDevelopment, string storageConnectionString, string storageTableName = "Logs") {
        var loggerConfiguration = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Debug();

        if (isDevelopment) {
            loggerConfiguration.WriteTo.Console();
        }
        else {
            loggerConfiguration
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .WriteTo.AzureTableStorage(storageConnectionString, storageTableName: storageTableName,
                restrictedToMinimumLevel: LogEventLevel.Warning);
        }

        Log.Logger = loggerConfiguration.CreateLogger();
    }
}