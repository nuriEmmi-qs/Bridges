using WhatsAppBridge.Filters;
using WhatsAppBridge.Middlewares;

namespace WhatsAppBridge;

public static class ProgramExtensions {


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

}