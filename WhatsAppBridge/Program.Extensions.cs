using WhatsAppBridge.Filters;
using WhatsAppBridge.Middlewares;
using Utilities;

namespace WhatsAppBridge;

public static class ProgramExtensions {


    public static IServiceCollection AddAppServices(this IServiceCollection services, ConfigurationManager configuration, bool isDevelopment) {

        services.Configure<BridgesSettings>(configuration.GetSection(nameof(BridgesSettings)));

        services.UseLog(isDevelopment);

        //filters
        services.AddScoped<LogExecutionFilter>();

        return services;
    }

    public static IApplicationBuilder UseApp(this IApplicationBuilder app) {

        app.UseMiddleware<UnhandledExceptionMiddleware>();
        return app;
    }
}