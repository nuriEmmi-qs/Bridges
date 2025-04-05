using WhatsAppBridge.Filters;
using WhatsAppBridge.Middlewares;
namespace WhatsAppBridge;

public static class BuilderExtensions {

    public static IServiceCollection AddAppServices(this IServiceCollection services, ConfigurationManager configuration) {

        services.Configure<BridgesSettings>(configuration.GetSection(nameof(BridgesSettings)));

        //filters
        services.AddScoped<LogExecutionFilter>();

        return services;
    }

    public static IApplicationBuilder UseApp(this IApplicationBuilder app, BridgesSettings appSettings, IWebHostEnvironment environment) {

        app.UseMiddleware<UnhandledExceptionMiddleware>();
        return app;
    }
}
