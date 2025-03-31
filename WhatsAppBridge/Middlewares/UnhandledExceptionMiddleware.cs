namespace WhatsAppBridge.Middlewares;
public class UnhandledExceptionMiddleware(RequestDelegate next, ILogger<UnhandledExceptionMiddleware> logger) {
    
    private readonly RequestDelegate _next = next;
    private readonly ILogger<UnhandledExceptionMiddleware> _logger = logger;

    public async Task Invoke(HttpContext context) {
        try {
            await _next(context);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Unhandled exception occurred.");
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync("{\"error\": \"An unexpected error occurred.\"}");
        }
    }
}
