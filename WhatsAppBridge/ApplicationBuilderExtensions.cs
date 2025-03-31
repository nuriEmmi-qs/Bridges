using WhatsAppBridge.Filters;
using WhatsAppBridge.Middlewares;

namespace WhatsAppBridge.Extensions;

public static class ApplicationBuilderExtensions {
    public static IServiceCollection AddWhatsAppBridgeServices(this IServiceCollection services) {
        services.AddScoped<LogExecutionFilter>();
        // Add other services here
        return services;
    }

    public static IApplicationBuilder UseWhatsAppBridgeMiddlewares(this IApplicationBuilder app) {
        app.UseMiddleware<UnhandledExceptionMiddleware>();
        // Add other middlewares here
        return app;
    }
}