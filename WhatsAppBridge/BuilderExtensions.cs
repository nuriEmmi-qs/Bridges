using WhatsAppBridge.Filters;
using WhatsAppBridge.Middlewares;

public static class BuilderExtensions {

    public static IServiceCollection AddAppServices(this IServiceCollection services, ConfigurationManager configuration) {

        services.Configure<ApiSettings>(configuration.GetSection(nameof(ApiSettings)));

        //filters
        services.AddScoped<LogExecutionFilter>();

        return services;
    }

    public static IApplicationBuilder UseApp(this IApplicationBuilder app, ApiSettings appSettings, IWebHostEnvironment environment) {

        app.UseMiddleware<UnhandledExceptionMiddleware>();
        return app;
    }
}
