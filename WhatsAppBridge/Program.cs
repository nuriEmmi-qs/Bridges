using Microsoft.Extensions.Options;
using Serilog;

namespace WhatsAppBridge;
public class Program {
    private static void Main() {

        var builder = WebApplication.CreateBuilder();

        builder.Services.AddControllers();

        builder.Services.AddAppServices(builder.Configuration, builder.Environment.IsDevelopment());

        var app = builder.Build();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.UseApp();

        app.Run();
    }
}
