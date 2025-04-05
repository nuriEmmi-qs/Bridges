using Microsoft.Extensions.Options;

namespace WhatsAppBridge;
public class Program {
    private static void Main() {

        var builder = WebApplication.CreateBuilder();

        builder.SetupLogger();

        builder.Services.AddControllers();
        builder.Services.AddAppServices(builder.Configuration);

        var app = builder.Build();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.UseApp(app.Services.GetRequiredService<IOptions<BridgesSettings>>().Value,
                   builder.Environment);

        app.Run();
    }

}
