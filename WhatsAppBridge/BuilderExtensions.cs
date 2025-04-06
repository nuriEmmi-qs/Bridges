using Serilog.Events;
using Serilog;
using WhatsAppBridge.Filters;
using WhatsAppBridge.Middlewares;
using WhatsAppBridge.Utilities;

namespace WhatsAppBridge;

public static class BuilderExtensions {


    public static IServiceCollection AddAppServices(this IServiceCollection services, ConfigurationManager configuration) {

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

    public static void SetupLogger(this WebApplicationBuilder builder) {
        
        if (builder.Environment.IsDevelopment()) {
            var loggerConfig = new LoggerConfiguration();
            loggerConfig.WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information);
            Log.Logger = loggerConfig.CreateLogger();
        }
        else {
            string storageConnectionString = builder.Configuration.GetConnectionString("StorageConnectionString");
            var storageTableName = builder.Configuration["StorageLogTableName"] ?? "Logs";
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Warning()
                .WriteTo.Sink(new SimpleAzureTableSink(storageConnectionString, storageTableName))
                .CreateLogger();
        }
        builder.Host.UseSerilog();
    }
}




