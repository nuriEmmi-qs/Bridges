using Serilog.Events;
using Serilog;
using WhatsAppBridge.Filters;
using WhatsAppBridge.Middlewares;
using System;

namespace WhatsAppBridge;

public static class ProgramExtensions {


    public static IServiceCollection AddAppServices(this IServiceCollection services, ConfigurationManager configuration, bool isDevelopment) {

        ConfigureLog(isDevelopment);

        services.AddSerilog();

        services.Configure<BridgesSettings>(configuration.GetSection(nameof(BridgesSettings)));

        //filters
        services.AddScoped<LogExecutionFilter>();
        //logs icin logforbridges storage account'ta
        return services;
    }

    public static IApplicationBuilder UseApp(this IApplicationBuilder app, BridgesSettings appSettings, IWebHostEnvironment environment) {

        app.UseMiddleware<UnhandledExceptionMiddleware>();
        return app;
    }

    private static void ConfigureLog(bool isDevelopment) {

        var loggerConfiguration = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Information()
            .WriteTo.Console();

        if (!isDevelopment) {
            loggerConfiguration.MinimumLevel.Override("Microsoft", LogEventLevel.Warning);

            var homePath = Environment.GetEnvironmentVariable("HOME")
                ?? Path.Combine(Directory.GetCurrentDirectory(), "Logs");

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
    }




}