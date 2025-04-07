using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;

namespace WhatsAppBridge;
public class Program {
    private static void Main() {

        var builder = WebApplication.CreateBuilder();

        builder.Services.AddSerilog(); 
        builder.Services.AddControllers();
        builder.Services.AddAppServices(builder.Configuration);

        var app = builder.Build();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.UseApp(app.Services.GetRequiredService<IOptions<BridgesSettings>>().Value,
                   builder.Environment);
        ConfigureLog();

        app.Run();
    }

    private static void ConfigureLog() {

        var homePath = Environment.GetEnvironmentVariable("HOME")
            ?? Path.Combine(Directory.GetCurrentDirectory(), "Logs");

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File(
               path: Path.Combine(homePath, "LogFiles", "Application", "diagnostics.txt"),
               rollingInterval: RollingInterval.Day,
               fileSizeLimitBytes: 10 * 1024 * 1024,
               retainedFileCountLimit: 2,
               rollOnFileSizeLimit: true,
               shared: true,
               flushToDiskInterval: TimeSpan.FromSeconds(1))
            .CreateLogger();
    }
}
