using Serilog.Events;
using Serilog;
using WhatsAppBridge.Filters;
using WhatsAppBridge.Middlewares;

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

        const string logDefaultTableName = "Logs";

        string storageConnectionString = builder.Configuration.GetConnectionString("StorageConnectionString");
        var storageTableName = builder.Configuration["StorageLogTableName"] ?? logDefaultTableName;
        
        var logEventLevel = builder.Configuration.GetValue("Logging:LogLevel:WhatsAppBridge", LogEventLevel.Warning);

        var loggerConfig = new LoggerConfiguration();
        if (builder.Environment.IsDevelopment()) {
            loggerConfig.WriteTo.Console(restrictedToMinimumLevel: logEventLevel);
        }
        else {
            loggerConfig.WriteTo.AzureTableStorage(
                storageTableName: storageTableName,
                connectionString: storageConnectionString,
                restrictedToMinimumLevel: logEventLevel
            );
        }

        Log.Logger = loggerConfig.CreateLogger();
        builder.Host.UseSerilog();
    }
}
