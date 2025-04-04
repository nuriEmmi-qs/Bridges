using Microsoft.Extensions.Options;
using Serilog;
using WhatsAppBridge.Filters;
using WhatsAppBridge.Middlewares;

public static class BuilderExtensions {
    private const string LogStorageFileName = "logs/log-{yyyy}-{MM}-{dd}.txt";
    private const Serilog.Events.LogEventLevel Loglevel = Serilog.Events.LogEventLevel.Information;

    public static IServiceCollection AddAppServices(this IServiceCollection services, ConfigurationManager configuration) {

        services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

        services.AddAppFilters(configuration);

        return services;
    }

    private static IServiceCollection AddAppFilters(this IServiceCollection services, ConfigurationManager configuration) {

        services.AddScoped<LogExecutionFilter>();
        return services;
    }

    private static IApplicationBuilder UseAppMiddlewares(this IApplicationBuilder app, AppSettings appsettings) {
        app.UseMiddleware<UnhandledExceptionMiddleware>();
        return app;
    }
    public static IApplicationBuilder UseApp(this IApplicationBuilder app, AppSettings appSettings, string environmentName) {

        app.UseAppMiddlewares(appSettings);
        if (environmentName == "Development") {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }
        else {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.AzureBlobStorage(
                    connectionString: appSettings.SerilogBlobStorageConnectionString,
                    storageFileName: LogStorageFileName,
                    restrictedToMinimumLevel: Loglevel)
                .CreateLogger();
        }
        //app.UseSerilogRequestLogging(); // her request loglanir.
        return app;
    }
}
